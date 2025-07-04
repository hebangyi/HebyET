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

        public static T GetUnitEntityElemData<T>(this UnitEntity self) where T : class
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
                battleUnitEntity.UnitEntityElemDatas[unitEntityElemDataKv.Key] = MemoryPackHelper.Serialize(unitEntityElemDataKv.Value);
            }
            return battleUnitEntity;
        }
    }
}
