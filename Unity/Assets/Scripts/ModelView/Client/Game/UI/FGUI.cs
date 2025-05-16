using System;

namespace ET.Client
{
    public class FGUI: Entity
    {
        public String UIPackageName = "";
        public String UIResourceName = "";
        public String UIResURL = "";
        public String FUIName = "";
    }


    public class FGUITagAttribute: BaseAttribute
    {
        public String PackageName {get;}
        public String ResourceName {get;}

        public FGUITagAttribute(String PackageName, String ResourceName)
        {
            this.PackageName = PackageName;
            this.ResourceName = ResourceName;
        }
    }
}

