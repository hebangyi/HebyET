using System;
using ET.Server;

namespace ET
{
    public abstract class MessageClientHandler<E, Message>: HandlerObject, IMessageClientHandler where E : Entity where Message : MessageObject
    {
        protected abstract void Run(E entity, Message message);
        
        public void Handle(Entity entity, object message)
        {
            if (message == null)
            {
                Log.Error($"消息类型转换错误: {message.GetType().FullName} to {typeof (Message).Name}");
                return;
            }

            
            if (entity is not E e)
            {
                Log.Error($"Actor类型转换错误: {entity.GetType().FullName} to {typeof (E).Name} --{typeof (Message).FullName}");
                return;
            }
            
            // TODO 判断 entity 有没有 entity 的 Session
            /*
            if (session.IsDisposed)
            {
                Log.Error($"session disconnect {message}");
                return;
            }*/

            var request = message as Message;
            if (request == null)
            {
                throw new Exception($"消息类型转换错误: {message.GetType().FullName} to {typeof (Message).FullName}");
            }
            
            this.Run(e, request);
        }

        public Type GetRequestType()
        {
            return typeof (Message);
        }

        public Type GetResponseType()
        {
            return null;
        }
    }
    
    
    public abstract class MessageClientHandler<E, Request, Response>: HandlerObject, IMessageClientHandler where E : Entity  where Request : MessageObject, IRequest where Response : MessageObject, IResponse
    {
        protected abstract void Run(E e, Request request, Response response);

        public void Handle(Entity entity, object message)
        {
            try
            {
                if (message is not Request request)
                {
                    Log.Error($"消息类型转换错误: { message.GetType().FullName} to {typeof (Request).Name}");
                    return;
                }

                if (entity is not E ee)
                {
                    Log.Error($"entity 转换错误: {entity.GetType().FullName} to {typeof (E).FullName} --{typeof (Request).FullName}");
                    return;
                }

                int rpcId = request.RpcId;
                // 这里用using很安全，因为后面是session发送出去了
                using Response response = ObjectPool.Instance.Fetch<Response>();
                try
                {
                    this.Run(ee, request, response);
                }
                catch (RpcException exception)
                {
                    // 这里不能返回堆栈给客户端
                    Log.Error(exception.ToString());
                    response.Error = exception.Error;
                }
                catch (Exception exception)
                {
                    // 这里不能返回堆栈给客户端
                    Log.Error(exception.ToString());
                    response.Error = ErrorCore.ERR_RpcFail;
                }
                
                response.RpcId = rpcId; // 在这里设置rpcId是为了防止在Run中不小心修改rpcId字段
                var clientSessionComponent = ee.GetComponent<EntityClientSessionComponent>();

                if (clientSessionComponent != null)
                {
                    clientSessionComponent.Session?.Send(response);
                }
                
            }
            catch (Exception e)
            {
                throw new Exception($"解释消息失败: {message.GetType().FullName}", e);
            }
        }

        public Type GetRequestType()
        {
            return typeof (Request);
        }

        public Type GetResponseType()
        {
            return typeof (Response);
        }
    }
    
    
    
    
    
    
    
    
    
}
