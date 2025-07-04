using Unity.Mathematics;

namespace ET.Client
{
    [EntitySystemOf(typeof(UnitySceneCameraComponent))]
    public static partial class UnitySceneCameraComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UnitySceneCameraComponent self)
        {
            self.MainCamera = GlobalComponent.Instance.MainCamera;
            self.OffsetPosition = new float3(0, 10f, -6f);
        }


        [EntitySystem]
        private static void LateUpdate(this UnitySceneCameraComponent self)
        {
            // TODO 跟随 Unit 的相对位置
            // TODO 研究一下需不需要控制频率
            self.MainCamera.transform.position = self.OffsetPosition;
            self.MainCamera.transform.LookAt(new float3(0f, 0f, 0f));
        }
    }
}
