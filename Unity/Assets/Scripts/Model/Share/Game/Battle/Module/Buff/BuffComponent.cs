namespace ET
{
    
    /***
     * Buff 具体的状态变化
     * 变强
     * 变弱
     * 加速 / 减速
     * 眩晕、沉默、击飞
     * 护盾、回血、免伤
     */
    [ComponentOf(typeof(UnitEntity))]
    public class BuffComponent: Entity, IAwake
    {
    }
}