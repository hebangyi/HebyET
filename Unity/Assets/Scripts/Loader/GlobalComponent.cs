using UnityEngine;

namespace ET
{
    [FriendOf(typeof(GlobalComponent))]
    public static partial class GlobalComponentSystem
    {
        [EntitySystem]
        public static void Awake(this GlobalComponent self)
        {
            GlobalComponent.Instance = self;
            
            
            self.Global = GameObject.Find("/Global").transform;
            self.MainCamera = GameObject.Find("/Global/MainCamera").GetComponent<Camera>();
            // self.UICamera = GameObject.Find("/Global/UICamera").GetComponent<Camera>();
            // self.UI = GameObject.Find("/Global/UI").transform;
            self.Unit = GameObject.Find("/Global/Unit").transform;
            
            self.GlobalConfig = Resources.Load<GlobalConfig>("GlobalConfig");
        }
    }

    [ComponentOf(typeof(Scene))]
    public class GlobalComponent : Entity, IAwake
    {
        [StaticField]
        public static GlobalComponent Instance;
        
        public Transform Global;
        public Transform Unit { get; set; }
        public Transform UI { get; set; }
        public Camera UICamera { get; set; }
        public Camera MainCamera { get; set; }
        
        
        

        public GlobalConfig GlobalConfig { get; set; }
    }
}