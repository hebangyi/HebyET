namespace ET.Client
{
    [UnitEntityViewLogic]
    public class UnitEntityTowardAngleClientEleLogic: BaseClientEleLogic<UnitEntityTowardAngle>
    {
        public override void OnInitT(ClientUnitEntity unitEntity, UnitEntityTowardAngle elemData)
        {
        }

        public override void OnDestroyT(ClientUnitEntity unitEntity, UnitEntityTowardAngle elemData)
        {
        }

        public override void OnUpdateT(ClientUnitEntity unitEntity, UnitEntityTowardAngle oldData, UnitEntityTowardAngle newData)
        {
            var clientWorld = unitEntity.ClientWorld();
            var playerCacheDataComponent = clientWorld.MainPlayer.GetComponent<MyPlayerCacheDataComponent>();
            if (playerCacheDataComponent == null)
            {
                return;
            }
            
            var spineAnimation = unitEntity.GetSpineAnimation();
            if (spineAnimation == null)
            {
                return;
            }
            
            AnimationHelper.ReCalUnitEntityAnimationSkeleton(unitEntity);
        }
    }
}