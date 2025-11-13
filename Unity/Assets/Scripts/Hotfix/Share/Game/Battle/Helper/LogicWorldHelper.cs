using System.Collections.Generic;
using ET.Client;

namespace ET
{
    public  static partial class LogicWorldHelper
    {
        public static UnitEntity 
                CreateEntity(this LogicWorld self)
        {
            var unitEntity = self.AddChild<UnitEntity>();
            unitEntity.InsId = unitEntity.Id;
            self.AllEntity[unitEntity.InsId] = unitEntity;
            return unitEntity;
        }

        public static void CreateEntityFinish(this LogicWorld self, UnitEntity unitEntity)
        {
            self.PublishEvent(new CreateUnitEntityEvent0(){UnitEntity = unitEntity});
            self.PublishEvent(new CreateUnitEntityEvent1(){UnitEntity = unitEntity});
            self.PublishEvent(new CreateUnitEntityEvent2(){UnitEntity = unitEntity});

            foreach (var unitEntityElemDataKv in unitEntity.UnitEntityData)
            {
                self.PublishEvent(new CreateUnitEntityElementData(){UnitEntity = unitEntity, UnitEntityElemData = unitEntityElemDataKv.Value, ComponentId = unitEntityElemDataKv.Key});
            }
        }
        
        
        public static void RemoveEntity(this LogicWorld self, UnitEntity unitEntity)
        {
            self.PublishEvent(new RemoveUnitEntity(){UnitEntity = unitEntity});
            self.AllEntity.Remove(unitEntity.InsId);
            unitEntity.Dispose();
        }
        
        public static void PublishEvent<T>(this LogicWorld self, T args) where T : struct
        {
            var events = BattleEventManagerComponent.Instance.AllEvents.GetValueOrDefault(typeof(T));
            if (events == null)
            {
                return;
            }

            foreach (var e in events)
            {
                if (!(e is ABattleEvent<T> aEvent))
                {
                    Log.Error($"Battle Event Error: {e.GetType().FullName}");
                    continue;
                }
                
                aEvent.Handle(self, args);
            }
        }

        public static void ClientInput(UnitEntity unitEntity, BattleUnitEntity battleUnitEntity)
        {
            var elementList = battleUnitEntity.EleDatas;
            foreach (var element in elementList)
            {
                var compId = element.CompId;
                var unitElemType = OpcodeType.Instance.GetType(compId);

                if (unitElemType == null)
                {
                    Log.Error($"ClientInput 没有找到UnitEntity 组件 {compId} 对应的数据类型");
                    continue;
                }
                
                var newUnitEntityElemData =
                        MemoryPackHelper.Deserialize(unitElemType, element.ElemDatas, 0, element.ElemDatas.Length) as IUnitEntityElemData;

                if (newUnitEntityElemData == null)
                {
                    Log.Error($"ClientInput 反序列化 UnitEntity Element 组件 {compId} 失败!");
                    continue;
                }
                
                var oleUnitEntityElemData = unitEntity.UnitEntityData.GetValueOrDefault(compId);
                if (oleUnitEntityElemData == null)
                {
                    Log.Error($"ClientInput 没有 UnitEntity Element 组件 {compId} 失败!");
                    continue;
                }

                var elementLogic = BattleUnitEntityLogicManagerComponent.Instance.ClientInputLogics.GetValueOrDefault(compId);
                if (elementLogic == null)
                {
                    return;
                }

                bool canInput = elementLogic.CanInput(unitEntity, newUnitEntityElemData);
                // TODO 做操作回退
                if (!canInput)
                {
                    continue;
                }
                
                // 更新数据
                unitEntity.UnitEntityData[compId] = newUnitEntityElemData;
                
                // 记录Dirty
                unitEntity.Dirty(newUnitEntityElemData);
                
                // 
                elementLogic.Updated(unitEntity);
            }
        }
        
        
        public static BattleWorld ToBattleWorld(this LogicWorld logicWorld)
        {
            BattleWorld battleWorld = BattleWorld.Create();
            battleWorld.WorldStatus = logicWorld.WorldStatusEnum;
            battleWorld.Frame = logicWorld.Frame;
            return battleWorld;
        }
    }
}