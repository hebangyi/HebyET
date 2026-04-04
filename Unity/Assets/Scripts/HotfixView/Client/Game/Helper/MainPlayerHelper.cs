using UnityEngine;

namespace ET.Client
{
    public static class MainPlayerHelper
    {
        public static ClientUnitEntity GetCurrentWorldMainPlayer()
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
        }

        // 辅助判断：是否朝向边界移动
        public static bool IsMovingTowardsBoundary(ClientUnitEntity unitEntity, Collider2D boundary, Vector2 moveVelocity)
        {
            var gameObject = unitEntity.GetGameObject();
            // 计算物体中心到边界中心的方向
            Vector2 dirToBoundary = (boundary.bounds.center - gameObject.transform.position).normalized;
            // 点乘：速度方向与边界方向同向 → 朝向边界
            return Vector2.Dot(moveVelocity.normalized, dirToBoundary) > 0.1f;
        }
        
    }
}