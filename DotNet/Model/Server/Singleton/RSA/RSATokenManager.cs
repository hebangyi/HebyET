using System.Text;

namespace ET;

using System.Security.Cryptography;

public abstract class RSATokenBean
{
    public long GenerateTime = TimeInfo.Instance.ServerNowSec();
}

[Code]
public class RSATokenManager : Singleton<EtcdManager>, ISingletonAwake
{
    private RSA m_privateRsa;
    private RSA m_publicRsa;

    public void Awake()
    {
        string pubRsaText = GameServerConstant.RSAPublicKey;
        string priRsaText = GameServerConstant.PrivateKey;

        this.m_publicRsa = RSAUtil.LoadRSAPublicKey(pubRsaText);
        this.m_privateRsa = RSAUtil.LoadRSAPrivateKey(priRsaText);
    }

    public byte[] Sign<T>(T bean) where T : RSATokenBean
    {
        var json = JsonHelper.ToJson(bean);
        var data = Encoding.UTF8.GetBytes(json);
        return RSAUtil.SignData(m_privateRsa, data);
    }

    public bool Verify<T>(T bean, byte[] signBytes) where T : RSATokenBean
    {
        if (bean == null || signBytes == null)
        {
            return false;
        }

        if (bean.GenerateTime + GameServerConstant.RSAEffectiveSec < TimeInfo.Instance.ServerNowSec())
        {
            return false;
        }

        var json = JsonHelper.ToJson(bean);
        var data = Encoding.UTF8.GetBytes(json);
        return RSAUtil.VerifyData(m_publicRsa, data, signBytes);
    }
}