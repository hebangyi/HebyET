using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    public class BuffManagerComponent : Entity, IAwake
    {
        public static BuffManagerComponent Instance { get; set; }

        public Dictionary<BuffHandlerType, IBuffExecutor> BuffType2BuffConfigs = new();
    }

    public interface IBuffExecutor
    {
        // 初始化进入
        void Enter(UnitEntity unitEntity, BuffData buffData);
        // 正常退出
        void Exit(UnitEntity unitEntity, BuffData buffData);
    }
    
    
    public class BuffAttribute : BaseAttribute
    {
        public BuffHandlerType BuffHandlerType;

        public BuffAttribute(BuffHandlerType buffHandlerType)
        {
            this.BuffHandlerType = buffHandlerType;
        }
    }
}