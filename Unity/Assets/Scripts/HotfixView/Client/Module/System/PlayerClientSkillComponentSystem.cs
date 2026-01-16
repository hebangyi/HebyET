namespace ET.Client
{
   [FriendOf(typeof(PlayerClientSkillComponent))]
   [EntitySystemOf(typeof(PlayerClientSkillComponent))]
   public static partial class PlayerClientSkillComponentSystem
   {
       
       [EntitySystem]
       private static void Awake(this PlayerClientSkillComponent self)
       {
           
       }

       public static void InitSkillData(this PlayerClientSkillComponent self, UnitEntity unitEntity)
       {
           // 清理数据
           self.NormalAttackSkill = null;
           var unitEntityPlayerSkill = unitEntity.GetUnitEntityElemData<UnitEntityPlayerSkill>();
           foreach (var skillDataItem in unitEntityPlayerSkill.SkillDataItems)
           {
               Log.Info($"Skill Id : {skillDataItem.SkillId}");
               var skillConfig = SkillConfigCategory.Instance.GetById(skillDataItem.SkillId);
               if (skillConfig.PlayerSkillTag == PlayerSkillTagEnum.NormalAttack)
               {
                   self.NormalAttackSkill = skillDataItem;
               }
           }
       }

       public static void OnClickAttack(this PlayerClientSkillComponent self)
       {
           if (self.NormalAttackSkill == null)
           {
               return;
           }

           Log.Info($"发送普通技能 {self.NormalAttackSkill.SkillId}");
           C2B_PlayerUseSkill playerUseSkill = C2B_PlayerUseSkill.Create();
           playerUseSkill.SkillId = self.NormalAttackSkill.SkillId;
           ClientBattleSenderComponent.Instance.Send(playerUseSkill);
       }
   }
}