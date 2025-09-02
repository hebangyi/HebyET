using System;
using Unity.Mathematics;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(UnitySceneCameraComponent))]
    public static partial class UnitySceneCameraComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UnitySceneCameraComponent self)
        {
            self.MainCamera = GlobalComponent.Instance.MainCamera;
            float height = 15;
            float degree = 45;
            float behind = height / (float)(Math.Tan(degree * Mathf.Deg2Rad)) * -1;
            self.OffsetPosition = new float3(0, height, behind);
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
