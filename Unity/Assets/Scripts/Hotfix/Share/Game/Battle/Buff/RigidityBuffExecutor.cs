namespace ET
{
    [Buff(BuffTypeEnum.Rigidity)]
    public class RigidityBuffExecutor: IBuffExecutorTick
    {
        public void Enter(UnitEntity unitEntity, BuffData buffData)
        {
            var buffConfig = buffData.BuffConfig;
            var paramConfig = buffConfig.RigidityBuffParamConfig;

            buffData.BuffEndFrame = buffData.BuffStartFrame + FrameHelper.CalFrameNum(paramConfig.DurationTime); 
        }

        public void Exit(UnitEntity unitEntity, BuffData buffData)
        {
        }

        public void Interrupt(UnitEntity unitEntity, BuffData buffData)
        {
        }

        public void Tick(UnitEntity unitEntity, BuffData buffData)
        {
        }
    }
}

