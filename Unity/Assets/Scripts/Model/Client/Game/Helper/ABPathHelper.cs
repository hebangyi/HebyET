namespace ET.Client
{
    public static class ABPathHelper
    {
        private static readonly string ROOT_PATH = "Assets/Bundles";
        
        
        public static string GetFGUIDescPath(string fileName)
        {
            return $"{ROOT_PATH}/FGUI/{fileName}.bytes";
        }
    }
}

