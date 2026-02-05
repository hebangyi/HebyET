using System;
using System.Collections.Generic;
using FairyGUI;

namespace ET.Client
{
    [EnableMethod]
    [ChildOf]
    public class FGUILayer : Entity, IAwake<GObject>
    {
        public GObject GObject;
        public Dictionary<string, GObject> FGuiChildNode = new ();

        public void AddWindow(GObject gObject)
        {
            if (this.FGuiChildNode.ContainsKey(gObject.id))
            {
                throw new Exception($"ui.Name({gObject.id}) already exist");
            }
            FGuiChildNode[gObject.id] = gObject;
            // 挂载
            this.GObject.asCom.AddChild(gObject);
        }

        public void RemoveWindow(GObject gObject)
        {
            this.FGuiChildNode.Remove(gObject.id);
            gObject.RemoveFromParent();
        }
    }
}