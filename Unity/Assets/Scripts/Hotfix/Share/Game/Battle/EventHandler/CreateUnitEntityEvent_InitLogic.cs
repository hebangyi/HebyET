namespace ET
{
    [BattleEvent(WorldMode.Logic)]
    public class CreateUnitEntityEvent_InitLogic : ABattleEvent<CreateUnitEntityEvent0>
    {
        protected override void Run(World world, CreateUnitEntityEvent0 args)
        {
            var unitEntity = args.UnitEntity;
            foreach (var unitEntityElemDataKv in unitEntity.UnitEntityData)
            {
                var compId = unitEntityElemDataKv.Key;
                var logics = BattleUnitEntityLogicManagerComponent.Instance.GetInitLogicByComponentId(compId);
                if (logics != null)
                {
                    foreach (var logic in logics)
                    {
                        logic.OnInit(args.UnitEntity);
                    }
                }
            }
        }
    }
}