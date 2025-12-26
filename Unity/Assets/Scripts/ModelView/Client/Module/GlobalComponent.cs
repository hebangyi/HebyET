using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class GlobalComponent : Entity, IAwake
    {
        [StaticField]
        public static GlobalComponent Instance;
        
        public Transform Global;
        
        ////////////////////////////////////////////////////// Unit
        public Transform Unit { get; set; }
        public Transform Default { get; set; }
        public Transform Env { get; set; }
        public Transform Plant { get; set; }
        public Transform Player { get; set; }
        public Transform Monster { get; set; }
        
        
        ////////////////////////////////////////////////////// Unit
        
        
        ////////////////////////////////////////////////////// UI
        public Transform UI { get; set; }
        public Camera UICamera { get; set; }
        ////////////////////////////////////////////////////// UI
        
        // 相机外包装类 通常用于旋转
        public GameObject CameraPack { get; set; }
        
        public Camera MainCamera { get; set; }
        

        public GlobalConfig GlobalConfig { get; set; }
    }
}