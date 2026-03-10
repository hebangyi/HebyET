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
            self.CameraPack = GlobalComponent.Instance.CameraPack;
            
            float height = 15;
            float degree = 45;
            float behind = height / (float)(Math.Tan(degree * Mathf.Deg2Rad)) * -1;
            self.OffsetPosition = new float3(0, behind, -height);

            var transform = self.MainCamera.transform;
            transform.position = self.OffsetPosition;
            transform.rotation = Quaternion.identity;
            self.MainCamera.transform.transform.Rotate(new Vector3(-45, 0, 0));
            
            UpdateLogicManagerComponent.Instance.AddUpdateFunc(self.CameraRotateUpdate);
        }

        

        [EntitySystem]
        private static void LateUpdate(this UnitySceneCameraComponent self)
        {
            // TODO 研究一下需不需要控制频率
            ClientUnitEntity flowUnitEntity = self.FlowUnitEntity;
            if (flowUnitEntity == null)
            {
                return;
            }
            
            var unitEntityGameObjectComponent = flowUnitEntity.GetComponent<UnitEntityGameObjectComponent>();
            if (unitEntityGameObjectComponent == null)
            {
                return;
            }

            var gameObject = unitEntityGameObjectComponent.GameObject;
            if (gameObject.transform.position == self.CameraPack.transform.position)
            {
                return;
            }
            
            self.CameraPack.transform.position = new float3(unitEntityGameObjectComponent.Transform.position);
            flowUnitEntity.ClientWorld().PublishEvent(new ClientCameraPositionUpdate());
        }

    }
}
