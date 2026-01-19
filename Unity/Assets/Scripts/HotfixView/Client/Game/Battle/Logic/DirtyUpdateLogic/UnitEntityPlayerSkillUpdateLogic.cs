namespace ET.Client
{
    [UnitEntityViewLogic]
    public class UnitEntityPlayerSkillUpdateLogic: IClientEleUpdate
    {
        public ushort WatchComponentId()
        {
            return OpcodeType.Instance.GetOpcode(typeof(UnitEntityPlayerSkill));
        }

        public void OnUpdate(UnitEntity unitEntity, IUnitEntityElemData oldData, IUnitEntityElemData newData)
        {
            var playerClientSkillComponent = unitEntity.GetComponent<PlayerClientSkillComponent>();
            Log.Info("技能Dirty");
        }
    }    
}
