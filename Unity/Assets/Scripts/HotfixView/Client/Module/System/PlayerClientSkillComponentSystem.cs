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

       public static void InitSkillData(UnitEntity unitEntity)
       {
           var unitEntityPlayerSkill = unitEntity.GetUnitEntityElemData<UnitEntityPlayerSkill>();
           var skillId = unitEntityPlayerSkill.SkillId;
       }
   }
}