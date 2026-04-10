namespace ET.Client
{
    public static class ClientSkillHelper
    {
        public static async ETTask CheckAndSendSkill(ClientUnitEntity unitEntity, long skillId)
        {
            var mainPlayer = MainPlayerHelper.GetCurrentWorldMainPlayer();
            if (mainPlayer == null)
            {
                return;
            }

            if (SkillInCD(mainPlayer, skillId))
            {
                Log.Info($"技能[{skillId}] 在CD中");
                return;
            }
            
            var request = C2B_PlayerUseSkill.Create();
            request.SkillId = skillId;
            var response = await ClientBattleSenderComponent.Instance.Call(request);

            if (response.Error == ErrorCode.ERR_Success)
            {
                UseSkillCacheCD(unitEntity, skillId);
            }
        }
        
        
        public static bool SkillInCD(ClientUnitEntity unitEntity, long skillId)
        {
            var clientSkillComponent = unitEntity.GetComponent<ClientSkillComponent>();
            if (clientSkillComponent == null)
            {
                return false;
            }

            return clientSkillComponent.SkillInCD(skillId);
        }

        public static void UseSkillCacheCD(ClientUnitEntity unitEntity, long skillId)
        {
            var clientSkillComponent = unitEntity.GetComponent<ClientSkillComponent>();
            if (clientSkillComponent == null)
            {
                return;
            }
            
            clientSkillComponent.UseSkill(skillId);
        }
    }    
}
