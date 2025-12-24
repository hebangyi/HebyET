namespace ET
{
    [FriendOf(typeof(MonsterStateMachineComponent))]
    [EntitySystemOf(typeof(MonsterStateMachineComponent))]
    public static partial class MonsterStateMachineComponentSystem
    {
        [EntitySystem]
        private static void Awake(this MonsterStateMachineComponent self)
        {
            self.ChangeState(MachineStateEnum.Default);
        }
    }
}

