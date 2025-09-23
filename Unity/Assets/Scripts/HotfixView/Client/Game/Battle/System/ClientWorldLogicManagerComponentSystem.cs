using System;
using System.Collections.Generic;

namespace ET.Client
{
    [EntitySystemOf(typeof(ClientWorldLogicManagerComponent))]
    [FriendOf(typeof(ClientWorldLogicManagerComponent))]
    public static partial class ClientWorldLogicManagerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ClientWorldLogicManagerComponent self)
        {
            ClientWorldLogicManagerComponent.Instance = self;

            var viewTypes = CodeTypes.Instance.GetAttributeTypes(typeof(UnitEntityViewLogicAttribute));
            foreach (var type in viewTypes)
            {
                var handler = Activator.CreateInstance(type);
                if (handler is IClientWorldInit viewInitLogic)
                {
                    var componentId = viewInitLogic.WatchComponentId();
                    var logics = self.CompId2InitViewLogics.GetValueOrDefault(componentId);
                    if (logics == null)
                    {
                        logics = new List<IClientWorldInit>();
                        self.CompId2InitViewLogics.Add(componentId, logics);
                    }

                    logics.Add(viewInitLogic);
                }

                if (handler is IClientWorldElementDataUpdateLogic dataUpdateLogic)
                {
                    var componentId = dataUpdateLogic.WatchComponentId();

                    var logics = self.CompId2ElementDataUpdates.GetValueOrDefault(componentId);
                    if (logics == null)
                    {
                        logics = new List<IClientWorldElementDataUpdateLogic>();
                        self.CompId2ElementDataUpdates.Add(componentId, logics);
                    }

                    logics.Add(dataUpdateLogic);
                }
            }
        }

        public static List<IClientWorldInit> GetInitViewLogicByComponentId(this ClientWorldLogicManagerComponent self,
        ushort componentId)
        {
            return self.CompId2InitViewLogics.GetValueOrDefault(componentId);
        }

        public static List<IClientWorldElementDataUpdateLogic> GetUpdateLogicByComponentId(this ClientWorldLogicManagerComponent self,
        ushort componentId)
        {
            return self.CompId2ElementDataUpdates.GetValueOrDefault(componentId);
        }
    }
}