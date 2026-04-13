using System.Collections.Generic;
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

       public static int UseSkill(this SkillComponent self, long skillId)
       {
           var unitEntity = self.GetParent<UnitEntity>();
           var unitEntitySkillData = unitEntity.GetUnitEntityElemData<UnitEntitySkillData>();
           if (unitEntitySkillData == null)
           {
               Log.Error($"UnitEntity {unitEntity.InsId} 使用技能 {skillId} 找不到技能数据");
               return ErrorCode.ServerInternalErr;
           }

           var skillDataItem = unitEntitySkillData.SkillId2UnitEntitySkillDataItems.GetValueOrDefault(skillId);
           if (skillDataItem == null)
           {
               Log.Error($"UnitEntity {unitEntity.InsId} 使用技能 {skillId} 找不到技能项数据");
               return ErrorCode.ServerInternalErr;
           }

           var skillConfig = SkillConfigCategory.Instance.GetById(skillId);
           if (skillConfig == null)
           {
               Log.Error($"技能使用异常 找不到配置ID {skillId}");
               return ErrorCode.ServerInternalErr;
           }

           if (skillDataItem.CDFrame > unitEntity.LogicWorld().Frame)
           {
               return ErrorCode.SkillInCD;
           }

           var skillStatusEnum = skillDataItem.SkillStatusEnum;
           skillDataItem.ActiveFrame = unitEntity.LogicWorld().Frame;
           skillDataItem.CDFrame = skillDataItem.ActiveFrame + FrameHelper.CalFrameNum(skillConfig.CDTime);
           
           // 技能信息更新
           unitEntitySkillData.Dirty();
           SkillHelper.UseSkill(unitEntity, skillConfig);
           return ErrorCode.ERR_Success;
       }
   }
}