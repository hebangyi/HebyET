using System;
using System.Collections.Generic;

namespace ET.Client
{
    [EntitySystemOf(typeof(ClientWorldEventManagerComponent))]
    [FriendOf(typeof(ClientWorldEventManagerComponent))]
    public static partial class ClientWorldEventManagerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ClientWorldEventManagerComponent self)
        {
            ClientWorldEventManagerComponent.Instance = self;
            var types = CodeTypes.Instance.GetAttributeTypes(typeof(ClientWorldEventHandlerAttribute));
            foreach (Type type in types)
            {
                IClientWorldEvent obj = Activator.CreateInstance(type) as IClientWorldEvent;
                if (obj == null)
                {
                    throw new Exception($"type not is BattleEvent: {type.Name}");
                }

                Type eventType = obj.Type;
                var list = self.AllEvents.GetValueOrDefault(eventType);
                if (list == null)
                {
                    list = new List<IClientWorldEvent>();
                    self.AllEvents[eventType] = list;
                }

                list.Add(obj);
            }
        }
    }
}