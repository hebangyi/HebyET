namespace ET.Client
{
    [UnitEntityViewLogic]
    public class UnitEntityPlayerSkillUpdateLogic: BaseClientEleLogic<UnitEntityPlayerSkill>
    {
        public override void OnInit(UnitEntity unitEntity)
        {
        }

        public override void OnDestroy(UnitEntity unitEntity)
        {
        }

        public override void OnUpdateT(UnitEntity unitEntity, UnitEntityPlayerSkill oldData, UnitEntityPlayerSkill newData)
        {
            var playerClientSkillComponent = unitEntity.GetComponent<PlayerClientSkillComponent>();
            Log.Info("技能Dirty");
        }
    }    
}
