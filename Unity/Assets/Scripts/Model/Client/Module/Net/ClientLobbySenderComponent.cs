using System;
using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class ClientLobbySenderComponent: Entity, IAwake, IDestroy
    {
        public int fiberId;

        public ActorId netClientActorId;

        public static ClientLobbySenderComponent Instance;

        public bool IsSending;
        
        // 需要发送信息的队列
        public Queue<ClientLobbyQueueMessage> SendMessageQueue = new();
    }

    public class ClientLobbyQueueMessage
    {
        public Type RequestType;
        public IRequest Request;
        public ETTask<IResponse> Response;
    }
}