namespace ET;

/// <summary>
/// Component 自动挂载
/// </summary>
public class AutoAddComponentAttribute(Type[] entityTypes) : BaseAttribute
{
    public Type[] EntityTypes = entityTypes;
}