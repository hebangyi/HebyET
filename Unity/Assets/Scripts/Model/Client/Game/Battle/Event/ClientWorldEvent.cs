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
        
        protected abstract void Run(ClientWorld world, A a);
        
        public void Handle(ClientWorld world, A a)
        {
            try
            {
                Run(world, a);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}
