namespace ET
{
    [Buff(BuffTypeEnum.Damage)]
    public class DamageBuffExecutor: IBuffExecutor
    {
        public void Enter(UnitEntity unitEntity, BuffData buffData)
        {
            Log.Info($"{buffData.BuffConfig.Id} DamageBuff Enter...");
        }
        
        public void Exit(UnitEntity unitEntity, BuffData buffData)
        {
            Log.Info($"{buffData.BuffConfig.Id} DamageBuff Exit...");
        }
    }
}
