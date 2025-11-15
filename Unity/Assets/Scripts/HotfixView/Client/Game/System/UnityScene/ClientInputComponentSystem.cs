using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(ClientInputComponent))]
    [FriendOf(typeof(ClientInputComponent))]
    public static partial class ClientInputComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.ClientInputComponent self)
        {
            ClientInputComponent.Instance = self;
        }

        [EntitySystem]
        private static void Update(this ET.Client.ClientInputComponent self)
        {
            var operaComponent = UnitySceneManagerComponent.Instance.UnityScene?.GetComponent<OperaComponent>();
            if (operaComponent == null)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                operaComponent.AddCameraAngelOffset(-GameConstant.GameOperaAngle);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                operaComponent.AddCameraAngelOffset(GameConstant.GameOperaAngle);
            }

            if (Input.GetKeyDown(KeyCode.W) && Input.GetKeyDown(KeyCode.D))
            {
                operaComponent.SetOperaMoveAngle(45);
                return;
            }

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
            
            
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
            {
                operaComponent.SetOperaMoveAngle(135);
                self.IsKeyDown = true;
                return;
            }

            if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S))
            {
                operaComponent.SetOperaMoveAngle(-135);
                self.IsKeyDown = true;
                return;
            }

            if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
            {
                operaComponent.SetOperaMoveAngle(-45);
                self.IsKeyDown = true;
                return;
            }

            if (Input.GetKey(KeyCode.W))
            {
                operaComponent.SetOperaMoveAngle(90);
                self.IsKeyDown = true;
                return;
            }

            if (Input.GetKey(KeyCode.A))
            {
                operaComponent.SetOperaMoveAngle(180);
                self.IsKeyDown = true;
                return;
            }

            if (Input.GetKey(KeyCode.S))
            {
                operaComponent.SetOperaMoveAngle(-90);
                self.IsKeyDown = true;
                return;
            }

            if (Input.GetKey(KeyCode.D))
            {
                operaComponent.SetOperaMoveAngle(0);
                self.IsKeyDown = true;
                return;
            }

            
            if(self.IsKeyDown)
            {
                self.IsKeyDown = false;
                operaComponent.SetEndMoving();    
            }
        }

        [EntitySystem]
        private static void Destroy(this ET.Client.ClientInputComponent self)
        {
            ClientInputComponent.Instance = null;
        }
    }
}