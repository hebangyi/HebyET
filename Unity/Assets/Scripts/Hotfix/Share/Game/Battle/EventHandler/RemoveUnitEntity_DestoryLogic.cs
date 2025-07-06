namespace ET
{
    [BattleEvent(WorldMode.Logic)]
    public class RemoveUnitEntity_DestroyLogic : ABattleEvent<RemoveUnitEntity>
    {
        protected override void Run(World world, RemoveUnitEntity args)
        {
            var unitEntity = args.UnitEntity;
            foreach (var unitEntityElemDataKv in unitEntity.UnitEntityData)
            {
                var elemId = unitEntityElemDataKv.Key;
                var logics = BattleUnitEntityLogicManagerComponent.Instance.GetInitLogicByComponentId(elemId);
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