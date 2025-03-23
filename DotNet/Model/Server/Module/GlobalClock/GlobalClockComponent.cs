namespace ET.Server;

[ComponentOf(typeof (Scene))]
public class GlobalClockComponent: Entity, IAwake
{
    public GlobalClockData GlobalClockData; // 存储的数据
}