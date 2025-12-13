using System.Diagnostics.CodeAnalysis;
using Unity.Mathematics;

namespace ET
{
    [UnitEntityLogic]
    public class MonsterUpdateStateMachineLogic : ILogicTickUpdate
    {
        public void OnTick(LogicWorld logicWorld)
        {
            // 更新所有怪物状态
            foreach (var unitEntity in logicWorld.Monsters.Values)
            {
                unitEntity.GetComponent<MonsterStateMachineComponent>().Execute();
            }
        }
    }
}