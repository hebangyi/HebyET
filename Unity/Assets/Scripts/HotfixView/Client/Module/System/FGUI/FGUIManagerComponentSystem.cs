using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(FGUIManagerComponent))]
    [FriendOf(typeof(FGUIManagerComponent))]
    public static partial class  FGUIManagerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this FGUIManagerComponent self)
        {
            FGUIManagerComponent.Instance = self;
            
            var dlgTypes = CodeTypes.Instance.GetAttributeTypes(typeof(FGUIDLGAttribute));
            foreach (var dlgType in dlgTypes)
            {
                var dlgAttribute = dlgType.GetCustomAttribute(typeof(FGUIDLGAttribute)) as FGUIDLGAttribute;
                var windowID = dlgAttribute.WindowID;
                var fguiType = dlgAttribute.FGUIType;

                self.WindowID2FGUITypes[windowID] = fguiType;
                self.FGUIType2WindowIds[fguiType] = windowID;
                
                var fguiTagAttribute = fguiType.GetCustomAttribute(typeof(FGUITagAttribute)) as FGUITagAttribute;
                var packageName = fguiTagAttribute.PackageName;
                var resourceName = fguiTagAttribute.ResourceName;
                self.WindowId2Resources[windowID] = (packageName, resourceName);
            }
            
            var handlerTypes = CodeTypes.Instance.GetAttributeTypes(typeof(FGUIEventAttribute));
            foreach (var handlerType in handlerTypes)
            {
                IFGUIEventHandler eventHandler = Activator.CreateInstance(handlerType) as IFGUIEventHandler;
                var eventAttribute = handlerType.GetCustomAttribute(typeof(FGUIEventAttribute)) as FGUIEventAttribute;
                var fguiType = eventAttribute.FGUIType;
                var windowId = self.FGUIType2WindowIds[fguiType];
                self.WindowID2EventHandlers[windowId] = eventHandler;
            }
        }

        public static (string, string) GetWindowPackageAndRes(this FGUIManagerComponent self, WindowID windowID)
        {
            return self.WindowId2Resources.GetValueOrDefault(windowID);
        }

        public static IFGUIEventHandler GetEventHandlerByWindowID(this FGUIManagerComponent self, WindowID windowID)
        {
            return self.WindowID2EventHandlers.GetValueOrDefault(windowID);
        }
    }
}
