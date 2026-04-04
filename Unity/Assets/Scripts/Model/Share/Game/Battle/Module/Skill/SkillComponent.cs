namespace ET
{
    /**
     * 技能:
     * 释放目标
     * 能否打断
     * Buff组
     */
    [ComponentOf(typeof(UnitEntity))]
    public class SkillComponent : Entity, IAwake
    {
    }

    /***
     * Buff 具体的状态变化
     * 变强
     * 变弱
     * 加速 / 减速
     * 眩晕、沉默、击飞
     * 护盾、回血、免伤
     */
    
}