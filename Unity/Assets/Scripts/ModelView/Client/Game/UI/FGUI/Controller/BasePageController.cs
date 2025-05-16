using System;

namespace ET.Client
{
    public abstract class BasePageController
    {
        public bool isOpen = false;
        protected abstract ETTask ViewOpen(params object[] param);
    }


    public class BasePageController<View> :BasePageController where View : FGUI
    {
        protected View _view;
        protected object[] args;
        
        protected override async ETTask ViewOpen(params object[] args)
        {
            this.args = args;
            this.OnPreViewOpen();
            this.OnViewOpen();
            await ETTask.CompletedTask;
        }

        protected virtual void OnPreViewOpen()
        {
        }


        protected virtual void OnViewOpen()
        {
            this.isOpen = true;
        }
        
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class FGUIViewControllerAttribute : Attribute
    {
        public ViewType ViewType { get; }
        
        public FGUIViewControllerAttribute(ViewType type)
        {
            this.ViewType = type;
        }
    }
}

