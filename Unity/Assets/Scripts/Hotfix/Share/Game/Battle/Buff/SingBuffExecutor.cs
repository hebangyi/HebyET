namespace ET
{
    [Buff(BuffTypeEnum.Sing)]
    public class SingBuffExecutor : IBuffExecutorTick
    {
        public void Enter(UnitEntity unitEntity, BuffData buffData)
        {
            Log.Info($"{buffData.BuffConfig.Id} SingBuff Init...");
        }

        public void Interrupt(UnitEntity unitEntity, BuffData buffData)
        {
            Log.Info($"{buffData.BuffConfig.Id} SingBuff Interrupt...");
        }

        public void Tick(UnitEntity unitEntity, BuffData buffData)
        {
            Log.Info($"{buffData.BuffConfig.Id} SingBuff Tick ...");
        }

        public void Exit(UnitEntity unitEntity, BuffData buffData)
        {
            Log.Info($"{buffData.BuffConfig.Id} SingBuff Exit");
        }
    }
}