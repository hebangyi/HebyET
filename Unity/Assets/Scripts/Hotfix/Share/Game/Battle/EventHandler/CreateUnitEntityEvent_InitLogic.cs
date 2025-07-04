namespace ET
{
    [BattleEvent(WorldMode.Logic)]
    public class CreateUnitEntityEvent_InitLogic : ABattleEvent<CreateUnitEntityEvent>
    {
        protected override void Run(World world, CreateUnitEntityEvent args)
        {
            var unitEntity = args.UnitEntity;
            foreach (var unitEntityElemDataKv in unitEntity.UnitEntityData)
            {
                var compId = unitEntityElemDataKv.Key;
                var logics = BattleUnitEntityDataLogicManagerComponent.Instance.GetInitLogicByComponentId(compId);
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