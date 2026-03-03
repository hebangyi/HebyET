namespace ET.Client
{
    public static class MainPlayerHelper
    {
        public static UnitEntity GetCurrentWorldMainPlayer()
        {
            var clientWorld = UnitySceneClientWorldManagerComponent.Instance.CurrentClientWorld;
            if (clientWorld == null)
            {
                return null;
            }
            
            var mainPlayer = clientWorld.MainPlayer;
            if (mainPlayer == null)
            {
                return null;
            }

            return mainPlayer;
        }
        
        
        public static void OnClickAttack()
        {
            Log.Info("OnClickAttack");
            SendUseAttackSkill().Coroutine();
            // playerClientSkillComponent.OnClickAttack();
        }


        public static async ETTask SendUseAttackSkill()
        {
            var mainPlayer = GetCurrentWorldMainPlayer();
            if (mainPlayer == null)
            {
                return;
            }
            
            var unitEntityCommonData = mainPlayer.GetUnitEntityElemData<UnitEntityCommonData>();
            BattlePlayerConfig playerConfig = BattlePlayerConfigCategory.Instance.GetById(unitEntityCommonData.ConfigId);

            var request = C2B_PlayerUseSkill.Create();
            request.SkillId = playerConfig.AttackSkill;
            var response = await ClientBattleSenderComponent.Instance.Call(request);
            
            var req = C2B_PlayerGetAllAOIWorldData.Create();
            await ClientBattleSenderComponent.Instance.Call(req);
        }
    }    
}

