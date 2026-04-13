using System;
using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class ClientBattleSenderComponent: Entity, IAwake, IDestroy
    {
        public int fiberId;
        public ActorId netClientActorId;
        
        public static ClientBattleSenderComponent Instance;
        
        public Dictionary<Type, ClientBattleQueueMessage> Type2ClientMessage = new ();
        
        public UpdateLogicManagerComponent.TaskIntervalUpdateContext TimerContext;
    }
    
    
    
    public class ClientBattleQueueMessage
    {
        public bool IsSend = false;
        public Type RequestType;
        public IRequest Request;
        public ETTask<IResponse> Response;
    }
}