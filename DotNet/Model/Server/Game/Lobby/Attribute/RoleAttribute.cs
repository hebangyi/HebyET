using System;

namespace ET.Server;

[AttributeUsage(AttributeTargets.Field)]
public class MongoFieldAttribute(string field) : BaseAttribute
{
    public string Field = field;
}