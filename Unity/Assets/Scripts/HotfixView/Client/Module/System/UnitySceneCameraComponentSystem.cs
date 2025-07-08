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
            // TODO 研究一下需不需要控制频率
            UnitEntity flowUnitEntity = self.FlowUnitEntity;
            if (flowUnitEntity != null)
            {
                var unitEntityGameObjectComponent = flowUnitEntity.GetComponent<UnitEntityGameObjectComponent>();
                if (unitEntityGameObjectComponent != null && unitEntityGameObjectComponent.Transform)
                {
                    self.MainCamera.transform.position = new float3(unitEntityGameObjectComponent.Transform.position) + self.OffsetPosition;
                    self.MainCamera.transform.LookAt(unitEntityGameObjectComponent.Transform.position);
                }
            }
        }

        public static void SetFlowUnitEntity(this UnitySceneCameraComponent self, UnitEntity unitEntity)
        {
            self.FlowUnitEntity = unitEntity;
        }
    }
}
