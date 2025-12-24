namespace ET.Server;


[EntitySystemOf(typeof(BattleRoleComponent))]
[FriendOf(typeof(BattleRoleComponent))]
public static partial class BattleRoleComponentSystem
{
    [EntitySystem]
    private static void Awake(this BattleRoleComponent self)
    {
        BattleRoleComponent.Instance = self;
    }
    
    [EntitySystem]
    private static void Destroy(this BattleRoleComponent self)
    {
        
    }
}