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
            // TODO  控制发送频率
            if (self.operaAngel != self.lastAngel)
            {
                C2B_PlayerMoveOperationMessage message = C2B_PlayerMoveOperationMessage.Create();
                int moveAngle =  self.operaAngel != -1000 ? self.operaAngel - self.CameraAngelOffset : self.operaAngel;
                
                message.MoveAngle = moveAngle;
                ClientBattleSenderComponent.Instance.Send(message);
                self.lastAngel = self.operaAngel;
            }
            
            
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
            self.operaAngel = angle;
        }


        public static void AddCameraAngelOffset(this OperaComponent self, int angle)
        {
            self.CameraAngelOffset += angle;
            self.CameraAngelOffset %= 360;

            var unityScene = self.GetParent<UnityScene>();
            if (unityScene != null)
            {
                // 相机移动
                var unitySceneCameraComponent = unityScene.GetComponent<UnitySceneCameraComponent>();
                if (unitySceneCameraComponent != null)
                {
                    unitySceneCameraComponent.SetCameraRotate(self.CameraAngelOffset);
                }
            }

            var clientWorld = ClientWorldManagerComponent.Instance.CurrentClientWorld;
            if (clientWorld != null)
            {
                foreach (var unitEntity in clientWorld.EvnUnitEntities.Values)
                {
                    var unitEntityGameObjectComponent = unitEntity.GetComponent<UnitEntityGameObjectComponent>();
                    var gameObject = unitEntityGameObjectComponent.GameObject;
                    
                    gameObject.transform.rotation = Quaternion.Euler(GameConstant.GameOperaAngle, self.CameraAngelOffset, 0);
                }
            }
        }
    }
}