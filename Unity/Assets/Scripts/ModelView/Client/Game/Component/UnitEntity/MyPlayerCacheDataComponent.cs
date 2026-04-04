using UnityEngine;

namespace ET.Client
{
    // 玩家缓存信息
    [ComponentOf(typeof(UnitEntity))]
    public class MyPlayerCacheDataComponent: Entity, IAwake
    {
        // 位置
        public Unity.Mathematics.float2 Position { get; set; }
        
        // 操作角度
        public int OperaAngel { get; set; }
        
        // 相机目标偏移角度
        public int TargetCameraAngleOffSet { get; set; }
        
        // 相机偏移角度
        public int CameraAngleOffSet { get; set; }

        // 玩家实际朝向 (摄像机+操作角度)
        public int TowardAngle { get; set; }

        // 是否拖拽正在移动
        public bool IsDragging { get; set; }

        // 是否正在移动
        public bool IsMoving { get; set; }

        // 上次更新时间
        public long LastUpdateTime { get; set; }
        
        // 玩家状态
        // public AnimateStatusEnum animateStatusEnum { get; set; }
    }
}

