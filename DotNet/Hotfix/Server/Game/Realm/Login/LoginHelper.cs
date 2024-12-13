namespace ET.Server;
[FriendOf(typeof(EtcdClientComponent))]
public static class LoginHelper
{
    public static ErrorCode CheckLoginCommon(Scene scene)
    {
        var etcdClientComponent = scene.GetComponent<EtcdClientComponent>();
        if (!etcdClientComponent.IsRegisterOver)
        {
            return ErrorCode.ServerIsStarting;
        }

        return ErrorCode.ERR_Success;
    }
}