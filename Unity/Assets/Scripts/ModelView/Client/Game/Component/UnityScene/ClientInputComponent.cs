namespace ET.Client
{
    [ComponentOf(typeof(UnityScene))]
    public class ClientInputComponent: Entity, IAwake, IUpdate, IDestroy
    {
        public static ClientInputComponent Instance;
    }
}