using UnityEngine;

namespace ET.Client
{
    [BattleEvent(WorldMode.View)]
    public class CreateUnitEntityEvent1_CreateGameObject: ABattleEvent<CreateUnitEntityEvent1>
    {
        protected override void Run(World world, CreateUnitEntityEvent1 args)
        {
            var unitEntity = args.UnitEntity;

            var unitEntityCommonData = unitEntity.GetUnitEntityElemData<UnitEntityCommonData>();

            if (unitEntityCommonData == null)
            {
                Log.Error("创建 GameObject 错误, 找不到 UnitEntity UnitEntityCommonData");
                return;
            }
            
            unitEntityCommonData.UnitEntityType = UnitEntityTypeEnum.Player;
            switch (unitEntityCommonData.UnitEntityType)
            {
                case UnitEntityTypeEnum.Player:
                {
                    CreatePlayer(unitEntity).Coroutine();
                    break;
                }
            }
        }
        
        public async ETTask CreatePlayer(UnitEntity unitEntity)
        {
            string assetsName = $"Assets/Bundles/Unit/Unit.prefab";
            GameObject bundleGameObject = await unitEntity.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(assetsName);
            GameObject playerGameObject = bundleGameObject.Get<GameObject>("Skeleton");

            GameObject go = UnityEngine.Object.Instantiate(playerGameObject, GlobalComponent.Instance.Unit, true);
            var unitEntityPosition = unitEntity.GetUnitEntityElemData<UnitEntityPosition>();

            var unitEntityGameObjectComponent = unitEntity.TryAddComponent<UnitEntityGameObjectComponent>();
            unitEntityGameObjectComponent.GameObject = go;
            go.name = $"Player_{unitEntity.InsId}";
            go.transform.position = new Vector3(unitEntityPosition.Position.x, unitEntityPosition.Position.y, unitEntityPosition.Position.z);
        }
    }    
}
