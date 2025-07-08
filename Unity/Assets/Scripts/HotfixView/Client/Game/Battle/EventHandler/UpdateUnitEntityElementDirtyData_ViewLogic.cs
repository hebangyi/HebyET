namespace ET.Client
{
    [BattleEvent(WorldMode.View)]
    public class UpdateUnitEntityElementDirtyData_ViewLogic: ABattleEvent<UpdateUnitEntityElementDirtyData>
    {
        protected override void Run(World world, UpdateUnitEntityElementDirtyData args)
        {
            var unitEntity = args.UnitEntity;
            var componentId = args.ComponentId;

            var logics = BattleUnitEntityViewLogicManagerComponent.Instance.GetUpdateLogicByComponentId(componentId);
            if (logics != null)
            {
                foreach (var logic in logics)
                {
                    logic.OnUpdate(unitEntity, args.OldUnitEntityElemData, args.NewUnitEntityElemData);
                }
            }
        }
    }
}

