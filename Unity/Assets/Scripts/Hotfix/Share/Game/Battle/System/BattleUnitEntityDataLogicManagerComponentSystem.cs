using System;
using System.Collections.Generic;
using System.Linq;

namespace ET
{
    [EntitySystemOf(typeof(BattleUnitEntityDataLogicManagerComponent))]
    [FriendOf(typeof(BattleUnitEntityDataLogicManagerComponent))]
    public static partial class BattleUnitEntityDataLogicManagerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this BattleUnitEntityDataLogicManagerComponent self)
        {
            BattleUnitEntityDataLogicManagerComponent.Instance = self;
            
            var logicTypes = CodeTypes.Instance.GetAttributeTypes(typeof(UnitEntityLogicAttribute));
            foreach (var type in logicTypes)
            {
                var handler =  Activator.CreateInstance(type);
                if (handler is IUnitEntityInitLogic dataLogic)
                {
                    foreach (ushort componentId in dataLogic.WatchComponentIds())
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

        public static List<IUnitEntityInitLogic> GetInitLogicByComponentId(this BattleUnitEntityDataLogicManagerComponent self, ushort componentId)
        {
            return self.CompId2InitLogics.GetValueOrDefault(componentId);
        }
        
        public static List<IUnitEntityTickLogic> GetTickLogicByComponentId(this BattleUnitEntityDataLogicManagerComponent self, ushort componentId)
        {
            return self.CompId2TickLogics.GetValueOrDefault(componentId);
        }
        
    }
}