using System;

namespace ET
{
    [EntitySystemOf(typeof(SessionAcceptCheckTimeoutComponent))]
    [FriendOf(typeof(SessionAcceptCheckTimeoutComponent))]
    public static partial class SessionAcceptCheckTimeoutComponentHelper
    {
        [Invoke(TimerInvokeType.SessionAcceptTimeout)]
        public class SessionAcceptTimeout: ATimer<SessionAcceptCheckTimeoutComponent>
        {
            protected override void Run(SessionAcceptCheckTimeoutComponent self)
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
        private static void Awake(this SessionAcceptCheckTimeoutComponent self)
        {
            self.Timer = self.Root().GetComponent<TimerComponent>().NewOnceTimer(TimeInfo.Instance.ServerNowMillTime() + 5000, TimerInvokeType.SessionAcceptTimeout, self);
        }
        
        [EntitySystem]
        private static void Destroy(this SessionAcceptCheckTimeoutComponent self)
        {
            self.Root().GetComponent<TimerComponent>()?.Remove(ref self.Timer);
        }
        
    }
    
    
}