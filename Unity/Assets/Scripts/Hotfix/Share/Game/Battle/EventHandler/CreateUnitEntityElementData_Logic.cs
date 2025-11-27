/*
namespace ET
{
    [BattleEvent]
    public class CreateUnitEntityElementData_Logic: ABattleEvent<CreateUnitEntityElementData>
    {
        protected override void Run(LogicWorld logicWorld, CreateUnitEntityElementData args)
        {
            var unitEntity = args.UnitEntity;
            var componentId = args.ComponentId;
            
            var logics = BattleUnitEntityLogicManagerComponent.Instance.GetInitLogicByComponentId(componentId);
            if (logics != null)
            {
                foreach (var logic in logics)
                {
                    logic.OnInit(unitEntity);
                }
            }
        }
    }
}
*/

