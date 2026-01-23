namespace ET.Server
{
    [MessageClientHandler(SceneType.Battle)]
    public class C2B_PlayerMoveHandler : MessageClientHandler<BattleRole, C2B_PlayerMove, B2C_PlayerMove>
    {
        protected override void Run(BattleRole battlerole, C2B_PlayerMove request, B2C_PlayerMove response)
        {
            
        }
    }
}