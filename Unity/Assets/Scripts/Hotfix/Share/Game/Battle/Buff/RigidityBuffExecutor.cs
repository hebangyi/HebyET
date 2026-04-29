namespace ET
{
    [Buff(BuffHandlerType.Rigidity)]
    public class RigidityBuffExecutor: IBuffExecutor
    {
        public void Enter(UnitEntity unitEntity, BuffData buffData)
        {
            Log.Info("僵直进入");
        }

        public void Exit(UnitEntity unitEntity, BuffData buffData)
        {
            Log.Info("僵直退出");
        }
    }
}

