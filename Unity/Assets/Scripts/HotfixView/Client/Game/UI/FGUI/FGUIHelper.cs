using FairyGUI;

namespace ET.Client
{
    public static class FGUIHelper
    {
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
