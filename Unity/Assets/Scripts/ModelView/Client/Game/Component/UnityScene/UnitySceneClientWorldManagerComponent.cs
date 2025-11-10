namespace ET.Client
{
    [ComponentOf(typeof(UnityScene))]
    public class UnitySceneClientWorldManagerComponent: Entity, IAwake, IDestroy
    {
        public static UnitySceneClientWorldManagerComponent Instance;
        
        public ClientWorld CurrentClientWorld { get; set; }
    }
}

