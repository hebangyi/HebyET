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

        public const int GameOperaAngle = 45;  // 相机与地面角度
        public const int FixedUpdateDeltaTime = 20; // 固定更新 20ms FixedUpdate 每秒50次更新
        public const int CameraRotationSpeed = 300; // 300ms 旋转90度
        
        public const int AOICellSize = 50;  // AOI 网格
        public const int AOIWatchCellRadius = 1; // AOI 监听的Cell半径数
        
        
        public const int LogicInterval = 50;
        
        public const float Rad2Deg = 57.29578f;
    }
}