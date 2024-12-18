namespace ET.Server;

[ComponentOf(typeof(Scene))]
public class EtcdClientComponent: Entity, IAwake
{
    // 本服是否注册成功
    public bool IsRegisterOver = false;
}