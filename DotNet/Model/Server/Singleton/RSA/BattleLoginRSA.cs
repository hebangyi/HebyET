namespace ET;

public class BattleLoginRSA: RSATokenBean
{
    public long RoleId;
    public long WorldId;

    public override long RSAEffectiveSec()
    {
        return GameServerConstant.BattleRSAEffectiveSec;
    }
}