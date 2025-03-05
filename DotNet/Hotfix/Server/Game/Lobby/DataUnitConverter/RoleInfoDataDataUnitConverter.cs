namespace ET.Server;

[DataUnitConverter]
public class RoleInfoDataDataUnitConverter : DataUnitConverter<RoleInfoData, RoleInfoUnitData>
{
    public override RoleInfoUnitData Convert(RoleInfoData data)
    {
        RoleInfoUnitData unitDataBean = RoleInfoUnitData.Create();
        unitDataBean.NickName = data.NickName;
        return unitDataBean;
    }
}