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
            
            self.MainCamera.transform.position = self.OffsetPosition;
            self.MainCamera.transform.rotation = Quaternion.identity;
            self.MainCamera.transform.transform.Rotate(new Vector3(-45, 0, 0));
            
            
            UpdateLogicManagerComponent.Instance.AddUpdateFunc(self.CameraRotateUpdate);
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

        public static void InitCameraRotate(this UnitySceneCameraComponent self, float yAngle)
        {
            self.SetCameraRotate(yAngle);
        }
        
        public static void SetCameraRotate(this UnitySceneCameraComponent self, float yAngle)
        {
            self.CameraPack.transform.rotation = Quaternion.Euler(0, 0, yAngle);
            var clientWorld = UnitySceneClientWorldManagerComponent.Instance.CurrentClientWorld;
            if (clientWorld == null)
            {
                return;
            }
            
            // 所有的 环境 UnitEntity 朝向移动
            var env = GlobalComponent.Instance.Env;
            for (int i = 0; i < env.childCount; i++)
            {
                var child = env.GetChild(i).gameObject;
                child.transform.rotation = Camera.main.transform.rotation;
            }
            

            var player = GlobalComponent.Instance.Player;
            for (int i = 0; i < player.childCount; i++)
            {
                var child = player.GetChild(i).gameObject;
                child.transform.rotation = Camera.main.transform.rotation;
            }
            
            var monster = GlobalComponent.Instance.Monster;
            for (int i = 0; i < monster.childCount; i++)
            {
                var child = monster.GetChild(i).gameObject;
                child.transform.rotation = Camera.main.transform.rotation;
            }
            
            // 刷新 所有动画骨骼
            foreach (var unitEntity in clientWorld.AllEntities.Values)
            {
                AnimationHelper.ReCalUnitEntityAnimationSkeleton(unitEntity);
            }
        }
        
        
        public static void AddTargetCameraRotate(this UnitySceneCameraComponent self, int yAngle)
        {
            var mainPlayer = MainPlayerHelper.GetCurrentWorldMainPlayer();
            
            var playerCacheDataComponent = mainPlayer.GetComponent<MyPlayerCacheDataComponent>();
            if (playerCacheDataComponent == null)
            {
                return;
            }


            playerCacheDataComponent.TargetCameraAngleOffSet += yAngle;
        }


        public static void CameraRotateUpdate(this UnitySceneCameraComponent self, long deleteTime)
        {
            var mainPlayer = MainPlayerHelper.GetCurrentWorldMainPlayer();
            if (mainPlayer == null)
            {
                return;
            }
            
            var playerCacheDataComponent = mainPlayer.GetComponent<MyPlayerCacheDataComponent>();
            if (playerCacheDataComponent == null)
            {
                return;
            }
            
            if (Mathf.Approximately(playerCacheDataComponent.TargetCameraAngleOffSet, playerCacheDataComponent.CameraAngleOffSet))
            {
                return;
            }
            
            // 0.5s 转 90度
            // TODO 相机平移
            var angle = deleteTime * 1.0f / GameConstant.CameraRotationSpeed * 90;
            if (playerCacheDataComponent.CameraAngleOffSet < playerCacheDataComponent.TargetCameraAngleOffSet)
            {
                playerCacheDataComponent.CameraAngleOffSet = Math.Min(playerCacheDataComponent.CameraAngleOffSet + angle, playerCacheDataComponent.TargetCameraAngleOffSet);
            }
            else
            {
                playerCacheDataComponent.CameraAngleOffSet = Math.Max(playerCacheDataComponent.CameraAngleOffSet - angle, playerCacheDataComponent.TargetCameraAngleOffSet);
            }

            self.SetCameraRotate(playerCacheDataComponent.CameraAngleOffSet);
        }
    }
}
