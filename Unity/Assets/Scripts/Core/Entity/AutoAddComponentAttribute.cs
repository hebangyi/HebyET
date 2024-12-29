using System;

namespace ET;

/// <summary>
/// Component 自动挂载
/// </summary>
public class AutoAddComponentAttribute : BaseAttribute
{
    public Type[] EntityTypes;
    public AutoAddComponentAttribute(Type[] entityTypes)
    {
        this.EntityTypes = entityTypes;
    }
}