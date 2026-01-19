using System.Linq;

namespace ET
{
   [FriendOf(typeof(PlayerServerSkillComponent))]
   [EntitySystemOf(typeof(PlayerServerSkillComponent))]
   public static partial class PlayerServerSkillComponentSystem
   {
       
       [EntitySystem]
       private static void Awake(this PlayerServerSkillComponent self)
       {
       }

       
       
       public static void InitSkillData(this PlayerServerSkillComponent self)
       {
           // 清理数据
           UnitEntity unitEntity = self.GetParent<UnitEntity>();
           self.NormalAttackSkill = null;
           var unitEntityPlayerSkill = unitEntity.GetUnitEntityElemData<UnitEntityPlayerSkill>();
           foreach (var skillDataItem in unitEntityPlayerSkill.SkillDataItems)
           {
               Log.Info($"Skill Id : {skillDataItem.SkillId}");
               var skillConfig = SkillConfigCategory.Instance.GetById(skillDataItem.SkillId);
               if (skillConfig == null)
               {
                   Log.Error($"初始化角色技能失败 找不到技能ID : {skillDataItem.SkillId}");
                   continue;
               }
               
               if (skillConfig.PlayerSkillTag == PlayerSkillTagEnum.NormalAttack)
               {
                   self.NormalAttackSkill = skillDataItem;
               }
           }
       }
       
       // 使用技能
       public static void PlayerUseSkill(this PlayerServerSkillComponent self, long skillId)
       {
           // TODO 判断Skill执行器执行
           UnitEntity unitEntity = self.GetParent<UnitEntity>();
           unitEntity.LogicWorld();
           
           var unitEntityPlayerSkill = unitEntity.GetUnitEntityElemData<UnitEntityPlayerSkill>();
           
           var skillDataItems = unitEntityPlayerSkill.SkillDataItems;
           var skillDataItem = skillDataItems.FirstOrDefault(x => x.SkillId == skillId);
           // 激活状态
           skillDataItem.SkillStatusEnum = SkillStatusEnum.Active;
           skillDataItem.ActiveFrame = unitEntity.LogicWorld().Frame;
           
           unitEntityPlayerSkill.Dirty();
       }
   }
}