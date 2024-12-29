using System.Collections.Generic;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ET.Server;

[EntitySystemOf(typeof(MongoFieldAutoLoadComponent))]
[FriendOf(typeof(MongoFieldAutoLoadComponent))]
public static partial class MongoFieldAutoLoadComponentSystem
{
    [EntitySystem]
    private static void Awake(this MongoFieldAutoLoadComponent self)
    {

        foreach (var type in CodeTypes.Instance.GetAllTypes().Values)
        {
            var fields = type.GetFields();
            foreach (var field in fields)
            {
                var attribute = field.GetCustomAttribute(typeof(MongoFieldAttribute));
                if (attribute == null)
                {
                    continue;
                }
                
                var fieldList = self.Type2FieldInfos.GetValueOrDefault(type);
                
                if (fieldList == null)
                {
                    fieldList = new List<FieldInfo>();
                    self.Type2FieldInfos[type] = fieldList;
                }
                fieldList.Add(field);
            }
        }
    }
}