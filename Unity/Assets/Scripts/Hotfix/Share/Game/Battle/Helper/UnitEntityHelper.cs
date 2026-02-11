using System;
using System.Collections.Generic;

namespace ET
{
    public static class UnitEntityHelper
    {
        public static T CreateUnitEntityElemData<T>(this UnitEntity self) where T : IUnitEntityElemData
        {
            var world = self.GetParent<LogicWorld>();
        
            Type type = typeof(T);
            var componentId = OpcodeType.Instance.GetOpcode(type);
            if (self.UnitEntityData.ContainsKey(componentId))
            {
                Log.Error("this world component id is already exist, can not create entity element data");
                return default;
            }

            var methodInfo = type.GetMethod("Create");
            var obj = methodInfo.Invoke(null, new object[]{self.InsId, world.GetComponent<AOIManagerComponent>().DirtyHandler ,true});
            T instance = (T)obj;
            self.UnitEntityData[componentId] = instance;
            return instance;
        }

        public static T CreateUnitEntityLogicElemData<T>(this UnitEntity self) where T : class, IUnitEntityLogicElemData
        {
            Type type = typeof(T);
            if (self.UnitEntityLogicData.ContainsKey(type))
            {
                Log.Error("this world component id is already exist, can not create entity element data");
                return default;
            }

            var logicElemData = Activator.CreateInstance(type) as IUnitEntityLogicElemData;
            if (logicElemData == null)
            {
                return null;
            }
            
            self.UnitEntityLogicData[type] = logicElemData;
            return (T)logicElemData;
        }
        
        
        public static T GetUnitEntityLogicElemData<T>(this UnitEntity self) where T : class, IUnitEntityLogicElemData
        {
            Type type = typeof(T);
            T elemData = self.UnitEntityLogicData.GetValueOrDefault(type) as T;
            return elemData;
        }


        public static void Dirty(this UnitEntity self, IUnitEntityElemData elemData)
        {
            var logicWorld = self.LogicWorld();
            if (logicWorld == null)
            {
                return;
            }
            
            logicWorld.GetComponent<AOIManagerComponent>()?.DirtyHandler?.Dirty(self.InsId, elemData);
        }
        
        
        public static LogicWorld LogicWorld(this UnitEntity unitEntity)
        {
            return unitEntity.GetParent<LogicWorld>();
        }
        
        
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
