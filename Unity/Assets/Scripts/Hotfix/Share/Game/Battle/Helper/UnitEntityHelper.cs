using System;
using System.Collections.Generic;

namespace ET
{
    public static class UnitEntityHelper
    {
        // 是否有 ElementData 数据
        public static bool HasUnitEntityElementData<T>(this UnitEntity self) where T : IUnitEntityElemData
        {
            Type type = typeof(T);
            var componentId = OpcodeType.Instance.GetOpcode(type);
            return self.UnitEntityData.ContainsKey(componentId);
        }

        public static T GetUnitEntityElemData<T>(this UnitEntity self) where T : class, IUnitEntityElemData
        {
            Type type = typeof(T);
            var componentId = OpcodeType.Instance.GetOpcode(type);
            T elemData = self.UnitEntityData.GetValueOrDefault(componentId) as T;
            return elemData;
        }
    
        public static BattleUnitEntity ToBattleUnitEntity(this UnitEntity unitEntity)
        {
            BattleUnitEntity battleUnitEntity = BattleUnitEntity.Create();
            battleUnitEntity.InsId = unitEntity.InsId;

            foreach (var unitEntityElemDataKv in unitEntity.UnitEntityData)
            {
                var unitEntityElemData = UnitEntityElemData.Create();
                unitEntityElemData.CompId = unitEntityElemDataKv.Key;
                unitEntityElemData.ElemDatas = MemoryPackHelper.Serialize(unitEntityElemDataKv.Value);
                battleUnitEntity.EleDatas.Add(unitEntityElemData);
            }
            return battleUnitEntity;
        }

        public static BattleUnitEntity ToBattleUnitEntity(this SyncDirtyUnitEntity syncDirtyUnitEntity)
        {
            BattleUnitEntity battleUnitEntity = BattleUnitEntity.Create();
            battleUnitEntity.InsId = syncDirtyUnitEntity.InsId;
            
            foreach (var unitEntityElemDataKv in syncDirtyUnitEntity.DirtyElemDatas)
            {
                var unitEntityElemData = UnitEntityElemData.Create();
                unitEntityElemData.CompId = unitEntityElemDataKv.Key;
                unitEntityElemData.ElemDatas = MemoryPackHelper.Serialize(unitEntityElemDataKv.Value);
                battleUnitEntity.EleDatas.Add(unitEntityElemData);
            }
            
            return battleUnitEntity;
        }
        
    }
}
