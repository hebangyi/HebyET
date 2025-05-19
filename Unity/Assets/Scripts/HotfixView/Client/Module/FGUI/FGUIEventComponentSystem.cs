using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(FGUIEventComponent))]
    [FriendOf(typeof(FGUIEventComponent))]
    public static partial class  FGUIEventComponentSystem
    {
        [EntitySystem]
        private static void Awake(this FGUIEventComponent self)
        {
            FGUIEventComponent.Instance = self;
            var handlerTypes = CodeTypes.Instance.GetAttributeTypes(typeof(FGUIEventAttribute));

            foreach (var handlerType in handlerTypes)
            {
                IFGUIEventHandler eventHandler = Activator.CreateInstance(handlerType) as IFGUIEventHandler;
                var eventAttribute = handlerType.GetCustomAttribute(typeof(FGUIEventAttribute)) as FGUIEventAttribute;
                self.WindowID2EventHandlers[eventAttribute.WindowID] = eventHandler;
                self.WindowID2UITypes[eventAttribute.WindowID] = eventAttribute.FGUIType;

                var fguiTagAttribute = eventAttribute.FGUIType.GetCustomAttribute(typeof(FGUITagAttribute)) as FGUITagAttribute;
                var packageName = fguiTagAttribute.PackageName;
                var resourceName = fguiTagAttribute.ResourceName;
                self.WindowId2Resources[eventAttribute.WindowID] = (packageName, resourceName);
            }
        }

        public static (string, string) GetWindowPackageAndRes(this FGUIEventComponent self, WindowID windowID)
        {
            return self.WindowId2Resources.GetValueOrDefault(windowID);
        }

        public static IFGUIEventHandler GetEventHandlerByWindowID(this FGUIEventComponent self, WindowID windowID)
        {
            return self.WindowID2EventHandlers.GetValueOrDefault(windowID);
        }
    }
}
