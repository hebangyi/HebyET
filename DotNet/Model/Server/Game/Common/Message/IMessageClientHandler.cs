using System;

namespace ET
{
    public interface IMessageClientHandler
    {
        void Handle(Entity entity, object message);
        
        Type GetRequestType();
        
        Type GetResponseType();
    }
}
