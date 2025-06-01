using System;

namespace ET.Client
{
    [AttributeUsage(AttributeTargets.Class)]
    public class FGUIEventAttribute: BaseAttribute
    {
        public Type FGUIType
        {
            get;
        }
        
        public FGUIEventAttribute(Type FGUIType)
        {
            this.FGUIType = FGUIType;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class FGUIDLGAttribute : BaseAttribute
    {
        public WindowID WindowID
        {
            get;
        }

        public Type FGUIType
        {
            get;
        }
        
        public FGUIDLGAttribute(WindowID windowID, Type FGUIType)
        {
            this.WindowID = windowID;
            this.FGUIType = FGUIType;
        }
    }
}