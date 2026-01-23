namespace ET.Server
{
    [MessageClientHandler(SceneType.Battle)]
    public class C2B_PlayerMoveStopHandler : MessageClientHandler<BattleRole, C2B_PlayerMoveStop, B2C_PlayerMoveStop>
    {
        protected override void Run(BattleRole battlerole, C2B_PlayerMoveStop request, B2C_PlayerMoveStop response)
        {
        }
    }
}