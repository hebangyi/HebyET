namespace ET.Server;


[EntitySystemOf(typeof(LobbySyncUnitDataComponent))]
[FriendOf(typeof(LobbySyncUnitDataComponent))]
[FriendOf(typeof(LobbySyncUnitDataComponent))]
public static partial class LobbySyncUnitDataComponentSystem
{
    [EntitySystem]
    public static void Awake(this LobbySyncUnitDataComponent self)
    {
    }
}