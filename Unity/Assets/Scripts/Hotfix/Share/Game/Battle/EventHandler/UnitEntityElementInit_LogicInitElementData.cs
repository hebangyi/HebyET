namespace ET
{
    [BattleEvent]
    public class UnitEntityElementInit_LogicInitElementData : ABattleEvent<LogicInitElementData>
    {
        protected override void Run(LogicWorld logicWorld, LogicInitElementData args)
        {
            var unitEntity = args.UnitEntity;
            foreach (var unitEntityElemDataKv in unitEntity.UnitEntityData)
            {
                var compId = unitEntityElemDataKv.Key;
                var logics = LogicWorldLogicManagerComponent.Instance.GetInitLogicByComponentId(compId);
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