using System.Collections.Generic;

namespace ET
{
   [FriendOf(typeof(WorldSkillExecuteComponent))]
   [EntitySystemOf(typeof(WorldSkillExecuteComponent))]
   public static partial class WorldSkillExecuteComponentSystem
   {
       
       [EntitySystem]
       private static void Awake(this WorldSkillExecuteComponent self)
       {
       }

       // 使用技能
       public static void UseSkill(this WorldSkillExecuteComponent self, UnitEntity unitEntity, long skillId)
       {
           var unitEntitySkill = self.GetUnitEntitySkill(unitEntity, skillId);
           UnitEntitySkillItem unitEntitySkillItem = new UnitEntitySkillItem();
           unitEntitySkillItem.SkillId = skillId;
           unitEntitySkillItem.StartTime = unitEntity.LogicWorld().NowMilliTime;
           unitEntitySkill.UnitEntitySkillItems.Add(unitEntitySkillItem);
       }
       
       private static UnitEntitySkill GetUnitEntitySkill(this WorldSkillExecuteComponent self, UnitEntity unitEntity, long skillId)
       {
           var insId = unitEntity.InsId;
           var unitEntitySkill = self.InsId2UnitEntitySkills.GetValueOrDefault(insId);
           if (unitEntitySkill == null)
           {
               unitEntitySkill = new UnitEntitySkill();
               unitEntitySkill.InsId = insId;
               self.InsId2UnitEntitySkills[insId] = unitEntitySkill;
           }

           return unitEntitySkill;
       }
   }
}