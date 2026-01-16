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
            var mainPlayer = GetCurrentWorldMainPlayer();
            if (mainPlayer == null)
            {
                return;
            }
            
            var playerClientSkillComponent = mainPlayer.GetComponent<PlayerClientSkillComponent>();
            if (playerClientSkillComponent == null)
            {
                return;
            }
            
            Log.Info("OnClickAttack1");
            playerClientSkillComponent.OnClickAttack();
        }
    }    
}

