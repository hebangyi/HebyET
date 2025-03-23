namespace ET
{
    [UniqueId(100, 10000)]
    public static class TimerInvokeType
    {
        // 框架层100-200，逻辑层的timer type从200起
        public const int WaitTimer = 100;
        public const int SessionIdleChecker = 101;
        public const int MessageLocationSenderChecker = 102;
        public const int MessageSenderChecker = 103;
        
        // 框架层100-200，逻辑层的timer type 200-300
        public const int MoveTimer = 201;
        public const int AITimer = 202;
        public const int SessionAcceptTimeout = 203;
        
        
        public const int RoomUpdate = 301;

        // 应用层
        public const int MongoCacheCheckerTimer = 1001;
        public const int GlobalClockTimer = 1002;



        public const int OneSecondTimer = 2001; // 秒级定时器
        public const int TenSecondTimer = 2002; // 十秒级定时器
        public const int OneMinuteTimer = 2003; // 分钟级定时器
        public const int OneHourTimer = 2004;   // 小时级定时器

    }
}