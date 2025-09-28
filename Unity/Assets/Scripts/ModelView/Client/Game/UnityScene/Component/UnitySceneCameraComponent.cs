using Unity.Mathematics;
using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(UnityScene))]
    public class UnitySceneCameraComponent : Entity, IAwake, ILateUpdate
    {
        // 外部相机包装类
        public GameObject CameraPack;
        // 战斗主相机
        public Camera MainCamera;
        // 偏移地址
        public float3 OffsetPosition;
        // 跟踪对象
        public EntityRef<UnitEntity> FlowUnitEntity;
        // 上一次更新的时间
        public long LastUpdateTime;

    }
}
