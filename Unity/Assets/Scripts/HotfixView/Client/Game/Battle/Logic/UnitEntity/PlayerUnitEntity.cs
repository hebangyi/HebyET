namespace ET.Client
{
    [ClientUnitEntityContext(UETypeEnum.Player)]
    public class PlayerUnitEntityContext : BaseUnitEntityContext
    {
        public override void Init(UnitEntity unitEntity)
        {
            var playerCacheDataComponent = unitEntity.AddComponent<PlayerCacheDataComponent>();
            var clientWorld = unitEntity.ClientWorld();
            if (unitEntity.InsId == clientWorld.MainPlayerId)
            {
                var unitEntityPosition = unitEntity.GetUnitEntityElemData<UnitEntityPosition>();
                playerCacheDataComponent.Position = unitEntityPosition.Position;
                
                var unitEntityPlayerData = unitEntity.GetUnitEntityElemData<UnitEntityCameraData>();
                playerCacheDataComponent.CameraAngleOffSet = unitEntityPlayerData.CameraAngleOffSet; 
                
                var unitEntityPlayerAnimateStatus = unitEntity.GetUnitEntityElemData<UnitEntityPlayerAnimateStatus>();
                playerCacheDataComponent.PlayerAnimateStatusEnum = unitEntityPlayerAnimateStatus.Status;
                
                var unitEntityTowardAngle = unitEntity.GetUnitEntityElemData<UnitEntityTowardAngle>();
                playerCacheDataComponent.TowardAngle = unitEntityTowardAngle.TowardAngle;
                
                clientWorld.MainPlayer = unitEntity;
                var unityScene = UnitySceneManagerComponent.Instance.UnityScene;
                var unitySceneCameraComponent = unityScene.GetComponent<UnitySceneCameraComponent>();
                unitySceneCameraComponent.SetFlowUnitEntity(unitEntity);
            }
            
            var playerInfo = unitEntity.GetUnitEntityElemData<UnitEntityPlayerInfo>();
            clientWorld.PlayerUnitEntities[playerInfo.PlayerId] = unitEntity;
        }

        public override void Destroy(UnitEntity unitEntity)
        {
            var playerInfo = unitEntity.GetUnitEntityElemData<UnitEntityPlayerInfo>();
            var clientWorld = unitEntity.ClientWorld();
            clientWorld.PlayerUnitEntities.Remove(playerInfo.PlayerId);
        }
    }
}
