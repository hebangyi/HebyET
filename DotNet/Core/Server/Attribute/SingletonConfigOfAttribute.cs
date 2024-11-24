namespace ET.Server;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public class SingletonConfigOfAttribute(String key) : BaseAttribute
{
    // 配置的Key值
    public string Key = key;
}