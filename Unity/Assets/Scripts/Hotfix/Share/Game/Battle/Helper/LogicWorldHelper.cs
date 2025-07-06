using System.Collections.Generic;

namespace ET
{
    public  static partial class LogicWorldHelper
    {
        public static UnitEntity 
                CreateEntity(this World self, UnitEntity unitEntity)
        {
            unitEntity.InsId = unitEntity.InstanceId;
            self.AllEntity[unitEntity.InsId] = unitEntity;
            self.PublishEvent(new CreateUnitEntityEvent0(){UnitEntity = unitEntity});
            self.PublishEvent(new CreateUnitEntityEvent1(){UnitEntity = unitEntity});
            self.PublishEvent(new CreateUnitEntityEvent2(){UnitEntity = unitEntity});
            return unitEntity;
        }
        
        public static UnitEntity CreateEntity(this World self, BattleUnitEntity battleUnitEntity)
        {
            var unitEntity = self.AddChild<UnitEntity>();
            unitEntity.InsId = battleUnitEntity.InsId;
            self.AllEntity[unitEntity.InsId] = unitEntity;
            foreach (var unitEntityElemDataKv in battleUnitEntity.UnitEntityElemDatas)
            {
                var componentId = unitEntityElemDataKv.Key;
                var unitElemType = OpcodeType.Instance.GetType(componentId);
                if (unitElemType == null)
                {
                    Log.Error($"没有找到UnitEntity 组件 {componentId} 对应的数据类型");
                    continue;
                }
                
                var unitEntityElemData = MemoryPackHelper.Deserialize(unitElemType, unitEntityElemDataKv.Value, 0, unitEntityElemDataKv.Value.Length) as IUnitEntityElemData;
                unitEntity.UnitEntityData[unitEntityElemDataKv.Key] = unitEntityElemData;
            }
            
            self.PublishEvent(new CreateUnitEntityEvent0(){UnitEntity = unitEntity});
            self.PublishEvent(new CreateUnitEntityEvent1(){UnitEntity = unitEntity});
            self.PublishEvent(new CreateUnitEntityEvent2(){UnitEntity = unitEntity});
            return unitEntity;
        }
        

        public static void RemoveEntity(this World self, UnitEntity unitEntity)
        {
            self.PublishEvent(new RemoveUnitEntity(){UnitEntity = unitEntity});
            self.AllEntity.Remove(unitEntity.InsId);
            unitEntity.Dispose();
        }
        
        public static void PublishEvent<T>(this World self, T args) where T : struct
        {
            var events = BattleEventManagerComponent.Instance.AllEvents.GetValueOrDefault(typeof(T));
            if (events == null)
            {
                return;
            }

            foreach (var e in events)
            {
                if (e.WorldMode != self.WorldMode)
                {
                    continue;
                }
                
                if (!(e.BattleEvent is ABattleEvent<T> aEvent))
                {
                    Log.Error($"Battle Event Error: {e.GetType().FullName}");
                    continue;
                }
                
                aEvent.Handle(self, args);
            }
        }

        
        public static BattleWorld ToBattleWorld(this World world)
        {
            BattleWorld battleWorld = BattleWorld.Create();
            battleWorld.WorldStatus = world.WorldStatusEnum;
            battleWorld.Frame = world.Frame;
            return battleWorld;
        }
    }
}