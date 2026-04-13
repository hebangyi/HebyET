using UnityEngine;

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
                return;
            }
            
            var request = C2B_PlayerUseSkill.Create();
            request.SkillId = skillId;
            
            var response = await ClientBattleNetComponentHelper.Call(request);
            if (response.Error == ErrorCode.ERR_Success)
            {
                UseSkillCacheCD(unitEntity, skillId);
            }
            else
            {
                Log.Info($"Skill Is Error : {response.Error}]");
            }
        }
        
        
        public static bool SkillInCD(ClientUnitEntity unitEntity, long skillId)
        {
            var clientSkillComponent = unitEntity.GetComponent<ClientSkillComponent>();
            if (clientSkillComponent == null)
            {
                return false;
            }

            return clientSkillComponent.CheckSkillInCD(skillId);
        }

        public static void UseSkillCacheCD(ClientUnitEntity unitEntity, long skillId)
        {
            var clientSkillComponent = unitEntity.GetComponent<ClientSkillComponent>();
            if (clientSkillComponent == null)
            {
                return;
            }
            
            clientSkillComponent.AddSkillCD(skillId);
        }
    }    
}
