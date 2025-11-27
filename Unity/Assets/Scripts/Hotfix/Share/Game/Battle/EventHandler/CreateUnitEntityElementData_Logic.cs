using System.Collections.Generic;

namespace ET
{
    [BattleEvent]
    public class CreateUnitEntityElementData_Logic: ABattleEvent<CreateUnitEntityEvent0>
    {
        protected override void Run(LogicWorld logicWorld, CreateUnitEntityEvent0 args)
        {
            var unitEntity = args.UnitEntity;
            
            var unitEntityCommonData = unitEntity.GetUnitEntityElemData<UnitEntityCommonData>();
            var unitEntityContext = LogicWorldLogicManagerComponent.Instance.UnitEntityContexts.GetValueOrDefault(unitEntityCommonData.UnitEntityType);
            if (unitEntityContext == null)
            {
                unitEntityContext = LogicWorldLogicManagerComponent.Instance.UnitEntityContexts.GetValueOrDefault(UETypeEnum.None);
            }
            
            unitEntityContext.Init(unitEntity);
        }
    }
}


