namespace ET
{
    [Buff(BuffTypeEnum.Damage)]
    public class DamageBuffExecutor: IBuffExecutor
    {
        public void Enter(UnitEntity unitEntity, BuffData buffData)
        {
            Log.Info($"{buffData.BuffConfig.Id} DamageBuff Enter...");
            var enemy = UnitPlayerHelper.NearestEnemy(unitEntity);
            if (enemy == null)
            {
                Log.Info("释放技能 没有找到敌人");
                return;
            }
            
            BuffLogicHelper.Damage(unitEntity, enemy);
        }
        
        public void Exit(UnitEntity unitEntity, BuffData buffData)
        {
            Log.Info($"{buffData.BuffConfig.Id} DamageBuff Exit...");
        }
    }
}
