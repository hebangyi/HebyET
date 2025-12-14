using Unity.Mathematics;

namespace ET
{
    [MachineState(MachineStateEnum.Patrol)]
    public class PatrolMachineState : IMachineState
    {
        public void Enter(MonsterStateMachineComponent component)
        {
            var unitEntity = component.GetParent<UnitEntity>();
            Log.Info($"UnitEntity {unitEntity.InsId} Enter Patrol");
            this.GenPatrolData(component, PatrolStatus.Run);
        }

        public void Execute(MonsterStateMachineComponent component)
        {
            var unitEntity = component.GetParent<UnitEntity>();
            var logicWorld = unitEntity.LogicWorld();
            var monsterRuntimeAIData = unitEntity.GetUnitEntityLogicElemData<MonsterRuntimeData>();
            var patrolData = monsterRuntimeAIData.PatrolData;
            
            if (patrolData.PatrolStatus == PatrolStatus.Idle)
            {
                if (logicWorld.NowMilliTime >= patrolData.IdleFinishTime)
                {
                    this.GenPatrolData(component, PatrolStatus.Run);
                }
            }
            else
            {
                // 更新巡逻状态 怪物坐标
                int speed = 3;
                var currentPosition = unitEntity.GetUnitEntityElemData<UnitEntityPosition>().Position;

                var moveMax = speed * logicWorld.IntervalMillis;
                var distance = BattleHelper.Distance(currentPosition, patrolData.ToPosition);
                if (moveMax >= distance)
                {
                    unitEntity.GetUnitEntityElemData<UnitEntityPosition>().Position = patrolData.ToPosition;
                    this.GenPatrolData(component, PatrolStatus.Idle);
                }
                else
                {
                    float2 sub = (patrolData.ToPosition - currentPosition);
                    var subDistance = math.normalize(sub) * speed * logicWorld.IntervalMillis;
                    unitEntity.GetUnitEntityElemData<UnitEntityPosition>().Position += subDistance;
                }
            }
        }

        public void Exit(MonsterStateMachineComponent component)
        {
            
        }

        // 设置巡逻数据
        public void GenPatrolData(MonsterStateMachineComponent component, PatrolStatus patrolStatus)
        {
            var unitEntity = component.GetParent<UnitEntity>();
            var monsterRuntimeData = unitEntity.GetUnitEntityLogicElemData<MonsterRuntimeData>();
            var patrolData = monsterRuntimeData.PatrolData;
            
            if (patrolStatus == PatrolStatus.Run)
            {
                var bornPosition = monsterRuntimeData.BornPosition;
                float patrolRadius = 30f;
                var targetPosition = BattleHelper.InnerCircleRandPoint(patrolRadius) + bornPosition;
                patrolData.ToPosition = targetPosition;    
            }
            else
            {
                patrolData.IdleFinishTime = TimeInfo.Instance.NowMillTime() + 5 * 1000;
            }
            
            patrolData.PatrolStatus = patrolStatus;
        }
    }
}