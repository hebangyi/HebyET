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
                var handler = Activator.CreateInstance(type);
                if (handler is IUnitEntityClientWorldInitLogic viewInitLogic)
                {
                    var componentId = viewInitLogic.WatchComponentId();
                    var logics = self.CompId2InitViewLogics.GetValueOrDefault(componentId);
                    if (logics == null)
                    {
                        logics = new List<IUnitEntityClientWorldInitLogic>();
                        self.CompId2InitViewLogics.Add(componentId, logics);
                    }

                    logics.Add(viewInitLogic);
                }

                if (handler is IUnitEntityClientWorldElementDataUpdateLogic dataUpdateLogic)
                {
                    var componentId = dataUpdateLogic.WatchComponentId();

                    var logics = self.CompId2ElementDataUpdates.GetValueOrDefault(componentId);
                    if (logics == null)
                    {
                        logics = new List<IUnitEntityClientWorldElementDataUpdateLogic>();
                        self.CompId2ElementDataUpdates.Add(componentId, logics);
                    }

                    logics.Add(dataUpdateLogic);
                }
            }
        }

        public static List<IUnitEntityClientWorldInitLogic> GetInitViewLogicByComponentId(this BattleUnitEntityViewLogicManagerComponent self,
        ushort componentId)
        {
            return self.CompId2InitViewLogics.GetValueOrDefault(componentId);
        }

        public static List<IUnitEntityClientWorldElementDataUpdateLogic> GetUpdateLogicByComponentId(this BattleUnitEntityViewLogicManagerComponent self,
        ushort componentId)
        {
            return self.CompId2ElementDataUpdates.GetValueOrDefault(componentId);
        }
    }
}