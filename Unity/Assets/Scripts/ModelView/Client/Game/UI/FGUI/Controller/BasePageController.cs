/*
using System;

namespace ET.Client
{
    public abstract class BasePageController
    {
        public bool isOpen = false;
        protected abstract ETTask ViewOpen(object[] args);
    }


    public class BasePageController<View> :BasePageController where View : FGUI
    {
        protected View _view;
        protected object[] args;
        
        protected override async ETTask ViewOpen(object[] args1)
        {
            this.args = args1;
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
    public class FGUIViewControllerAttribute : BaseAttribute
    {
        public WindowID WindowId { get; }
        
        public FGUIViewControllerAttribute(WindowID id)
        {
            this.WindowId = id;
        }
    }
}
*/

