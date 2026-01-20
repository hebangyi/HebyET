namespace ET.Client
{
    [UnitEntityViewLogic]
    public class UnitEntityTowardAngleClientEleLogic: BaseClientEleLogic<UnitEntityTowardAngle>
    {
        public override void OnInitT(UnitEntity unitEntity, UnitEntityTowardAngle elemData)
        {
        }

        public override void OnDestroyT(UnitEntity unitEntity, UnitEntityTowardAngle elemData)
        {
        }

        public override void OnUpdateT(UnitEntity unitEntity, UnitEntityTowardAngle oldData, UnitEntityTowardAngle newData)
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