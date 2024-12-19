using System.Collections.Generic;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ET.Server;

[EntitySystemOf(typeof(LobbyRoleToolComponent))]
[FriendOf(typeof(LobbyRoleToolComponent))]
public static partial class LobbyRoleToolComponentSystem
{
    [EntitySystem]
    private static void Awake(this LobbyRoleToolComponent self)
    {
        var components = EntitySystemSingleton.Instance.EntityAutoAddComponents.GetValueOrDefault(typeof(LobbyRole));
        if (components == null)
        {
            return;
        }

        foreach (var component in components)
        {
            var fieldList = self.Type2FieldInfos.GetValueOrDefault(component);
            if (fieldList == null)
            {
                fieldList = new List<FieldInfo>();
                self.Type2FieldInfos[component] = fieldList;
            }
            
            var fields = component.GetFields();
            foreach (var field in fields)
            {
                var roleFieldAttribute = field.GetCustomAttribute(typeof(RoleFieldAttribute));
                if (roleFieldAttribute == null)
                {
                    continue;
                }
                fieldList.Add(field);
            }
        }
    }
}