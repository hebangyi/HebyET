namespace ET.Server
{
    [MessageClientHandler(SceneType.Battle)]
    public class C2B_PlayerMoveStopHandler : MessageClientHandler<BattleRole, C2B_PlayerMoveStop, B2C_PlayerMoveStop>
    {
        protected override void Run(BattleRole battleRole, C2B_PlayerMoveStop request, B2C_PlayerMoveStop response)
        {
            var unitEntityPlayer = battleRole.WorldPlayer();
            if (unitEntityPlayer == null)
            {
                response.Error = ErrorCode.NotFoundWorldPlayer;
                return;
            }

            unitEntityPlayer.ChangeAnimateStatus(AnimateStateEnum.Idle);
        }
    }
}