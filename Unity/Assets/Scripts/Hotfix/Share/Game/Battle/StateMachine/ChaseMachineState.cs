using System.Collections.Generic;
using Unity.Mathematics;

namespace ET
{
    [MachineState(MachineStateEnum.Chase)]
    public class ChaseMachineState: IMachineState
    {
        public void Enter(MonsterStateMachineComponent component)
        {
            var unitEntity = component.GetParent<UnitEntity>();
            var monsterRuntimeData = unitEntity.GetUnitEntityLogicElemData<MonsterRuntimeData>();
            var chaseData = monsterRuntimeData.ChaseData;
            
            var unitEntityPlayer = UnitMonsterHelper.NearestPlayer(unitEntity);
            if (unitEntityPlayer == null)
            {
                return;
            }
            
            chaseData.FlowUnitEntityId = unitEntityPlayer.Id;
        }

        public void Execute(MonsterStateMachineComponent component)
        {
            
            var unitEntity = component.GetParent<UnitEntity>();
            var logicWorld = unitEntity.LogicWorld();
            
            var monsterRuntimeData = unitEntity.GetUnitEntityLogicElemData<MonsterRuntimeData>();
            var chaseData = monsterRuntimeData.ChaseData;
            
            var flowUnitEntity = logicWorld.AllEntities.GetValueOrDefault(chaseData.FlowUnitEntityId);
            if (flowUnitEntity == null)
            {
                return;
            }
            
            int speed = 5;
            var currentPosition = unitEntity.GetUnitEntityElemData<UnitEntityPosition>().Position;
            var distance = BattleHelper.Distance(currentPosition, flowUnitEntity.GetUnitEntityElemData<UnitEntityPosition>().Position);
            
            var moveMax = speed * logicWorld.IntervalMillis;
            if (moveMax >= distance)
            {
                unitEntity.GetUnitEntityElemData<UnitEntityPosition>().Position = currentPosition;
            }
            else
            {
                float2 sub = flowUnitEntity.GetUnitEntityElemData<UnitEntityPosition>().Position - currentPosition;
                var subDistance = math.normalize(sub) * speed * logicWorld.IntervalMillis;
                unitEntity.GetUnitEntityElemData<UnitEntityPosition>().Position += subDistance;
            }
            
        }

        public void Exit(MonsterStateMachineComponent component)
        {
        }
    }
}