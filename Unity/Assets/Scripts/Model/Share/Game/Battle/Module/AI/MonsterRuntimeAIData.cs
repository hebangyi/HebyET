using NativeCollection.UnsafeType;
using Unity.Mathematics;

namespace ET
{
    public class MonsterRuntimeAIData: IUnitEntityLogicElemData
    {
        public MonsterAIStatus AIStatus;

        // 巡逻数据
        public PatrolData PatrolData = new PatrolData();
    }

    public enum MonsterAIStatus
    {
        None = 0,
        Patrol = 1,
    }

    // 巡逻的数据
    public class PatrolData
    {
        // 巡逻状态
        public PatrolStatus patrolStatus;
        // 巡逻的目标点
        public float2 ToPosition { get; set; }
        // Id完成时间
        public long IdleFinishTime { get; set; }
    }

    public enum PatrolStatus
    {
        Idle = 0,
        Run = 1,
    }
}