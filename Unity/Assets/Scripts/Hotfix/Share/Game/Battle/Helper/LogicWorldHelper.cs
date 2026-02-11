using System;
using System.Collections.Generic;
using System.Linq;

namespace ET
{
    public static class LogicWorldHelper
    {
        public static UnitEntity GetUnitEntityByInsId(this LogicWorld logicWorld, long insId)
        {
            return logicWorld.AllEntities.GetValueOrDefault(insId);
        }
        
        /// <summary>
        /// 查询拥有指定组件组合的所有实体
        /// </summary>
        public static IEnumerable<UnitEntity> GetEntitiesWithComponents(this LogicWorld logicWorld, params Type[] componentTypes)
        {
            if (componentTypes.Length == 0) yield break;

            // 先获取第一个组件类型的所有实体ID
            var firstComponent = logicWorld.Components.GetValueOrDefault(componentTypes[0]);
            if (firstComponent == null) yield break;

            foreach (var entityId in firstComponent)
            {
                // 检查该实体是否拥有所有指定组件
                bool hasAllComponents = true;
                foreach (var type in componentTypes.Skip(1))
                {
                    if (!logicWorld.Components.ContainsKey(type) || !logicWorld.Components[type].Contains(entityId))
                    {
                        
                        = false;
                        break;
                    }
                }

                if (hasAllComponents)
                {
                    yield return logicWorld.AllEntities.GetValueOrDefault(entityId);
                }
            }
        }
        
        
        public static void Tick(this LogicWorld self)
        {
            self.Frame++;
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