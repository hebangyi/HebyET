namespace ET.Server;

[EntitySystemOf(typeof(MongoCacheAgentComponent))]
[FriendOf(typeof(MongoCacheAgentComponent))]
public static partial class MongoCacheAgentComponentSystem
{
    [EntitySystem]
    private static void Awake(this MongoCacheAgentComponent self)
    {
        
    }
}