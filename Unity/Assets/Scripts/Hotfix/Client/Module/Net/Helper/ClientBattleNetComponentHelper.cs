using System.Linq;

namespace ET.Client
{
    public static class ClientBattleNetComponentHelper
    {
        public static void Send(IMessage message)
        {
            ClientBattleSenderComponent.Instance.Send(message);
        }

        // Battle 战斗
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
                if (ClientBattleSenderComponent.Instance.Type2ClientMessage.ContainsKey(request.GetType()))
                {
                    var response = MessageHelper.CreateResponse(request.GetType(), 0, ErrorCore.ERR_RPCReapted);
                    responseTask.SetResult(response);
                    return responseTask;
                }
            }

            ClientBattleQueueMessage queueMessage = new();
            queueMessage.RequestType = request.GetType();
            queueMessage.Request = request;
            queueMessage.Response = responseTask;

            ClientBattleSenderComponent.Instance.Type2ClientMessage[request.GetType()] = queueMessage;
            return queueMessage.Response;
        }
    }
}