using Unity.Mathematics;

namespace ET.Client
{
    // 玩家缓存信息
    [ComponentOf(typeof(UnitEntity))]
    public class PlayerCacheDataComponent: Entity, IAwake
    {
        // 位置
        public float2 Position { get; set; }
        
        // 相机偏移角度
        public int CameraAngleOffSet { get; set; }
    }
}

