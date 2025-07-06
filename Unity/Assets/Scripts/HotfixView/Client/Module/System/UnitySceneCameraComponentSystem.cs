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
                var unitEntityPosition = flowUnitEntity.GetUnitEntityElemData<UnitEntityPosition>();
                if (unitEntityPosition != null)
                {
                    var position = new float3(unitEntityPosition.Position.x, unitEntityPosition.Position.y, unitEntityPosition.Position.z);
                    self.MainCamera.transform.position = position + self.OffsetPosition;
                    self.MainCamera.transform.LookAt(position);
                }
            }
        }

        public static void SetFlowUnitEntity(this UnitySceneCameraComponent self, UnitEntity unitEntity)
        {
            self.FlowUnitEntity = unitEntity;
        }
    }
}
