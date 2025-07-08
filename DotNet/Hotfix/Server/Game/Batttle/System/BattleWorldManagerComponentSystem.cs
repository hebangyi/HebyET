using System.Collections.Generic;
using ET.Client;

namespace ET.Server;

[EntitySystemOf(typeof(BattleWorldManagerComponent))]
[FriendOf(typeof(BattleWorldManagerComponent))]
public static partial class BattleWorldManagerComponentSystem
{
    public static World GetWorldById(this BattleWorldManagerComponent self, long worldId)
    {
        return self.Worlds.GetValueOrDefault(worldId);
    }
    
    public static World CreateWorld(this BattleWorldManagerComponent self, MatchRoom matchRoom)
    {
        var world = self.AddChild<World, int>((int)WorldMode.Logic);
        world.WorldStatusEnum = WorldStatusEnum.Init;

        var syncPlayerDirtyBattleDataHandler = new SyncPlayerDirtyBattleDataHandler(world, SyncDirtyBattleData);
        var logicDirtyHandler = new LogicDirtyHandler(world);
        
        world.InitDirtyHandler(logicDirtyHandler);
        world.InitSyncHandler(syncPlayerDirtyBattleDataHandler);
        
        // 创建玩家
        foreach (var matchOrder in matchRoom.MatchOrders)
        {
            UnitPlayerHelper.Create(world, matchOrder.PlayerId);
        }
        
        self.Worlds[world.Id] = world;
        
        world.WorldStatusEnum = WorldStatusEnum.Battle;
        self.UpdateQueue.AddFirst(world);
        return world;
    }

    public static void SyncDirtyBattleData(World world)
    {
        if (world.DirtyUnitEntities.Count == 0)
        {
            return;
        }

        L2C_PlayerAOIWorldDirtyPush message = L2C_PlayerAOIWorldDirtyPush.Create();
        foreach (var dirtyUnitEntityKv in world.DirtyUnitEntities)
        {
            var battleUnitEntity = dirtyUnitEntityKv.Value.ToBattleUnitEntity();
            message.DirtyUnitEntities.Add(battleUnitEntity);
        }

        foreach (var playerId in world.AllPlayers.Keys)
        {
            var battleRole = BattleRoleComponent.Instance.GetByRoleId(playerId);
            if (battleRole != null)
            {
                battleRole.SendToClient(message);
            }
        }

        world.DirtyUnitEntities.Clear();
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