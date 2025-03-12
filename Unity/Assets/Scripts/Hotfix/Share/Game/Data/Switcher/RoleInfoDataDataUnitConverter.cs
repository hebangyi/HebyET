namespace ET
{
    [DataUnitConverter]
    public class RoleInfoUnitDataConverter : UnitDataConverter<RoleInfoServerData, RoleInfoClientData, RoleInfoUnitData>
    {
        public override RoleInfoUnitData ToUnitData(RoleInfoServerData data)
        {
            RoleInfoUnitData unitDataBean = RoleInfoUnitData.Create();
            unitDataBean.NickName = data.NickName;
            return unitDataBean;
        }

        public override RoleInfoClientData FromUnitData(RoleInfoUnitData unitData)
        {
            RoleInfoClientData roleInfoClientData = new ();
            roleInfoClientData.NickName = unitData.NickName;
            return roleInfoClientData;
        }
    }
}