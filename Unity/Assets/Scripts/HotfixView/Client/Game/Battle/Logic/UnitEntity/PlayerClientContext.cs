using UnityEngine;

namespace ET.Client
{
    [ClientUnitEntityContext(UETypeEnum.Player)]
    public class PlayerClientContext : BaseClientUnitEntityContext
    {
        public override void Init(UnitEntity unitEntity)
        {
            var clientWorld = unitEntity.ClientWorld();
            if (unitEntity.InsId == clientWorld.MainPlayerId)
            {
                // 客户端缓存组件
                var playerCacheDataComponent = unitEntity.AddComponent<MyPlayerCacheDataComponent>();
                
                var unitEntityPosition = unitEntity.GetUnitEntityElemData<UnitEntityPosition>();
                
                playerCacheDataComponent.Position = unitEntityPosition.Position;
                
                var unitEntityPlayerData = unitEntity.GetUnitEntityElemData<UnitEntityCameraData>();
                playerCacheDataComponent.CameraAngleOffSet = unitEntityPlayerData.CameraAngleOffSet; 
                playerCacheDataComponent.TargetCameraAngleOffSet = unitEntityPlayerData.CameraAngleOffSet; 
                
                var unitEntityTowardAngle = unitEntity.GetUnitEntityElemData<UnitEntityTowardAngle>();
                playerCacheDataComponent.TowardAngle = unitEntityTowardAngle.TowardAngle;
                
                clientWorld.MainPlayer = unitEntity;
                var unityScene = UnitySceneManagerComponent.Instance.UnityScene;
                
                // 技能组件
                var playerClientSkillComponent = unitEntity.AddComponent<PlayerClientSkillComponent>();
                playerClientSkillComponent.InitSkillData(unitEntity);
                
                // 修改Scene 世界组件
                var unitySceneCameraComponent = unityScene.GetComponent<UnitySceneCameraComponent>();
                // 设置相机跟随物体
                unitySceneCameraComponent.SetFlowUnitEntity(unitEntity);
                // 设置相机朝向
                unitySceneCameraComponent.InitCameraRotate(playerCacheDataComponent.CameraAngleOffSet);
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
