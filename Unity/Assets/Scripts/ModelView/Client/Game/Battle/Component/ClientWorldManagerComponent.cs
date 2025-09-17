namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class ClientWorldManagerComponent: Entity, IAwake
    {
        public static ClientWorldManagerComponent Instance;
        
        public ClientWorld CurrentClientWorld { get; set; }
    }
}

