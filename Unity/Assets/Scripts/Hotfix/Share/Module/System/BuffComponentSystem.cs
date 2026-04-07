using System;
using System.Collections.Generic;
using System.Linq;

namespace ET
{
    [FriendOf(typeof(BuffComponent))]
    [EntitySystemOf(typeof(BuffComponent))]
    public static partial class BuffComponentSystem
    {
        [EntitySystem]
        private static void Awake(this BuffComponent self)
        {
        }

        public static void AddBuff(this BuffComponent self, long buffId)
        {
            var unitEntity = self.GetParent<UnitEntity>();
            var unitEntityBuffData = unitEntity.GetUnitEntityElemData<UnitEntityBuffData>();
            if (unitEntityBuffData == null)
            {
                Log.Error($"UnitEntity {unitEntity.InsId} 添加buff失败 {buffId} 找不到技能数据");
                return;
            }

            var buffDataItem = unitEntityBuffData.BuffId2BuffDataItems.GetValueOrDefault(buffId);

            if (buffDataItem != null)
            {
                // TODO 可能做堆叠
            }
            else
            {
                UnitEntityBuffDataItem dataItem = new ();
                dataItem.BuffId = buffId;
                unitEntityBuffData.BuffId2BuffDataItems.Add(buffId, dataItem);
                unitEntityBuffData.Dirty();
            }
        }

        public static void RemoveBuff(this BuffComponent self, long buffId)
        {
            var unitEntity = self.GetParent<UnitEntity>();
            var unitEntityBuffData = unitEntity.GetUnitEntityElemData<UnitEntityBuffData>();
            if (unitEntityBuffData == null)
            {
                Log.Error($"UnitEntity {unitEntity.InsId} 移除buff失败 {buffId} 找不到技能数据");
                return;
            }

            unitEntityBuffData.BuffId2BuffDataItems.Remove(buffId);
            unitEntityBuffData.Dirty();
        }
    }
}