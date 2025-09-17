namespace ET
{
    [BattleEvent]
    public class CreateUnitEntityEvent_InitLogic : ABattleEvent<CreateUnitEntityEvent0>
    {
        protected override void Run(LogicWorld logicWorld, CreateUnitEntityEvent0 args)
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