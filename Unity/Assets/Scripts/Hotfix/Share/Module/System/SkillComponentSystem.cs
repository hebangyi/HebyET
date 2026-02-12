using System.Linq;

namespace ET
{
   [FriendOf(typeof(SkillComponent))]
   [EntitySystemOf(typeof(SkillComponent))]
   public static partial class SkillComponentSystem
   {
       
       [EntitySystem]
       private static void Awake(this SkillComponent self)
       {
       }

       public static void UseSkill(this SkillComponent self, long skillId)
       {
           var unitEntity = self.GetParent<UnitEntity>();
           var unitEntitySkillData = unitEntity.GetUnitEntityElemData<UnitEntitySkillData>();
           if (unitEntitySkillData == null)
           {
               Log.Error($"UnitEntity {unitEntity.InsId} 使用技能 {skillId} 找不到技能数据");
               return;
           }


           var unitEntitySkillDataItem = unitEntitySkillData.SkillDataItems.FirstOrDefault(x => x.SkillId == skillId);
           if (unitEntitySkillDataItem == null)
           {
               Log.Error($"UnitEntity {unitEntity.InsId} 使用技能 {skillId} 找不到技能项数据");
               return;
           }

           var skillStatusEnum = unitEntitySkillDataItem.SkillStatusEnum;
           unitEntitySkillDataItem.ActiveFrame = unitEntity.LogicWorld().Frame;
           unitEntitySkillData.Dirty();
           
           AnimationLogicHelper.ChangeUseSkillStatus(unitEntity, skillId, unitEntitySkillDataItem.ActiveFrame);
           SkillHelper.UseSkill(unitEntity, skillId);
       }
   }
}