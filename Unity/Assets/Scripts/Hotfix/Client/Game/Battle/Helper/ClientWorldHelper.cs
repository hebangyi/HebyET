using System.Collections.Generic;

namespace ET.Client
{
    public static class ClientWorldHelper
    {
        public static void InitWorld(this World world, BattleWorld battleWorld, List<BattleUnitEntity> battleUnitEntities)
        {
            world.WorldStatusEnum = battleWorld.WorldStatus;
            world.Frame = battleWorld.Frame;

            foreach (var battleUnitEntity in battleUnitEntities)
            {
                world.CreateEntity(battleUnitEntity);
            }
        }

        public static UnitEntity CreateEntity(this World self, BattleUnitEntity battleUnitEntity)
        {
            var unitEntity = self.AddChild<UnitEntity>();
            unitEntity.InsId = battleUnitEntity.InsId;
            self.AllEntity[unitEntity.InsId] = unitEntity;
            foreach (var elemData in battleUnitEntity.EleDatas)
            {
                var componentId = elemData.CompId;
                var unitElemType = OpcodeType.Instance.GetType(componentId);
                if (unitElemType == null)
                {
                    Log.Error($"没有找到UnitEntity 组件 {componentId} 对应的数据类型");
                    continue;
                }
                
                var unitEntityElemData = MemoryPackHelper.Deserialize(unitElemType, elemData.ElemDatas, 0, elemData.ElemDatas.Length) as IUnitEntityElemData;
                unitEntity.UnitEntityData[componentId] = unitEntityElemData;
            }
            
            self.PublishEvent(new CreateUnitEntityEvent0(){UnitEntity = unitEntity});
            self.PublishEvent(new CreateUnitEntityEvent1(){UnitEntity = unitEntity});
            self.PublishEvent(new CreateUnitEntityEvent2(){UnitEntity = unitEntity});
            return unitEntity;
        }
        
        
        public static void UpdateDirty(this World self, List<BattleUnitEntity> battleUnitEntities)
        {
            foreach (var battleUnitEntity in battleUnitEntities)
            {
                UnitEntity unitEntity = self.AllEntity.GetValueOrDefault(battleUnitEntity.InsId);
                if (unitEntity != null)
                {
                    // 更新
                    foreach (var elemData in battleUnitEntity.EleDatas)
                    {
                        var componentId = elemData.CompId;
                        var unitElemType = OpcodeType.Instance.GetType(componentId);
                        if (unitElemType == null)
                        {
                            Log.Error($"没有找到UnitEntity 组件 {componentId} 对应的数据类型");
                            continue;
                        }

                        var newUnitEntityElemData =
                                MemoryPackHelper.Deserialize(unitElemType, elemData.ElemDatas, 0, elemData.ElemDatas.Length) as IUnitEntityElemData;
                        var oleUnitEntityElemData = unitEntity.UnitEntityData.GetValueOrDefault(componentId);
                        unitEntity.UnitEntityData[componentId] = newUnitEntityElemData;
                        self.PublishEvent(new UpdateUnitEntityElementDirtyData() { ComponentId = componentId, UnitEntity = unitEntity, OldUnitEntityElemData = oleUnitEntityElemData, NewUnitEntityElemData = newUnitEntityElemData});
                    }
                }
                else
                {
                    // 新建
                    self.CreateEntity(battleUnitEntity);
                }
            }
        }
    }
}