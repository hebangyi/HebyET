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
            
        }
        
        
        [EntitySystem]
        private static void Destroy(this ET.Client.ClientInputComponent self)
        {
            ClientInputComponent.Instance = null;
        }
    }
}