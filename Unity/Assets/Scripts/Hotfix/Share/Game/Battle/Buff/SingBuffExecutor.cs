namespace ET
{
    [Buff(BuffTypeEnum.Sing)]
    public class SingBuffExecutor : IBuffExecutorTick
    {
        public void Init(UnitEntity unitEntity)
        {
            Log.Info("SingBuff Init...");
        }

        public void Interrupt(UnitEntity unitEntity)
        {
        }

        public void Tick(UnitEntity unitEntity, BuffExcuteContext buffExcuteContext)
        {
            Log.Info("SingBuff ExecuteUpdate ...");
        }

        public void Exit(UnitEntity unitEntity)
        {
            Log.Info("SingBuff Exit");
        }
    }
}