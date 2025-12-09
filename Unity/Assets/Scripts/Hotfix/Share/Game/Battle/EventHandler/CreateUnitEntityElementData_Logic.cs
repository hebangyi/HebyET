using System.Collections.Generic;

namespace ET
{
    [BattleEvent]
    public class CreateUnitEntityElementData_Logic: ABattleEvent<CreateUnitEntityEvent0>
    {
        protected override void Run(LogicWorld logicWorld, CreateUnitEntityEvent0 args)
        {
            var unitEntity = args.UnitEntity;
            var unitEntityContext = LogicWorldLogicManagerComponent.Instance.UnitEntityContexts.GetValueOrDefault(args.UEType);
            if (unitEntityContext == null)
            {
                Log.Warning($"Create UnitEntity Error , Not Found Type : {args.UEType} Logic Context");
                return ;
            }
            
            unitEntityContext.InitCommonData(unitEntity);
            unitEntityContext.InitCustomData(unitEntity);
            unitEntityContext.Init(unitEntity);
        }
    }
}


