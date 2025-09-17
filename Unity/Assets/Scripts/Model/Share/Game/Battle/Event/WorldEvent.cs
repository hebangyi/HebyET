using System;

namespace ET
{
    public class BattleEventAttribute : BaseAttribute
    {
        public BattleEventAttribute()
        {
        }
    } 

    public interface IBattleEvent
    {
        Type Type { get; }
    }

    public abstract class ABattleEvent<A> : IBattleEvent where A : struct
    {
        public Type Type 
        {
            get
            {
                return typeof (A);
            }
        }
        
        protected abstract void Run(LogicWorld logicWorld, A a);
        
        public void Handle(LogicWorld logicWorld, A a)
        {
            try
            {
                Run(logicWorld, a);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}
