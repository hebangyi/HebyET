using System;
using System.Collections.Generic;
using System.Linq;

namespace ET
{
    [EntitySystemOf(typeof(BattleUnitEntityLogicManagerComponent))]
    [FriendOf(typeof(BattleUnitEntityLogicManagerComponent))]
    public static partial class BattleUnitEntityLogicManagerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this BattleUnitEntityLogicManagerComponent self)
        {
            BattleUnitEntityLogicManagerComponent.Instance = self;
            
            var logicTypes = CodeTypes.Instance.GetAttributeTypes(typeof(UnitEntityLogicAttribute));
            foreach (var type in logicTypes)
            {
                var handler =  Activator.CreateInstance(type);
                if (handler is IUnitEntityInitLogic dataLogic)
                {
                    var watchComponentIds = dataLogic.WatchComponentIds();
                    foreach (ushort componentId in watchComponentIds)
                    {
                        var logics = self.CompId2InitLogics.GetValueOrDefault(componentId);
                        if (logics == null)
                        {
                            logics = new List<IUnitEntityInitLogic>();
                            self.CompId2InitLogics.Add(componentId, logics);
                        }
                        
                        logics.Add(dataLogic);
                    }
                }


                if (handler is IUnitEntityTickLogic tickLogic)
                {
                    foreach (ushort componentId in tickLogic.WatchComponentIds())
                    {
                        var logics = self.CompId2TickLogics.GetValueOrDefault(componentId);
                        if (logics == null)
                        {
                            logics = new List<IUnitEntityTickLogic>();
                            self.CompId2TickLogics.Add(componentId, logics);
                        }
                        
                        logics.Add(tickLogic);
                    }
                }
            }
        }

        public static List<IUnitEntityInitLogic> GetInitLogicByComponentId(this BattleUnitEntityLogicManagerComponent self, ushort componentId)
        {
            return self.CompId2InitLogics.GetValueOrDefault(componentId);
        }
        
        public static List<IUnitEntityTickLogic> GetTickLogicByComponentId(this BattleUnitEntityLogicManagerComponent self, ushort componentId)
        {
            return self.CompId2TickLogics.GetValueOrDefault(componentId);
        }
        
    }
}