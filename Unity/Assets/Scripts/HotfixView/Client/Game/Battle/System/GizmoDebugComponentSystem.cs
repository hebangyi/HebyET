namespace ET.Client
{

    [EntitySystemOf(typeof(GizmoDebugComponent))]
    [FriendOf(typeof(GizmoDebugComponent))]
    public static partial class GizmoDebugComponentSystem
    {
        [EntitySystem]
        private static void Awake(this GizmoDebugComponent self)
        {
            GizmoDebugComponent.Instance = self;
        }
    }
}
