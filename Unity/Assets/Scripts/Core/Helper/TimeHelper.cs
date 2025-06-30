using System;
using MongoDB.Driver.Linq;

namespace ET
{
    public static class TimeHelper
    {
        /// <summary>
        /// 两个秒级时间戳是否处在当地时间以指定整点作为分界点跨天
        /// </summary>
        /// <param name="timeSec1">第一个秒级时间戳</param>
        /// <param name="timeSec2">第二个秒级时间戳</param>
        /// <param name="hour">指定的整点作为跨天分界点，默认是当地时间凌晨5点</param>
        /// <returns>是否处在同一天</returns>
        public static bool IsCrossDay(long timeSec1, long timeSec2, int hour = GameConstant.DayStartHour)
        {
            hour %= GameConstant.OneDayHour;
            DateTimeOffset dateTime1 =
                    DateTimeOffset.FromUnixTimeSeconds(timeSec1).ToLocalTime().AddHours(-hour);
            DateTimeOffset dateTime2 =
                    DateTimeOffset.FromUnixTimeSeconds(timeSec2).ToLocalTime().AddHours(-hour);

            return !(dateTime1.Year == dateTime2.Year &&
                dateTime1.Month == dateTime2.Month &&
                dateTime1.Day == dateTime2.Day);
        }

        /// <summary>
        /// 两个秒级时间戳是否处在当地时间以指定整点作为跨天分界点跨周
        /// </summary>
        /// <param name="timeSec1">第一个秒级时间戳</param>
        /// <param name="timeSec2">第二个秒级时间戳</param>
        /// <param name="hour">指定的整点作为跨天分界点，默认是当地时间凌晨5点</param>
        /// <returns>是否处在同一周</returns>
        public static bool IsCrossWeek(long timeSec1, long timeSec2, int hour = GameConstant.DayStartHour)
        {
            hour %= GameConstant.OneDayHour;

            DateTime dt1 = DateTimeOffset.FromUnixTimeSeconds(timeSec1).ToLocalTime().AddHours(-hour).DateTime;
            DateTime dt2 = DateTimeOffset.FromUnixTimeSeconds(timeSec2).ToLocalTime().AddHours(-hour).DateTime;
            
            // 获得周一的时间
            DateTime weekStart1 = dt1.AddDays(-realDayOfWeek(dt1) + 1);
            DateTime weekStart2 = dt2.AddDays(-realDayOfWeek(dt2) + 1);
            
            // 获取同一周的周一
            return weekStart1.Date != weekStart2.Date;
        }

        /// <summary>
        /// 两个时间戳是否处在当地时间以指定整点作为跨天分界点跨月
        /// </summary>
        /// <param name="timeSec1">第一个秒级时间戳</param>
        /// <param name="timeSec2">第二个秒级时间戳</param>
        /// <param name="hour">指定的整点作为跨天分界点，默认是当地时间凌晨5点</param>
        /// <returns>是否处在同一月</returns>
        public static bool IsCrossMonth(long timeSec1, long timeSec2, int hour = GameConstant.DayStartHour)
        {
            hour %= GameConstant.OneDayHour;

            DateTimeOffset dateTime1 =
                    DateTimeOffset.FromUnixTimeSeconds(timeSec1).ToLocalTime().AddHours(-hour);
            DateTimeOffset dateTime2 =
                    DateTimeOffset.FromUnixTimeSeconds(timeSec2).ToLocalTime().AddHours(-hour);

            return !(dateTime1.Year == dateTime2.Year && dateTime1.Month == dateTime2.Month);
        }
        
