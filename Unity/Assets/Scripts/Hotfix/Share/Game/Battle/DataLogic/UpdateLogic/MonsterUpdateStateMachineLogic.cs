using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.Mathematics;

namespace ET
{
    [UnitEntityLogic]
    public class MonsterUpdateStateMachineLogic : ILogicTickUpdate
    {
        public void OnTick(LogicWorld logicWorld)
        {
            // 执行怪物AI选择器
            
            foreach (UnitEntity unitEntity in logicWorld.GetEntityIdsWithDataType(typeof(MonsterRuntimeData)))
            {
                // 执行怪物的状态器
                unitEntity.GetComponent<MonsterAIComponent>().MonsterAIAgent.btexec();
                unitEntity.GetComponent<MonsterStateMachineComponent>().Execute();
            }
        }
    }
}