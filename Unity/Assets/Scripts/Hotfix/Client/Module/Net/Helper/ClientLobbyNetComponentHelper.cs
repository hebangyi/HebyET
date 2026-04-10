using System;
using System.Linq;

namespace ET.Client
{
    public static class ClientLobbyNetComponentHelper
    {
        public static void Send(IMessage message)
        {
            ClientLobbySenderComponent.Instance.Send(message);
        }
        
        // Lobby 大厅
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="checkRepeated">是否检查反复发送的协议,如果有,则丢弃</param>
        /// <returns></returns>
        public static ETTask<IResponse> Call(IRequest request, bool checkRepeated = true)
        {
            var responseTask = ETTask<IResponse>.Create();
            if (checkRepeated)
            {
                if (ClientLobbySenderComponent.Instance.SendMessageQueue.Any(x => x.RequestType == request.GetType()))
                {
                    var response = MessageHelper.CreateResponse(request.GetType(), 0, ErrorCore.ERR_NotFoundActor);
                    responseTask.SetResult(response);
                    return responseTask;
                }
            }

            ClientLobbyQueueMessage queueMessage = new ();
            queueMessage.RequestType = request.GetType();
            queueMessage.Request = request;
            queueMessage.Response = responseTask;
            
            ClientLobbySenderComponent.Instance.SendMessageQueue.Enqueue(queueMessage);
            return queueMessage.Response;
        }
    }
}