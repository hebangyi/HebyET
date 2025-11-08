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

        // 玩家实际朝向 (摄像机+操作角度)
        public int TowardAngle { get; set; }

        // 玩家状态
        public PlayerAnimateStatusEnum PlayerAnimateStatusEnum { get; set; }
    }
}

