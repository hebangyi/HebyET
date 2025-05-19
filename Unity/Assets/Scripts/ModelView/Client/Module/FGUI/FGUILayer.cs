using System;
using System.Collections.Generic;
using FairyGUI;

namespace ET.Client
{
    public class FGUILayer : Entity, IAwake<GObject>
    {
        public GObject GObject;
        public Dictionary<string, GObject> FGuiChildNode = new ();

        public void AddFGUIPage(GObject gObject)
        {
            string uiName = gObject.name;
            if (this.FGuiChildNode.ContainsKey(uiName))
            {
                throw new Exception($"ui.Name({uiName}) already exist");
            }
            FGuiChildNode[uiName] = gObject;
            // 挂载
            this.GObject.asCom.AddChild(gObject);
        }
    }
}