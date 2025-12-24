using System;
using System.Collections.Generic;

namespace ET
{
    [EntitySystemOf(typeof(BattleEventManagerComponent))]
    [FriendOf(typeof(BattleEventManagerComponent))]
    public static partial class BattleEventManagerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this BattleEventManagerComponent self)
        {

            BattleEventManagerComponent.Instance = self;
        
            var types = CodeTypes.Instance.GetAttributeTypes(typeof(BattleEventAttribute));
            foreach (Type type in types)
            {
                IBattleEvent obj = Activator.CreateInstance(type) as IBattleEvent;
                if (obj == null)
                {
                    throw new Exception($"type not is BattleEvent: {type.Name}");
                }
                
                Type eventType = obj.Type;
                var list = self.AllEvents.GetValueOrDefault(eventType);
                if (list == null)
                {
                    list = new List<IBattleEvent>(); 
                    self.AllEvents[eventType] = list;
                }
                list.Add(obj);
            }
        }
    }
}
