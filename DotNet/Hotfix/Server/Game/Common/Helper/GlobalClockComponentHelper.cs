using System;

namespace ET.Server;

[Event(SceneType.All)]
public class GlobalClockComponent_InitGlobalComponentEvent : AEvent<Scene, InitGlobalComponentEvent>
{
    protected override async ETTask Run(Scene scene, InitGlobalComponentEvent a)
    {
        await GlobalClockComponentHelper.InitData(scene);
    }
}


[Event(SceneType.All)]
public class GlobalClockComponent_InitGlobalComponentFinishEvent : AEvent<Scene, InitGlobalComponentFinishEvent>
{
    protected override async ETTask Run(Scene scene, InitGlobalComponentFinishEvent a)
    {
        var globalClockComponent = scene.GetComponent<GlobalClockComponent>();
        if (globalClockComponent == null)
        {
            return;
        }
        
        scene.Root().GetComponent<TimerComponent>()
                .NewRepeatedTimer(1000, TimerInvokeType.GlobalClockTimer, globalClockComponent);
        await ETTask.CompletedTask;
    }
}


[Event(SceneType.All)]
public class GlobalTimeOneDay_InitGlobalComponentFinishEvent : AEvent<Scene, GlobalTimeCrossOneDay>
{
    protected override async ETTask Run(Scene scene, GlobalTimeCrossOneDay a)
    {
        GlobalClockComponentHelper.Save(scene);
        await ETTask.CompletedTask;
    }
}



[Invoke(TimerInvokeType.GlobalClockTimer)]
public class GlobalClockComponentTimer : ATimer<GlobalClockComponent>
{
    protected override void Run(GlobalClockComponent self)
    {
        try
        {
            self.CheckTimer();
        }
        catch (Exception e)
        {
            Log.Error($"move timer error: {self.Id}\n{e}");
        }
    } 
}


public static class GlobalClockComponentHelper
{
    
    public static void Reset(this GlobalClockComponent self)
    {
        var globalClockData = self.GlobalClockData;
        globalClockData.NextUpdateTenSecondTime = 0;
        globalClockData.NextUpdateMinuteTime = 0;
        globalClockData.NextUpdateHourTime = 0;
        
        globalClockData.NextUpdateDayTime = 0;
        globalClockData.NextUpdateWeekTime = 0;
        globalClockData.NextUpdateMonthTime = 0;
    }
    
    public static void CheckTimer(this GlobalClockComponent self)
    {
        var globalClockData = self.GlobalClockData;
        
        long time = TimeInfo.Instance.NowSec();
        EventSystem.Instance.Publish(self.Root(), new GlobalTimeOneSecond());
        if (time >= globalClockData.NextUpdateTenSecondTime)
        {
            EventSystem.Instance.Publish(self.Root(), new GlobalTimeTenSecond());
            globalClockData.NextUpdateTenSecondTime += ((time - globalClockData.NextUpdateTenSecondTime) / 10 + 1) * 10;
        }

        if (time >= globalClockData.NextUpdateMinuteTime)
        {
            EventSystem.Instance.Publish(self.Root(), new GlobalTimeOneMinute());
            globalClockData.NextUpdateMinuteTime += ((time - globalClockData.NextUpdateMinuteTime) / GameConstant.OneMinuteSecond + 1) * GameConstant.OneMinuteSecond;
        }

        if (time >= globalClockData.NextUpdateHourTime)
        {
            EventSystem.Instance.Publish(self.Root(), new GlobalTimeOneHour());
            globalClockData.NextUpdateHourTime += ((time - globalClockData.NextUpdateHourTime) / GameConstant.OneHourSecond + 1) * GameConstant.OneHourSecond;
        }

        // Cross 类的 进行初始化
        if (time >= globalClockData.NextUpdateDayTime)
        {
            EventSystem.Instance.Publish(self.Root(), new GlobalTimeCrossOneDay());
            globalClockData.NextUpdateDayTime = TimeHelper.FutureTimestampSeconds(1, GameConstant.DayStartHour);
        }

        if (time >= globalClockData.NextUpdateWeekTime)
        {
            EventSystem.Instance.Publish(self.Root(), new GlobalTimeCrossOneWeek());
            globalClockData.NextUpdateWeekTime = TimeHelper.NextWeekMondayHourTimestampSeconds(GameConstant.DayStartHour);
        }
        
        if (time >= globalClockData.NextUpdateMonthTime)
        {
            EventSystem.Instance.Publish(self.Root(), new GlobalTimeCrossOneMonth());
            globalClockData.NextUpdateMonthTime = TimeHelper.NextMonthFirstDayTimestampSeconds(GameConstant.DayStartHour);
        }
    }
    
    public static async ETTask InitData(Scene scene)
    {
        var globalClockComponent = scene.GetComponent<GlobalClockComponent>();
        
        if (globalClockComponent == null)
        {
            return;
        }

        var mongoDBComponent = scene.GetComponent<MongoDBComponent>();
        var globalClockData = await mongoDBComponent.QueryOne<GlobalClockData>(x => x.Id == scene.Id);
        if (globalClockData == null)
        {
            globalClockData = new GlobalClockData();
            globalClockData.Id = scene.Id;
        }

        globalClockComponent.GlobalClockData = globalClockData;
        Save(scene);
        await ETTask.CompletedTask;
    }

    public static void Save(Scene scene)
    {
        var mongoDBComponent = scene.GetComponent<MongoDBComponent>();
        var globalClockComponent = scene.GetComponent<GlobalClockComponent>();
        if (mongoDBComponent == null || globalClockComponent == null)
        {
            return;
        }
        
        mongoDBComponent.Save(globalClockComponent.GlobalClockData).Coroutine();
    }
}