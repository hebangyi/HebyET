namespace ET
{
    public class GameConstant
    {
        
        public const string EntryServerHttpHost = "127.0.0.1";
        public const int EntryServerHttpPort = 18081;
        public const string EntryServerServerListUri = "/get_entry";
        
        public const int HttpSessionTimeoutTime = 30 * 1000;

        public const int OneDayHour = 24;
        public const int OneHourMinute = 60;
        
        
        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        public const int DayStartHour = 0; // 程序起始的时间点
        public const int OneMinuteSecond = 60;
        public const int OneHourSecond = 60 * 60;
        public const int OneDaySecond = 24 * 60 * 60;
        public const int OneWeekSecond = 7 * 24 * 60 * 60;
        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

        public const int GameOperaAngle = 45;  // 操作角度
        public const int FixedUpdateDeltaTime = 20;
        
        
        public const float Rad2Deg = 57.29578f;
    }
}