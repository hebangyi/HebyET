namespace ET.Client
{
    [ComponentOf]
    public class FGUIComponent : Entity, IAwake
    {
    }

    [ComponentOf]
    [EnableMethod]
    public class FGUIViewBinder<T> : Entity where T : FGUI
    {
        public T FGUI;
        
        protected async ETTask AddPackAsync()
        {
            // FGUIPackageComponent.Instance.AddPackageSync()
        }
    }
}