namespace ET.Client
{
    [BattleEvent(WorldMode.View)]
    public class CreateUnitEntityElementData_ViewLogic: ABattleEvent<CreateUnitEntityElementData>
    {
        protected override void Run(World world, CreateUnitEntityElementData args)
        {
            var unitEntity = args.UnitEntity;
            var componentId = args.ComponentId;
            
            var logics = BattleUnitEntityViewLogicManagerComponent.Instance.GetInitViewLogicByComponentId(componentId);
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