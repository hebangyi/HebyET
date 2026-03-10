using System;
using System.Collections.Generic;

namespace ET.Client
{
    public static class ClientWorldHelper
    {
        public static void InitWorld(this ClientWorld world, BattleWorld battleWorld, int logicInterval)
        {
            world.Frame = battleWorld.Frame;
            world.LogicInterval = logicInterval;
        }

        public static void HandleDirtyMessage(this ClientWorld world, L2C_PlayerAOIWorldDirtyPush message)
        {
            world.Frame = message.CurrentSyncFrame;
            world.AddBattleUnits(message.AddUnitEntiities);
            world.UpdateDirty(message.DirtyUnitEntities);
            world.DeleteEntities(message.DeleteUnitEntites);
        }

        
        public static void AddBattleUnits(this ClientWorld world, List<BattleUnitEntity> battleUnitEntities)
        {
            foreach (var battleUnitEntity in battleUnitEntities)
            { 
                world.AddBattleUnit(battleUnitEntity);
            }
        }

        public static void AddBattleUnit(this ClientWorld world, BattleUnitEntity battleUnitEntity)
        {
            
            if (world.AllEntities.ContainsKey(battleUnitEntity.InsId))
            {
                return;
            }

            var unitEntity = world.DeserializeUnitEntity(battleUnitEntity);
            world.PublishUnitEntityCreateEvent(unitEntity);
        }

        public static ClientUnitEntity DeserializeUnitEntity(this ClientWorld self, BattleUnitEntity battleUnitEntity)
        {
            var unitEntity = self.AddChildWithId<ClientUnitEntity>(battleUnitEntity.InsId);
            unitEntity.InsId = battleUnitEntity.InsId;
            self.AllEntities[unitEntity.InsId] = unitEntity;
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

        public static ClientUnitEntity PublishUnitEntityCreateEvent(this ClientWorld self, ClientUnitEntity unitEntity)
        {
            Log.Info($"Create UnitEntity : {unitEntity.InsId}");
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

        public static void DeleteEntities(this ClientWorld self, List<long> instanceIds)
        {
            foreach (var instanceId in instanceIds)
            {
                var unitEntity = self.AllEntities.GetValueOrDefault(instanceId);
                if (unitEntity == null)
                {
                    continue;
                }

                foreach (var unitEntityElemDataKv in unitEntity.UnitEntityData)
                {
                    self.PublishEvent(new ClientDestroyElementData()
                            { UnitEntity = unitEntity, UnitEntityElemData = unitEntityElemDataKv.Value, ComponentId = unitEntityElemDataKv.Key });
                }
                
                self.PublishEvent(new ClientRemoveUnitEntity() { UnitEntity = unitEntity });
                self.AllEntities.Remove(unitEntity.InsId);
                unitEntity.Dispose();
            }
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
            List<ClientUpdateElementData> clientUpdateElementDatas = new List<ClientUpdateElementData>();
            foreach (var battleUnitEntity in battleUnitEntities)
            {
                ClientUnitEntity unitEntity = self.AllEntities.GetValueOrDefault(battleUnitEntity.InsId);
                if (unitEntity == null)
                {
                    Log.Error($"UpdateDirty 出错, 没有找到实体 {battleUnitEntity.InsId}");
                    continue;
                }

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
                    // 更新数据
                    unitEntity.UnitEntityData[componentId] = newUnitEntityElemData;
                    clientUpdateElementDatas.Add(new ClientUpdateElementData()
                    {
                        ComponentId = componentId, UnitEntity = unitEntity, OldUnitEntityElemData = oleUnitEntityElemData,
                        NewUnitEntityElemData = newUnitEntityElemData
                    });
                }
            }

            // 抛出事件
            foreach (var clientUpdateElementData in clientUpdateElementDatas)
            {
                self.PublishEvent(clientUpdateElementData);
            }
        }
    }
}