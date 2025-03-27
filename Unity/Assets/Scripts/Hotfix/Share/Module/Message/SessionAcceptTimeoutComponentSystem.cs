using System;

namespace ET
{
    [EntitySystemOf(typeof(SessionAcceptLoginCheckTimeoutComponent))]
    [FriendOf(typeof(SessionAcceptLoginCheckTimeoutComponent))]
    public static partial class SessionAcceptCheckTimeoutComponentHelper
    {
        [Invoke(TimerInvokeType.SessionAcceptTimeout)]
        public class SessionAcceptTimeout: ATimer<SessionAcceptLoginCheckTimeoutComponent>
        {
            protected override void Run(SessionAcceptLoginCheckTimeoutComponent self)
            {
                try
                {
                    self.Parent.Dispose();
                }
                catch (Exception e)
                {
                    Log.Error($"move timer error: {self.Id}\n{e}");
                }
            }
        }
        
        [EntitySystem]
        private static void Awake(this SessionAcceptLoginCheckTimeoutComponent self)
        {
            self.Timer = self.Root().GetComponent<TimerComponent>().NewOnceTimer(TimeInfo.Instance.ServerNowMillTime() + 5000, TimerInvokeType.SessionAcceptTimeout, self);
        }
        
        [EntitySystem]
        private static void Destroy(this SessionAcceptLoginCheckTimeoutComponent self)
        {
            self.Root().GetComponent<TimerComponent>()?.Remove(ref self.Timer);
        }
        
    }
    
    
}