namespace ET.Server
{
    [MessageClientHandler(SceneType.Battle)]
    public class C2B_PlayerUseSkillHandler : MessageClientHandler<BattleRole, C2B_PlayerUseSkill, B2C_PlayerUseSkill>
    {
        protected override void Run(BattleRole battleRole, C2B_PlayerUseSkill request, B2C_PlayerUseSkill response)
        {
            var skillId = request.SkillId;
            var unitEntityPlayer = battleRole.WorldPlayer();
            if (unitEntityPlayer == null)
            {
                response.Error = ErrorCode.NotFoundWorldPlayer;
                return;
            }

            var skillComponent = unitEntityPlayer.GetComponent<SkillComponent>();
            if (skillComponent == null)
            {
                response.Error = ErrorCode.NotFoundWorldPlayer;
                return;
            }
            
            response.Error = skillComponent.UseSkill(skillId);
        }
    }
}