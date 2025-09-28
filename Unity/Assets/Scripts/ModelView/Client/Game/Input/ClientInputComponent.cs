namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class ClientInputComponent: Entity, IAwake, IUpdate
    {
        public static ClientInputComponent Instance;
    }
}