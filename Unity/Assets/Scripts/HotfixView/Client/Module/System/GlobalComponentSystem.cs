using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof(GlobalComponent))]
    [EntitySystemOf(typeof(GlobalComponent))]
    public static partial class GlobalComponentSystem
    {
        [EntitySystem]
        public static void Awake(this GlobalComponent self)
        {
            GlobalComponent.Instance = self;
            
            self.Global = GameObject.Find("/Global").transform;
            self.CameraPack = GameObject.Find("/Global/CameraPack");
            self.MainCamera = GameObject.Find("/Global/CameraPack/MainCamera").GetComponent<Camera>();
            // self.UICamera = GameObject.Find("/Global/UICamera").GetComponent<Camera>();
            // self.UI = GameObject.Find("/Global/UI").transform;
            self.Unit = GameObject.Find("/Global/Unit").transform;
            self.Default = GameObject.Find("/Global/Unit/Default").transform;
            self.Env = GameObject.Find("/Global/Unit/Env").transform;
            self.Plant = GameObject.Find("/Global/Unit/Plant").transform;
            self.Player = GameObject.Find("/Global/Unit/Player").transform;
            self.Monster = GameObject.Find("/Global/Unit/Monster").transform;
            
            self.GlobalConfig = Resources.Load<GlobalConfig>("GlobalConfig");
        }
    }
}
