using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class UnitySceneManagerComponent: Entity, IAwake, IUpdate
    {
        public static UnitySceneManagerComponent Instance;
    
        public Dictionary<UnitySceneEnum, IUnitySceneContext> UnitySceneContexts = new ();
        
        private EntityRef<UnityScene> unityScene;

        public UnityScene UnityScene
        {
            get
            {
                return this.unityScene;
            }
            set
            {
                this.unityScene = value;
            }
        }
    
    }
}

