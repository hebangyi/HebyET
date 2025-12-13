using NativeCollection.UnsafeType;
using Unity.Mathematics;

namespace ET
{
    public class MonsterRuntimeData: IUnitEntityLogicElemData
    {
        public float2 BornPosition;
        
        // 巡逻数据
        public PatrolData PatrolData = new PatrolData();
    }
    

    // 巡逻的数据
    public class PatrolData
    {
        // 巡逻等待
        public PatrolStatus PatrolStatus;
        // 巡逻的目标点
        public float2 ToPosition { get; set; }
        // Id完成时间
        public long IdleFinishTime { get; set; }
    }

    public enum PatrolStatus
    {
        Idle,
        Run,
    }
}