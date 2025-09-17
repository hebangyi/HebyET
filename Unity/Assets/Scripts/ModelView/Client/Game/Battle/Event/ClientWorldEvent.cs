using System;

namespace ET.Client
{
    public class ClientWorldEventHandlerAttribute : BaseAttribute
    {
    } 
    
    // 客户端事件
    public interface IClientWorldEvent
    {
        Type Type { get; }
    }
    
    
    public abstract class AClientWorldEvent<A> : IClientWorldEvent where A : struct
    {
        public Type Type 
        {
            get
            {
                return typeof (A);
            }
        }
        
        protected abstract ETTask Run(ClientWorld world, A a);
        
        public async ETTask Handle(ClientWorld world, A a)
        {
            try
            {
                await Run(world, a);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
        
    }
}
