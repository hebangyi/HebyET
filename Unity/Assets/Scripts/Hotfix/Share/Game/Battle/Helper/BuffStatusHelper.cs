using System;
using System.Collections.Generic;
using System.Linq;

namespace ET
{
    public static class BuffStatusHelper
    {
        public static void AddBuffStatus(UnitEntity unitEntity, BuffStatus status, uint endFrame = UInt32.MaxValue)
        {
            var unitEntityBuffData = unitEntity.GetUnitEntityElemData<UnitEntityBuffData>();
            uint currentFrame = unitEntity.LogicWorld().Frame;
            
            var dataItem = unitEntityBuffData.BuffStatus2DataItems.GetValueOrDefault(status);
            // 处理旧的 dataItem
            if (dataItem != null)
            {
                dataItem.StackItems.RemoveAll(x => currentFrame >= x.EndFrame);
            }
            else
            {
                dataItem = UnitEntityBuffDataItem.Create();
                dataItem.BuffStatus = status;
                unitEntityBuffData.BuffStatus2DataItems[status] = dataItem;
            }

            var addConfig = BuffStatusTypeConfigCategory.Instance.BuffStatus2Config.GetValueOrDefault(status);
            if (addConfig == null)
            {
                Log.Error($"添加BuffStatus 叠加状态异常 没有找到 BuffStatusTypeConfig 配置 [{status}]");
                return;
            }
                
            // 层数叠加
            if (addConfig.IsBuffStackCount)
            {
                if (dataItem.StackItems.Count < addConfig.BuffStatusStatckMaxCount)
                {
                    UnitEntityBuffStackItem stackItem = UnitEntityBuffStackItem.Create();
                    stackItem.StartFrame = currentFrame;
                    stackItem.EndFrame = endFrame;
                    dataItem.StackItems.Add(stackItem);
                }
            }
            else
            {
                var firstStackItem = dataItem.StackItems.FirstOrDefault();
                if (firstStackItem == null)
                {
                    UnitEntityBuffStackItem stackItem = UnitEntityBuffStackItem.Create();
                    stackItem.StartFrame = currentFrame;
                    stackItem.EndFrame = endFrame;
                    dataItem.StackItems.Add(stackItem);
                }
                else
                {
                    if (endFrame == UInt32.MaxValue)
                    {
                        firstStackItem.EndFrame = endFrame;
                    }
                    else
                    {
                        // 时间叠加
                        switch (addConfig.BuffStatusTimeAddType)
                        {
                            case 0:
                            {
                                // 0=最大叠加时间
                                firstStackItem.EndFrame = Math.Max(firstStackItem.EndFrame, endFrame);
                                break;
                            }
                            case 1:
                            {
                                // 1=最小叠加时间
                                firstStackItem.EndFrame = Math.Min(firstStackItem.EndFrame, endFrame);
                                break;
                            }
                            case 2:
                            {
                                // 2=叠加时间之后
                                firstStackItem.EndFrame = currentFrame + (endFrame - currentFrame) + (firstStackItem.EndFrame - currentFrame);
                                break;
                            }
                        }
                    }
                }
            }
        }
        
        public static bool IsRigidity(UnitEntity unitEntity)
        {
            var unitEntityBuffData = unitEntity.GetUnitEntityElemData<UnitEntityBuffData>();
            if (unitEntityBuffData == null)
            {
                return false;
            }

            var dataItem = unitEntityBuffData.BuffStatus2DataItems.GetValueOrDefault(BuffStatus.Rigidity);

            if (dataItem == null)
            {
                return false;
            }

            foreach (var stackItem in dataItem.StackItems)
            {
                if (unitEntity.LogicWorld().Frame < stackItem.EndFrame)
                {
                    return true;
                }    
            }

            return false;
        }
    }
}

