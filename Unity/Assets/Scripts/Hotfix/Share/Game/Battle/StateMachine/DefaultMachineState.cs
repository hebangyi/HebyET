using Unity.Mathematics;

namespace ET
{
    [MachineState(MachineStateEnum.Default)]
    public class DefaultMachineState : IMachineState
    {
        public void Enter(MonsterStateMachineComponent component)
        {
            Log.Info("Enter Default");
        }

        public void Execute(MonsterStateMachineComponent component)
        {
            var unitEntity = component.GetParent<UnitEntity>();
            var logicWorld = unitEntity.LogicWorld();
            
            var monsterRuntimeAIData = unitEntity.GetUnitEntityLogicElemData<MonsterRuntimeAIData>();
            if (monsterRuntimeAIData.AIStatus == MonsterAIStatus.None)
            {
                monsterRuntimeAIData.AIStatus = MonsterAIStatus.Patrol;

                monsterRuntimeAIData.PatrolData.patrolStatus = PatrolStatus.Run;
                monsterRuntimeAIData.PatrolData.ToPosition =
                        unitEntity.GetUnitEntityElemData<UnitEntityPosition>().Position + new float2(10f, 10f);
            }

            // 更新巡逻状态 怪物坐标
            if (monsterRuntimeAIData.AIStatus == MonsterAIStatus.Patrol)
            {
                int speed = 1;
                var patrolData = monsterRuntimeAIData.PatrolData;
                var currentPosition = unitEntity.GetUnitEntityElemData<UnitEntityPosition>().Position;
                if (patrolData.patrolStatus == PatrolStatus.Run)
                {
                    var moveMax = speed * logicWorld.IntervalMillis;
                    var distance = BattleHelper.Distance(currentPosition, patrolData.ToPosition);
                    if (moveMax >= distance)
                    {
                        unitEntity.GetUnitEntityElemData<UnitEntityPosition>().Position = patrolData.ToPosition;
                        patrolData.patrolStatus = PatrolStatus.Idle;
                        patrolData.IdleFinishTime = logicWorld.NowMilliSeconds + 10 * 1000;
                    }
                    else
                    {
                        float2 sub = (patrolData.ToPosition - currentPosition);
                        var subDistance = math.normalize(sub) * speed * logicWorld.IntervalMillis;
                        unitEntity.GetUnitEntityElemData<UnitEntityPosition>().Position += subDistance;
                    }
                }
                else if (patrolData.patrolStatus == PatrolStatus.Idle)
                {
                    if (patrolData.IdleFinishTime >= logicWorld.NowMilliSeconds)
                    {
                        monsterRuntimeAIData.PatrolData.patrolStatus = PatrolStatus.Run;
                        monsterRuntimeAIData.PatrolData.ToPosition =
                                unitEntity.GetUnitEntityElemData<UnitEntityPosition>().Position + new float2(10f, 10f);
                    }
                }
            }
        }

        public void Exit(MonsterStateMachineComponent component)
        {
            Log.Info("Exit Default");
        }
    }
}

