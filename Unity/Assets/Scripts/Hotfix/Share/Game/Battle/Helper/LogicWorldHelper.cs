using System.Collections.Generic;

namespace ET
{
    public static class LogicWorldHelper
    {
        public static UnitEntity GetUnitEntityByInsId(this LogicWorld logicWorld, long insId)
        {
            return logicWorld.AllEntities.GetValueOrDefault(insId);
        }
        
        public static void Tick(this LogicWorld self)
        {
            self.Frame++;
            self.NowMilliTime = TimeInfo.Instance.NowMillTime();

            behaviac.Workspace.Instance.DoubleValueSinceStartup = self.NowMilliTime;
            
            // Log.Info($"World Id : {self.Id} Tick Frame: {self.Frame}");
            foreach (var comId2LogicsKv in LogicWorldLogicManagerComponent.Instance.Type2TickLogics)
            {
                var logicHandler = comId2LogicsKv.Value;
                logicHandler.OnTick(self);
            }
            // AI 更新
            self.GetComponent<AIComponent>().UpdateAITick();
            
            // 同步AOI数据
            self.GetComponent<AOIManagerComponent>()?.SyncHandler?.Sync();
        }
    }
}