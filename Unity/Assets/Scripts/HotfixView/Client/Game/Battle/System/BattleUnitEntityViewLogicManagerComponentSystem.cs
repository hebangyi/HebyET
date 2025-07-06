using System;
using System.Collections.Generic;

namespace ET.Client
{
    [EntitySystemOf(typeof(BattleUnitEntityViewLogicManagerComponent))]
    [FriendOf(typeof(BattleUnitEntityViewLogicManagerComponent))]
    public static partial class BattleUnitEntityViewLogicManagerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this BattleUnitEntityViewLogicManagerComponent self)
        {
            BattleUnitEntityViewLogicManagerComponent.Instance = self;
            
            var viewTypes = CodeTypes.Instance.GetAttributeTypes(typeof(UnitEntityViewLogicAttribute));
            foreach (var type in viewTypes)
            {
                var handler =  Activator.CreateInstance(type);
                if (handler is IUnitEntityViewInitLogic viewInitLogic)
                {
                    var watchComponentIds = viewInitLogic.WatchComponentIds();
                    foreach (ushort componentId in watchComponentIds)
                    {
                        var logics = self.CompId2InitViewLogics.GetValueOrDefault(componentId);
                        if (logics == null)
                        {
                            logics = new List<IUnitEntityViewInitLogic>();
                            self.CompId2InitViewLogics.Add(componentId, logics);
                        }
                        logics.Add(viewInitLogic);
                    }
                }
                
                
                if (handler is IUnitEntityViewElementDataUpdateLogic dataUpdateLogic)
                {
                    foreach (ushort componentId in dataUpdateLogic.WatchComponentIds())
                    {
                        var logics = self.CompId2ElementDataUpdates.GetValueOrDefault(componentId);
                        if (logics == null)
                        {
                            logics = new List<IUnitEntityViewElementDataUpdateLogic>();
                            self.CompId2ElementDataUpdates.Add(componentId, logics);
                        }
                        
                        logics.Add(dataUpdateLogic);
                    }
                }
            }
        }
        
        public static List<IUnitEntityViewInitLogic> GetInitViewLogicByComponentId(this BattleUnitEntityViewLogicManagerComponent self, ushort componentId)
        {
            return self.CompId2InitViewLogics.GetValueOrDefault(componentId);
        }
        
        public static List<IUnitEntityViewElementDataUpdateLogic> GetUpdateLogicByComponentId(this BattleUnitEntityViewLogicManagerComponent self, ushort componentId)
        {
            return self.CompId2ElementDataUpdates.GetValueOrDefault(componentId);
        }
        
    }
}
