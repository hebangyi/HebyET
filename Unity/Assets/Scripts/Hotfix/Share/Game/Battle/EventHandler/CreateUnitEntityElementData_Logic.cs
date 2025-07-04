namespace ET
{
    [BattleEvent(WorldMode.Logic)]
    public class CreateUnitEntityElementData_Logic: ABattleEvent<CreateUnitEntityElementData>
    {
        protected override void Run(World world, CreateUnitEntityElementData args)
        {
            var unitEntity = args.UnitEntity;
            var componentId = args.ComponentId;
            
            var logics = BattleUnitEntityDataLogicManagerComponent.Instance.GetInitLogicByComponentId(componentId);
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

