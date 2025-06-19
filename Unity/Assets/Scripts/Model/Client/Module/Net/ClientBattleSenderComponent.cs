namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class ClientBattleSenderComponent: Entity, IAwake, IDestroy
    {
        public int fiberId;
        public ActorId netClientActorId;
        
        public static ClientBattleSenderComponent Instance;
    }
}