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

            return null;
        }
        
        
        public static void OnClickAttack()
        {
            var unitEntity = GetCurrentWorldMainPlayer();
            if (unitEntity == null)
            {
                return;
            }
            
            var playerClientSkillComponent = unitEntity.GetComponent<PlayerClientSkillComponent>();
            if (playerClientSkillComponent == null)
            {
                return;
            }
            
            playerClientSkillComponent.OnClickAttack(unitEntity);
        }
    }    
}

