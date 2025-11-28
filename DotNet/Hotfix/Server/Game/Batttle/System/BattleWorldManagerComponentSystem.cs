using System;
using System.Collections.Generic;
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
        var syncPlayerDirtyBattleDataHandler = new SyncPlayerDirtyBattleDataHandler(world, SyncDirtyBattleData);
        
        
        // 同步AOI组件
        world.AddComponent<AOIManagerComponent, IDirtyHandler, ISyncHandler>(logicDirtyHandler, syncPlayerDirtyBattleDataHandler);
        world.WorldStatusEnum = WorldStatusEnum.Init;
        world.RandomGenerator = new Random(Guid.NewGuid().GetHashCode());
        
        // 创建地图
        PlantGenContext plantGenContext = new ();
        plantGenContext.InitData.AreaSize = 2000;
        plantGenContext.InitData.PointCount = 200;
        plantGenContext.InitData.Random = world.RandomGenerator;
        plantGenContext.InitData.NearEdgeMinDistance = 5;
        plantGenContext.InitData.GenCellCount = 25;
        
        
        UnitEntity unitEntityPlant = UnitPlaneHelper.GeneratePlane(world, plantGenContext);
        
        // 创建玩家
        foreach (var matchOrder in matchRoom.MatchOrders)
        {
            UnitPlayerHelper.Create(world, matchOrder.PlayerId);
        }
        
        // 创建环境
        var unitEntityMapMessage = unitEntityPlant.GetUnitEntityElemData<UnitEntityMapMessage>();
        foreach (var cellInfo in unitEntityMapMessage.PlantInfo.CellInfos)
        {
            UnitEnvHelper.Create(world, cellInfo.CenterPoint);
        }
        
        
        self.Worlds[world.Id] = world;
        
        world.WorldStatusEnum = WorldStatusEnum.Battle;
        self.UpdateQueue.AddFirst(world);
        return world;
    }

    public static void SyncDirtyBattleData(LogicWorld logicWorld)
    {
        if (logicWorld.DirtyUnitEntities.Count == 0)
        {
            return;
        }

        L2C_PlayerAOIWorldDirtyPush message = L2C_PlayerAOIWorldDirtyPush.Create();
        foreach (var dirtyUnitEntityKv in logicWorld.DirtyUnitEntities)
        {
            var battleUnitEntity = dirtyUnitEntityKv.Value.ToBattleUnitEntity();
            message.DirtyUnitEntities.Add(battleUnitEntity);
        }

        foreach (var playerId in logicWorld.PlayerId2Players.Keys)
        {
            var battleRole = BattleRoleComponent.Instance.GetByRoleId(playerId);
            if (battleRole != null)
            {
                battleRole.SendToClient(message);
            }
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