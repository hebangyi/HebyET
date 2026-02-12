using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    public class BuffManagerComponent : Entity, IAwake
    {
        public static BuffManagerComponent Instance { get; set; }

        public Dictionary<BuffTypeEnum, IBuffExecutor> BuffType2BuffConfigs = new();
        public Dictionary<BuffTypeEnum, IBuffExecutorTick> Tick2BuffConfigs = new();
    }

    public interface IBuffExecutor
    {
        // 初始化进入
        void Enter(UnitEntity unitEntity, BuffData buffData);
        // 正常退出
        void Exit(UnitEntity unitEntity, BuffData buffData);
    }

    public interface IBuffExecutorTick : IBuffExecutor
    {
        // 打断
        void Interrupt(UnitEntity unitEntity, BuffData buffData);
        // tick
        void Tick(UnitEntity unitEntity, BuffData buffData);
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