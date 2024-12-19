using System;

namespace ET.Server;

[AttributeUsage(AttributeTargets.Field)]
public class RoleFieldAttribute(string field) : BaseAttribute
{
    public string Field = field;
}