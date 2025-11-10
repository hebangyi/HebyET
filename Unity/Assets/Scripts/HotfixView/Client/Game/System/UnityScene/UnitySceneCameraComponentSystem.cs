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
            self.OffsetPosition = new float3(0, height, behind);
            
            self.MainCamera.transform.position = self.OffsetPosition;
            self.MainCamera.transform.rotation = Quaternion.identity;
            self.MainCamera.transform.transform.Rotate(new Vector3(45, 0, 0));
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
                    self.CameraPack.transform.position = new float3(unitEntityGameObjectComponent.Transform.position);
                }
            }
        }

        public static void SetFlowUnitEntity(this UnitySceneCameraComponent self, UnitEntity unitEntity)
        {
            self.FlowUnitEntity = unitEntity;
        }
        
        public static void SetCameraRotate(this UnitySceneCameraComponent self, int yAngle)
        {
            self.CameraPack.transform.rotation = Quaternion.Euler(0, yAngle, 0);
            var clientWorld = UnitySceneClientWorldManagerComponent.Instance.CurrentClientWorld;
            if (clientWorld == null)
            {
                return;
            }
            
            // 所有的 环境 UnitEntity 朝向移动
            foreach (var unitEntity in clientWorld.EvnUnitEntities.Values)
            {
                var unitEntityGameObjectComponent = unitEntity.GetComponent<UnitEntityGameObjectComponent>();
                var gameObject = unitEntityGameObjectComponent.GameObject;
                gameObject.transform.rotation = Quaternion.Euler(GameConstant.GameOperaAngle, yAngle, 0);
            }
            
        }
    }
}
