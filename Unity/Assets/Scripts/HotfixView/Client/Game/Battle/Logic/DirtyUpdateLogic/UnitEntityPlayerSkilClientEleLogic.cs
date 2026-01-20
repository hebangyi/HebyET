namespace ET.Client
{
    [UnitEntityViewLogic]
    public class UnitEntityPlayerSkilClientEleLogic: BaseClientEleLogic<UnitEntityPlayerSkill>
    {
        public override void OnInitT(UnitEntity unitEntity, UnitEntityPlayerSkill elemData)
        {
        }

        public override void OnDestroyT(UnitEntity unitEntity, UnitEntityPlayerSkill elemData)
        {
        }

        public override void OnUpdateT(UnitEntity unitEntity, UnitEntityPlayerSkill oldData, UnitEntityPlayerSkill newData)
        {
            var playerClientSkillComponent = unitEntity.GetComponent<PlayerClientSkillComponent>();
            Log.Info("技能Dirty");
        }
    }    
}
