using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(OperaComponent))]
    [FriendOf(typeof(OperaComponent))]
    public static partial class OperaComponentSystem
    {
        [EntitySystem]
        private static void Awake(this OperaComponent self)
        {
        }

        [EntitySystem]
        private static void Update(this OperaComponent self)
        {
            if (Input.GetMouseButtonDown(1))
            {
                /*if (Physics.Raycast(ray, out hit, 1000, self.mapMask))
                {
                    // C2M_PathfindingResult c2MPathfindingResult = C2M_PathfindingResult.Create();
                    // c2MPathfindingResult.Position = hit.point;
                    // self.Root().GetComponent<ClientSenderComponent>().Send(c2MPathfindingResult);
                }*/
            }
        }

        /// <summary>
        /// 设置操作的角度
        /// </summary>
        /// <param name="self"></param>
        /// <param name="angle"></param>
        public static void SetOperaMoveAngle(this OperaComponent self, int angle)
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

            playerCacheDataComponent.IsDragging = true;
            playerCacheDataComponent.OperaAngel = angle;
        }

        public static void SetEndMoving(this OperaComponent self)
        {
            var clientWorld = UnitySceneClientWorldManagerComponent.Instance.CurrentClientWorld;
            if (clientWorld == null)
            {
                return;
            }

            var mainPlayer = clientWorld.MainPlayer;
            if (mainPlayer == null)
            {
                return;
            }

            var playerCacheDataComponent = mainPlayer.GetComponent<MyPlayerCacheDataComponent>();
            if (playerCacheDataComponent == null)
            {
                return;
            }

            playerCacheDataComponent.IsDragging = false;
        }

        public static void AddCameraAngelOffset(this OperaComponent self, int angle)
        {
            var unityScene = self.GetParent<UnityScene>();
            if (unityScene != null)
            {
                // 相机移动
                var unitySceneCameraComponent = unityScene.GetComponent<UnitySceneCameraComponent>();
                if (unitySceneCameraComponent != null)
                {
                    unitySceneCameraComponent.AddTargetCameraRotate(angle);
                }
            }
        }
    }
}