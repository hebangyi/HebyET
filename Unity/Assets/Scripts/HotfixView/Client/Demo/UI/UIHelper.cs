using FairyGUI;

namespace ET.Client
{
    public static class UIHelper
    {
        [EnableAccessEntiyChild]
        public static async ETTask<UI> Create(Entity scene, string uiType, UILayer uiLayer)
        {
            return await scene.GetComponent<UIComponent>().Create(uiType, uiLayer);
        }
        
        [EnableAccessEntiyChild]
        public static async ETTask Remove(Entity scene, string uiType)
        {
            scene.GetComponent<UIComponent>().Remove(uiType);
            await ETTask.CompletedTask;
        }
        
        public static GObject CreateGObject(string packageName, string resourceName)
        {
         	return UIPackage.CreateObject(packageName, resourceName);
        } 

        public static void CreateGObjectAsync(string packageName, string resourceName, UIPackage.CreateObjectCallback result)
        {
            UIPackage.CreateObjectAsync(packageName, resourceName, result);
        }
    }
}