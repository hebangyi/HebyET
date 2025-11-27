namespace ET
{
    [BattleEvent]
    public class RemoveUnitEntity_DestroyLogic : ABattleEvent<RemoveUnitEntity>
    {
        protected override void Run(LogicWorld logicWorld, RemoveUnitEntity args)
        {
            var unitEntity = args.UnitEntity;
            foreach (var unitEntityElemDataKv in unitEntity.UnitEntityData)
            {
                var elemId = unitEntityElemDataKv.Key;
                var logics = LogicWorldLogicManagerComponent.Instance.GetInitLogicByComponentId(elemId);
                if (logics != null)
                {
                    foreach (var logic in logics)
                    {
                        logic.OnDestroy(unitEntity);
                    }
                }
            }
        }
    }
}