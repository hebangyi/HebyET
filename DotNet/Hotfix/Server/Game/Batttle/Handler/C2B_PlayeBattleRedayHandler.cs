namespace ET.Server
{
    [MessageClientHandler(SceneType.Battle)]
    public class C2B_PlayeBattleRedayHandler: MessageClientHandler<BattleRole, C2B_PlayeBattleReday, B2C_PlayeBattleReday>
    {
        protected override void Run(BattleRole battleRole, C2B_PlayeBattleReday request, B2C_PlayeBattleReday response)
        {
            
        }
    }
}

