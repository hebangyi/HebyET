using System;
using System.Reflection;

namespace ET
{
   [FriendOf(typeof(BuffManagerComponent))]
   [EntitySystemOf(typeof(BuffManagerComponent))]
   public static partial class BuffManagerComponentSystem
   {
       
       [EntitySystem]
       private static void Awake(this BuffManagerComponent self)
       {
           BuffManagerComponent.Instance = self;
           var types = CodeTypes.Instance.GetAttributeTypes(typeof(BuffAttribute));
           foreach (Type type in types)
           {
               var obj = Activator.CreateInstance(type);

               if (obj is IBuffExecutor executor)
               {
                   var buffAttribute = type.GetCustomAttribute(typeof(BuffAttribute)) as BuffAttribute;
                   self.BuffType2BuffConfigs[buffAttribute.BuffHandlerType] = executor;
               }
           }
       }
   }
}