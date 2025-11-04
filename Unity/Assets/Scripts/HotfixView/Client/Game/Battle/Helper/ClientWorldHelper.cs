using System;
using System.Collections.Generic;

namespace ET.Client
{
    public static class ClientWorldHelper
    {
        public static async ETTask InitWorld(this ClientWorld world, BattleWorld battleWorld, List<BattleUnitEntity> battleUnitEntities)
        {
            world.Frame = battleWorld.Frame;
            await world.CreateEntities(battleUnitEntities);
        }

        public static async ETTask CreateEntities(this ClientWorld world, List<BattleUnitEntity> battleUnitEntities)
        {
            List<UnitEntity> unitEntities = new List<UnitEntity>();
            foreach (var battleUnitEntity in battleUnitEntities)
            {
                var createUnitEntity = world.DeserializeUnitEntity(battleUnitEntity);
                unitEntities.Add(createUnitEntity);
            }
            
            // 抛出事件
            foreach (var unitEntity in unitEntities)
            {
                world.PublishUnitEntityCreateEvent(unitEntity);
            }
        }

        public static UnitEntity DeserializeUnitEntity(this ClientWorld self, BattleUnitEntity battleUnitEntity)
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

                var unitEntityElemData =
                        MemoryPackHelper.Deserialize(unitElemType, elemData.ElemDatas, 0, elemData.ElemDatas.Length) as IUnitEntityElemData;
                unitEntity.UnitEntityData[componentId] = unitEntityElemData;
            }

            return unitEntity;
        }

        public static UnitEntity PublishUnitEntityCreateEvent(this ClientWorld self, UnitEntity unitEntity)
        {
            // 初始化 UnitEntity
            self.PublishEvent(new ClientCreateUnitEntity0() { UnitEntity = unitEntity });
            self.PublishEvent(new ClientCreateUnitEntity1() { UnitEntity = unitEntity });
            self.PublishEvent(new ClientCreateUnitEntity2() { UnitEntity = unitEntity });

            // 初始化 Element
            foreach (var unitEntityElemDataKv in unitEntity.UnitEntityData)
            {
                self.PublishEvent(new ClientInitElementData()
                        { UnitEntity = unitEntity, UnitEntityElemData = unitEntityElemDataKv.Value, ComponentId = unitEntityElemDataKv.Key });
            }

            return unitEntity;
        }

        public static void RemoveEntity(this ClientWorld self, UnitEntity unitEntity)
        {
            self.PublishEvent(new RemoveUnitEntity() { UnitEntity = unitEntity });
            self.AllEntity.Remove(unitEntity.InsId);
            unitEntity.Dispose();
        }

        public static void PublishEvent<T>(this ClientWorld self, T args) where T : struct
        {
            var events = ClientWorldEventManagerComponent.Instance.AllEvents.GetValueOrDefault(typeof(T));
            if (events == null)
            {
                return;
            }

            foreach (var e in events)
            {
                if (!(e is AClientWorldEvent<T> aEvent))
                {
                    Log.Error($"Battle Event Error: {e.GetType().FullName}");
                    continue;
                }

                aEvent.Handle(self, args).Coroutine();
            }
        }

        public static async ETTask PublishEventAsync<T>(this ClientWorld self, T args) where T : struct
        {
            var events = ClientWorldEventManagerComponent.Instance.AllEvents.GetValueOrDefault(typeof(T));
            if (events == null)
            {
                return;
            }

            using ListComponent<ETTask> list = ListComponent<ETTask>.Create();

            foreach (var e in events)
            {
                if (!(e is AClientWorldEvent<T> aEvent))
                {
                    Log.Error($"Battle Event Error: {e.GetType().FullName}");
                    continue;
                }

                list.Add(aEvent.Handle(self, args));
            }

            try
            {
                await ETTaskHelper.WaitAll(list);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public static void UpdateDirty(this ClientWorld self, List<BattleUnitEntity> battleUnitEntities)
        {
            List<BattleUnitEntity> createBattleUnitEntities = null;
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
                        self.PublishEvent(new ClientUpdateElementData()
                        {
                            ComponentId = componentId, UnitEntity = unitEntity, OldUnitEntityElemData = oleUnitEntityElemData,
                            NewUnitEntityElemData = newUnitEntityElemData
                        });
                    }
                }
                else
                {
                    if (createBattleUnitEntities == null)
                    {
                        createBattleUnitEntities = new List<BattleUnitEntity>();
                    }
                    
                    createBattleUnitEntities.Add(battleUnitEntity);
                }
            }

            if (createBattleUnitEntities != null && createBattleUnitEntities.Count > 0)
            {
                // 新建
                self.CreateEntities(createBattleUnitEntities).Coroutine();
            }
        }
    }
}