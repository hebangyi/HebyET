using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    public class BuffManagerComponent : Entity, IAwake
    {
        public static BuffManagerComponent Instance { get; set; }

        public Dictionary<BuffTypeEnum, IBuffExecutor> BuffType2BuffConfigs = new();
        public Dictionary<BuffTypeEnum, IBuffExecutor> Tick2BuffConfigs = new();
    }

    public interface IBuffExecutor
    {
        // 初始化进入
        void Init(UnitEntity unitEntity);
        // 打断
        void Interrupt(UnitEntity unitEntity);
        // 正常退出
        void Exit(UnitEntity unitEntity);
    }

    public interface IBuffExecutorTick : IBuffExecutor
    {
        void Tick(UnitEntity unitEntity, BuffExcuteContext buffExcuteContext);
    }


    public struct BuffExcuteContext
    {
        public long BuffStartTime; // buff执行的时间
    }

    public class BuffAttribute : BaseAttribute
    {
        public BuffTypeEnum BuffTypeEnum;

        public BuffAttribute(BuffTypeEnum buffTypeEnum)
        {
            this.BuffTypeEnum = buffTypeEnum;
        }
    }
}