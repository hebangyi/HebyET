namespace ET.Client
{
    [UnitEntityViewLogic]
    public class UnitEntityTowardAngleClientEleUpdate: IClientEleUpdate
    {
        public ushort WatchComponentId()
        {
            return OpcodeType.Instance.GetOpcode(typeof(UnitEntityTowardAngle));
        }

        public void OnUpdate(UnitEntity unitEntity, IUnitEntityElemData oldData, IUnitEntityElemData newData)
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
            
            var unitEntityTowardAngle = newData as UnitEntityTowardAngle;
            var showAngle = (unitEntityTowardAngle.TowardAngle + playerCacheDataComponent.CameraAngleOffSet) % 360;

            Log.Error($"showAngle : {showAngle}");
            if (showAngle < -90 || showAngle > 90)
            {
                spineAnimation.Skeleton.ScaleX = -1;
            }
            else
            {
                spineAnimation.Skeleton.ScaleX = 1;
            }
        }
    }
}