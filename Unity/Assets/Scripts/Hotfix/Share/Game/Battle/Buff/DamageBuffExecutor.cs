namespace ET
{
    [Buff(BuffHandlerType.Damage)]
    public class DamageBuffExecutor: IBuffExecutor
    {
        public void Enter(UnitEntity unitEntity, BuffData buffData)
        {
            var enemy = UnitPlayerHelper.NearestEnemy(unitEntity);
            if (enemy == null)
            {
                return;
            }
            
            BuffLogicHelper.Damage(unitEntity, enemy);
        }
        
        public void Exit(UnitEntity unitEntity, BuffData buffData)
        {
        }
    }
}
