namespace ET;

public partial class SceneNodeInfo
{
    public string InnerIpAndOuterPortAddress
    {
        get { return $"{this.InnerIp}:{this.OuterPort}"; }
    }

    public string InnerIpAndInnerPortAddress
    {
        get { return $"{this.InnerIp}:{this.InnerPort}"; }
    }

    public string OuterIpAndOuterPortAddress
    {
        get { return $"{this.OuterIp}:{this.OuterPort}"; }
    }
}