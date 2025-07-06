using Unity.Mathematics;
using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(UnityScene))]
    public class UnitySceneCameraComponent : Entity, IAwake, ILateUpdate
    {
        // 战斗主相机
        public Camera MainCamera;
        // 偏移地址
        public float3 OffsetPosition;
        // 跟踪对象
        public EntityRef<UnitEntity> FlowUnitEntity;
    }
}
