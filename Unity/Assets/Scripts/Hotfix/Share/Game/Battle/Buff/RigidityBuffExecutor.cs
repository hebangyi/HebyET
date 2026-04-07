namespace ET
{
    [Buff(BuffTypeEnum.Rigidity)]
    public class RigidityBuffExecutor: IBuffExecutorTick
    {
        public void Enter(UnitEntity unitEntity, BuffData buffData)
        {
            
            unitEntity.ChangeAnimateStatus(AnimateStateEnum.Idle);
            
            var buffConfig = buffData.BuffConfig;
            var paramConfig = buffConfig.RigidityBuffParamConfig;

            buffData.BuffEndFrame = buffData.BuffStartFrame + FrameHelper.CalFrameNum(paramConfig.DurationTime); 
            Log.Info("僵直进入");
        }

        public void Exit(UnitEntity unitEntity, BuffData buffData)
        {
            Log.Info("僵直退出");
        }

        public void Interrupt(UnitEntity unitEntity, BuffData buffData)
        {
        }

        public void Tick(UnitEntity unitEntity, BuffData buffData)
        {
        }
    }
}

