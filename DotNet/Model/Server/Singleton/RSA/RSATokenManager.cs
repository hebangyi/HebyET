using System;
using System.Text;
using Microsoft.Extensions.Primitives;
using MongoDB.Bson;

namespace ET;

using System.Security.Cryptography;

public abstract class RSATokenBean
{
    public long GenerateTime = TimeInfo.Instance.ServerNowSec();
}

[Code]
public class RSATokenManager : Singleton<RSATokenManager>, ISingletonAwake
{
    public const string SplitChart = "@";
    private RSA m_privateRsa;
    private RSA m_publicRsa;

    public void Awake()
    {
        string pubRsaText = GameServerConstant.RSAPublicKey;
        string priRsaText = GameServerConstant.PrivateKey;

        this.m_publicRsa = RSAUtil.LoadRSAPublicKey(pubRsaText);
        this.m_privateRsa = RSAUtil.LoadRSAPrivateKey(priRsaText);
    }
    
    public string MakeToken<T>(T bean) where T: RSATokenBean
    {
        var bytes = _signBytes(bean);
        var contentStr = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonHelper.ToJson(bean)));
        var signStr = Convert.ToBase64String(bytes);

        StringBuilder sb = new StringBuilder();
        sb.Append(contentStr);
        sb.Append(SplitChart);
        sb.Append(signStr);
        return sb.ToString();
    }

    public (bool,T) VerifyToken<T>(string token) where T: RSATokenBean
    {
        try
        {
            var splits = token.Split(SplitChart);
            if (splits.Length != 2)
            {
                return (false, null);
            }

            var jsonBytes = Convert.FromBase64String(splits[0]);
            var signBytes = Convert.FromBase64String(splits[1]);
            T  t = JsonHelper.FromJson<T>(Encoding.UTF8.GetString(jsonBytes));
            if (t == null)
            {
                return (false,null);
            }

            return (_verifyBytes(t, signBytes), t);
        }
        catch (Exception)
        {
            return (false,null);
        }
    }
    
    
    private byte[] _signBytes<T>(T bean) where T : RSATokenBean
    {
        var json = JsonHelper.ToJson(bean);
        var data = Encoding.UTF8.GetBytes(json);
        return RSAUtil.SignData(m_privateRsa, data);
    }

    private  bool _verifyBytes<T>(T bean, byte[] signBytes) where T : RSATokenBean
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