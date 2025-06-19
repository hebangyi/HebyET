using System;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class ClientLobbySenderComponent: Entity, IAwake, IDestroy
    {
        public int fiberId;

        public ActorId netClientActorId;

        public static ClientLobbySenderComponent Instance;
    }
}