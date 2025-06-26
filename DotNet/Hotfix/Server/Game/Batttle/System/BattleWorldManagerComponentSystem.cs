using System.Collections.Generic;

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
        var world = self.AddChild<World>();
        // 创建玩家
        foreach (var matchOrder in matchRoom.MatchOrders)
        {
            UnitPlayerFactory.Create(world, matchOrder.PlayerId);
        }
        
        self.Worlds[world.Id] = world;
        return world;
    }
    
    
    [EntitySystem]
    private static void Awake(this ET.Server.BattleWorldManagerComponent self)
    {
        BattleWorldManagerComponent.Instance = self;
    }
}