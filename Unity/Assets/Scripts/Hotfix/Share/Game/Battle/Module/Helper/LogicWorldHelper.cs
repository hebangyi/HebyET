namespace ET
{
    public static class LogicWorldHelper
    {
        public static void Tick(this LogicWorld self)
        {
            self.Frame++;
            self.NowMilliSeconds = TimeInfo.Instance.NowMillTime();
            // Log.Info($"World Id : {self.Id} Tick Frame: {self.Frame}");
            foreach (var comId2LogicsKv in LogicWorldLogicManagerComponent.Instance.Type2TickLogics)
            {
                var logicHandler = comId2LogicsKv.Value;
                logicHandler.OnTick(self);
            }
            // AI 更新
            // self.GetComponent<AIComponent>().UpdateAITick();
            
            // 同步AOI数据
            self.GetComponent<AOIManagerComponent>()?.SyncHandler?.Sync();
        }
    }
}