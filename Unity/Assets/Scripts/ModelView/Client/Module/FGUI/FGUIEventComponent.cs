using System;
using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf]
    public class FGUIEventComponent : Entity, IAwake
    {
        public static FGUIEventComponent Instance;
    
        // WindowId 对应的 EventHandler 
        public Dictionary<WindowID, IFGUIEventHandler> WindowID2EventHandlers = new();

        // WindowId 对应的 UIType
        public Dictionary<WindowID, Type> WindowID2FGUITypes = new();

        // WindowId 对应的 packageName 和 ResName
        public Dictionary<WindowID, (string, string)> WindowId2Resources = new ();
        
        // FGUIType 对应的 WindowID
        public Dictionary<Type, WindowID> FGUIType2WindowIds = new();

        
    
    }
}

