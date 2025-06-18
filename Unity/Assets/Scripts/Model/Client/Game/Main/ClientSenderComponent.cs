using System;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class ClientSenderComponent: Entity, IAwake, IDestroy
    {
        public int fiberId;

        public ActorId netClientActorId;

        public static ClientSenderComponent Instance;
    }


    [AttributeUsage(AttributeTargets.Method)]
    public class ResponseHandlerAttribute : BaseAttribute
    {
        public ResponseHandlerAttribute(Type responseType)
        {
            
        }
    }
}