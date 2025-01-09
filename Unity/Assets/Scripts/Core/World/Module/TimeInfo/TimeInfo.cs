using System;

namespace ET
{
    public class TimeInfo: Singleton<TimeInfo>, ISingletonAwake
    {
        private int timeZone;
        
        public int TimeZone
        {
            get
            {
                return this.timeZone;
            }
            set
            {
                this.timeZone = value;
                dt = dt1970.AddHours(TimeZone);
            }
        }
        
        private DateTime dt1970;
        private DateTime dt;
        
        // ping消息会设置该值，原子操作
        public long ServerMinusClientTime { private get; set; }

        public long FrameTime { get; private set; }
        
        public void Awake()
        {
            this.dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            this.dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            this.FrameTime = this.ClientNowMillTime();
        }

        public void Update()
        {
            // 赋值long型是原子操作，线程安全
            this.FrameTime = this.ClientNowMillTime();
        }
        
        /// <summary> 
        /// 根据时间戳获取时间 
        /// </summary>  
        public DateTime ToDateTime(long timeStamp)
        {
            return dt.AddTicks(timeStamp * 10000);
        }
        
        // 线程安全
        public long ClientNowMillTime()
        {
            return (DateTime.UtcNow.Ticks - this.dt1970.Ticks) / 10000;
        }

        public long ClientNowSec()
        {
            return this.ClientNowMillTime() / 1000; 
        }
        
        public long ServerNowMillTime()
        {
            return this.ClientNowMillTime() + this.ServerMinusClientTime;
        }
        
        public long ServerNowSec()
        {
            return this.ServerNowMillTime() / 1000;
        }
        
        public long ClientFrameTime()
        {
            return this.FrameTime;
        }
        
        public long ServerFrameTime()
        {
            return this.FrameTime + this.ServerMinusClientTime;
        }
        
        public long Transition(DateTime d)
        {
            return (d.Ticks - dt.Ticks) / 10000;
        }
    }
}