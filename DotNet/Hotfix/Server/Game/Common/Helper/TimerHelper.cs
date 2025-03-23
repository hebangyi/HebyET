using System;

namespace ET.Server;

[Invoke(TimerInvokeType.OneSecondTimer)]
public class OneSecondTimer : ATimer<GlobalClockComponent>
{
    protected override void Run(GlobalClockComponent self)
    {
        try
        {
            Log.Info("秒级定时任务执行");
        }
        catch (Exception e)
        {
            Log.Error($"move timer error: {self.Id}\n{e}");
        }
    }
}
