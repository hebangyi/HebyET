namespace ET.Server;

public class GlobalClockData: MongoEntity
{
    public long NextUpdateTenSecondTime;
    public long NextUpdateMinuteTime;
    public long NextUpdateHourTime;
    
    public long NextUpdateDayTime;
    public long NextUpdateWeekTime; 
    public long NextUpdateMonthTime;
}