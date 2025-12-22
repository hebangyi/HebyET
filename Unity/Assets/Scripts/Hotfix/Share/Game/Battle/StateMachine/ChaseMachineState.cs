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
            
            unitEntity.ChangeAnimateStatus(AnimateStateEnum.Run);
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
            var toPosition = UnitMonsterHelper.ToPosition(logicWorld, currentPosition, flowUnitEntity.GetUnitEntityElemData<UnitEntityPosition>().Position, speed);
            unitEntity.GetUnitEntityElemData<UnitEntityPosition>().Position = toPosition;
        }

        public void Exit(MonsterStateMachineComponent component)
        {
        }
    }
}