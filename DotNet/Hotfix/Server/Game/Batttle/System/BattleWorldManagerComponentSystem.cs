using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Random = System.Random;

namespace ET.Server;

[EntitySystemOf(typeof(BattleWorldManagerComponent))]
[FriendOf(typeof(BattleWorldManagerComponent))]
public static partial class BattleWorldManagerComponentSystem
{
    public static LogicWorld GetWorldById(this BattleWorldManagerComponent self, long worldId)
    {
        return self.Worlds.GetValueOrDefault(worldId);
    }
    
    public static LogicWorld CreateWorld(this BattleWorldManagerComponent self, MatchRoom matchRoom)
    {
        var world = self.AddChild<LogicWorld>();
        var logicDirtyHandler = new LogicDirtyHandler(world);
        var syncDirtyDataHandler = new SyncPlayerDirtyBattleDataHandler(world, SyncDirtyBattleData);
        
        // 同步AOI组件
        world.AddComponent<AOIManagerComponent, IDirtyHandler, ISyncHandler>(logicDirtyHandler, syncDirtyDataHandler);
        world.WorldStatusEnum = WorldStatusEnum.Init;
        world.RandomGenerator = new Random(Guid.NewGuid().GetHashCode());
        
        // AI组件
        world.AddComponent<AIComponent>();
        
        // 创建地图
        var config = BattleGlobalConfigCategory.Instance.Config;
        var battleMapConfig = BattleMapConfigCategory.Instance.GetById(config.TestMapConfigId);
        
        
        PlantGenContext plantGenContext = new ();
        plantGenContext.InitData.AreaSize = battleMapConfig.AreaSize;
        plantGenContext.InitData.CellPointCount = battleMapConfig.CellPointCount;
        plantGenContext.InitData.CellPointMinDistance = battleMapConfig.CellPointMinDistance;
        plantGenContext.InitData.GenCellCount = battleMapConfig.GenCellCount;
        plantGenContext.InitData.Random = world.RandomGenerator;
        BattleMapHelper.GenerateBattleCells(plantGenContext);
        
        
        UnitEntity unitEntityPlant = UnitPlaneHelper.GeneratePlane(world, plantGenContext);
        
        // 创建玩家
        foreach (var matchOrder in matchRoom.MatchOrders)
        {
            UnitPlayerHelper.Create(world, matchOrder.PlayerId);
        }
        
        // 创建环境
        // 创建怪物
        var unitEntityMapMessage = unitEntityPlant.GetUnitEntityElemData<UnitEntityMapMessage>();
        foreach (var cellInfo in unitEntityMapMessage.PlantInfo.CellInfos)
        {
            long monsterId = RandomHelper.NextLong(1, 11);
            UnitTreeHelper.Create(world, cellInfo.CenterPoint);
            UnitMonsterHelper.Create(world, monsterId, cellInfo.CenterPoint + new float2(10, 0));
        }
        
        self.Worlds[world.Id] = world;
        
        world.WorldStatusEnum = WorldStatusEnum.Battle;
        world.NowMilliTime = TimeInfo.Instance.NowMillTime();
        self.UpdateQueue.AddFirst(world);
        return world;
    }

    public static void SyncDirtyBattleData(LogicWorld logicWorld)
    {
        if (logicWorld.DirtyUnitEntities.Count == 0)
        {
            return;
        }

        foreach (var pKV in logicWorld.PlayerId2Players)
        {
            long playerId = pKV.Key;
            var unitEntity = pKV.Value;
            var playerUnitEntityInsId = unitEntity.InsId;
            bool hasDirtyData = false;
            
            var playerAOISeeUnitEntity = unitEntity.GetComponent<PlayerAOISeeUnitEntity>();
            
            L2C_PlayerAOIWorldDirtyPush message = L2C_PlayerAOIWorldDirtyPush.Create(true);
            foreach (var enterEntityId in playerAOISeeUnitEntity.EnterEntityIds)
            {
                if (playerAOISeeUnitEntity.ManageEntityIds.Add(enterEntityId))
                {
                    var enterEntity = logicWorld.AllEntities.GetValueOrDefault(enterEntityId);
                    if (enterEntity == null)
                    {
                        continue;
                    }
                    message.AddUnitEntiities.Add(enterEntity.ToBattleUnitEntity());
                    hasDirtyData = true;
                }
            }
            
            foreach (var leaveEntityId in playerAOISeeUnitEntity.LeaveEntityIds)
            {
                if (playerAOISeeUnitEntity.ManageEntityIds.Remove(leaveEntityId))
                {
                    message.DeleteUnitEntites.Add(leaveEntityId);
                    hasDirtyData = true;
                }
            }
            
            // 增量脏数据
            foreach (var dirtyUnitEntityKv in logicWorld.DirtyUnitEntities)
            {
                var instanceId = dirtyUnitEntityKv.Key;
                if (!playerAOISeeUnitEntity.ManageEntityIds.Contains(instanceId) && instanceId != playerUnitEntityInsId)
                {
                    continue;
                }
                
                BattleUnitEntity battleUnitEntity = dirtyUnitEntityKv.Value.ToBattleUnitEntity();
                message.DirtyUnitEntities.Add(battleUnitEntity);
                hasDirtyData = true;
            }

            if (hasDirtyData)
            {
                var battleRole = BattleRoleComponent.Instance.GetByRoleId(playerId);
                if (battleRole != null)
                {
                    var lastSyncWorldFrame = battleRole.LastSyncWorldFrame;
                    battleRole.LastSyncWorldFrame = logicWorld.Frame;

                    message.LastSyncFrame = lastSyncWorldFrame;
                    message.CurrentSyncFrame = battleRole.LastSyncWorldFrame;
                    battleRole.SendToClient(message);
                }
            }
            
            // 清除玩家的AOI计算
            playerAOISeeUnitEntity.ClearPlayerAOI();
            message.Dispose();
        }
        
        logicWorld.DirtyUnitEntities.Clear();
    }
    
    
    [EntitySystem]
    private static void Awake(this ET.Server.BattleWorldManagerComponent self)
    {
        BattleWorldManagerComponent.Instance = self;
    }
    
    
    [EntitySystem]
    private static void Update(this ET.Server.BattleWorldManagerComponent self)
    {
        long now = TimeInfo.Instance.NowMillTime();
        while (self.UpdateQueue.Count > 0)
        {
            var world = self.UpdateQueue.First?.Value;
            if (world == null)
            {
                return;
            }
            
            if (world.NextUpdateMillTime > now)
            {
                return;
            }
            
            // 移除列表头
            self.UpdateQueue.RemoveFirst();
            if (world.WorldStatusEnum == WorldStatusEnum.Finish)
            {
                continue;
            }
            
            if (world.NextUpdateMillTime == 0)
            {
                world.NextUpdateMillTime = now + world.Interval;    
            }
            else
            {
                world.NextUpdateMillTime += world.Interval;
            }
            world.Tick();
            self.UpdateQueue.AddLast(world);
        }
    }
}