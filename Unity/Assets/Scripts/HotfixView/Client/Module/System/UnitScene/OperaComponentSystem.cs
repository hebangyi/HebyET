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
            if (self.targetAngel != self.lastAngel)
            {
                C2B_PlayerMoveOperationMessage message = C2B_PlayerMoveOperationMessage.Create();
                message.MoveAngle = self.targetAngel;
                ClientBattleSenderComponent.Instance.Send(message);
                
                self.lastAngel = self.targetAngel;
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

        public static void SetOperaMoveAngle(this OperaComponent self, int angle)
        {
            self.targetAngel = angle;
        }
    }
}