namespace ET.Client
{
    public static class AnimationHelper
    {
        public static void ReCalUnitEntityAnimationSkeleton(ClientUnitEntity unitEntity)
        { 
            var spineAnimation = unitEntity.GetSpineAnimation();
            if (spineAnimation == null)
            {
                return;
            }

            var clientWorld = unitEntity.ClientWorld();
            var playerCacheDataComponent = clientWorld.MainPlayer.GetComponent<MyPlayerCacheDataComponent>();
            var towardAngle = unitEntity.GetUnitEntityElemData<UnitEntityTowardAngle>();

            var toward = CalUnitEntityAnimationToward((int)playerCacheDataComponent.CameraAngleOffSet, towardAngle.TowardAngle);


            if (toward == UnitEntityAnimationToward.Left)
            {
                spineAnimation.Skeleton.ScaleX = -1;
            }
            else
            {
                spineAnimation.Skeleton.ScaleX = 1;
            }
        }
        
        
        public static UnitEntityAnimationToward CalUnitEntityAnimationToward(int cameraAngle, int regionTowardAngle)
        {
            int showAngle = regionTowardAngle - cameraAngle;
            showAngle %= 360;

            if (showAngle < 0)
            {
                showAngle += 360;
            }

            if (showAngle is >= 90 and < 270)
            {
                return UnitEntityAnimationToward.Left;
            }
            
            return UnitEntityAnimationToward.Right;
        }
    }
}
