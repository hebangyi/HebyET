using System;

namespace ET.Client
{
    [AttributeUsage(AttributeTargets.Class)]
    public class FGUIEventAttribute: BaseAttribute
    {
        public WindowID WindowID
        {
            get;
        }

        public Type FGUIType
        {
            get;
        }
        
        public FGUIEventAttribute(WindowID windowID, Type FGUIType)
        {
            this.WindowID = windowID;
            this.FGUIType = FGUIType;
        }
    }
}