        /// <summary>
        /// 今天指定整点的秒级时间戳
        /// </summary>
        /// <param name="targetTime">整点数</param>
        /// <param name="hour">整点数</param>
        /// <returns>时间戳</returns>
        /// <exception cref="Exception">参数错误异常</exception>
        public static long HourTimestampSeconds(Int64 targetTime, int hour)
        {
            hour %= GameConstant.OneDayHour;
            DateTimeOffset targetDateTimeOffset =
                    DateTimeOffset.FromUnixTimeSeconds(targetTime).ToLocalTime().AddHours(-hour);
            DateTime targetDateTime = new (targetDateTimeOffset.Year, targetDateTimeOffset.Month, targetDateTimeOffset.Day, hour, 0, 0, 0,
                DateTimeKind.Local);

            return new DateTimeOffset(targetDateTime).ToUnixTimeSeconds();
        }
        
        
        /// <summary>
        /// 今天指定整点的秒级时间戳
        /// </summary>
        /// <param name="hour">整点数</param>
        /// <returns>时间戳</returns>
        /// <exception cref="Exception">参数错误异常</exception>
        public static long TodayHourTimestampSeconds(int hour) => HourTimestampSeconds(TimeInfo.Instance.NowSec(), hour);

        
        /// <summary>
        /// 未来某天的时间戳
        /// </summary>
        /// <param name="dayNum"></param>
        /// <param name="hour"></param>
        /// <returns></returns>
        public static long FutureTimestampSeconds(int dayNum, int hour) => HourTimestampSeconds(TimeInfo.Instance.NowSec(), hour) + dayNum * GameConstant.OneDaySecond;


        public static long NextWeekMondayHourTimestampSeconds(int hour) =>
                MondayHourTimestampSeconds(TimeInfo.Instance.NowSec(), hour) + GameConstant.OneWeekSecond;
        
        /// <summary>
        /// 指定时间戳点该周的周一的时间戳
        /// </summary>
        /// <param name="targetTime"></param>
        /// <param name="hour"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static long MondayHourTimestampSeconds(long targetTime, int hour)
        {
            if (hour < 0 || hour > 23)
            {
                throw new Exception("error: input info hour must be between 0 and 23.");
            }
            DateTime nowOffsetDateTime = DateTimeOffset.FromUnixTimeSeconds(targetTime).ToLocalTime().DateTime;
            DateTime targetDateTime =
                    new DateTime(nowOffsetDateTime.Year, nowOffsetDateTime.Month, nowOffsetDateTime.Day, hour, 0, 0, 0, DateTimeKind.Local).AddDays(
                        -realDayOfWeek(nowOffsetDateTime) + 1);
            return new DateTimeOffset(targetDateTime).ToUnixTimeSeconds();
        }
        
        /// <summary>
        /// 真实的周几（中国的每周第一天是周一，外国的每周第一天是周日，所以需要该接口进行转换）
        /// </summary>
        /// <param name="dateTime">日期时间</param>
        /// <returns>周几</returns>
        private static int realDayOfWeek(DateTime dateTime)
        {
            return dateTime.DayOfWeek == DayOfWeek.Sunday? 7 : (int)dateTime.DayOfWeek;
        }

        
        
        /// <summary>
        /// 今天固定时间的时间戳
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="min"></param>
        /// <param name="sec"></param>
        /// <returns></returns>
        public static long TodayHourTimestampSeconds(int hour, int min, int sec)
        {
            hour %= GameConstant.OneDayHour;
            min %= GameConstant.OneHourMinute;
            sec %= GameConstant.OneMinuteSecond;
            
            DateTime nowOffsetDateTime = DateTime.Now;;
            DateTime targetDateTime = new (nowOffsetDateTime.Year, nowOffsetDateTime.Month, nowOffsetDateTime.Day, hour, min, sec, 0, DateTimeKind.Local);
            
            return new DateTimeOffset(targetDateTime).ToUnixTimeSeconds();
        }

        
        
        
        /// <summary>
        /// 下一个月第一天的时间戳
        /// </summary>
        /// <returns></returns>
        public static long NextMonthFirstDayTimestampSeconds(int hour)
        {
            // 获取当前时间
            DateTime now = DateTime.Now;
            // 获取下一个月的第一天 0 点
            DateTime nextMonth = new DateTime(now.Year, now.Month, 1).AddMonths(1);
            // 将时间转换为 Unix 时间戳
            DateTimeOffset dto = new DateTimeOffset(nextMonth);
            return dto.ToUnixTimeSeconds();
        }
    }
}