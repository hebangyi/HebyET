using System.Collections.Generic;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MongoDB.Bson;

namespace ET.Server;
[FriendOf(typeof(LobbyRoleToolComponent))]
public static class LobbyRoleHelper
{
    public static LobbyRole CreateLobbyPlayer(Fiber fiber, long playerId)
    {
        LobbyRole lobbyRole = new(fiber, playerId);
        lobbyRole.TryAddComponent<RoleDataComponent>();
        return lobbyRole;
    }

    public static void GetRoleComponent(this LobbyRole lobbyRole)
    {

    }


    public static void AttachRoleData(this LobbyRole lobbyRole, LobbyRoleInfo roleInfo)
    {
        var lobbyRoleToolComponent = lobbyRole.Fiber().Root.GetComponent<LobbyRoleToolComponent>();

        if (lobbyRoleToolComponent == null)
        {
            Log.Error("Attach RoleData Not Found LobbyRoleToolComponent");
            return;
        }

        foreach (var componentIns in lobbyRole.Components)
        {
            var componentType = componentIns.GetType();
            var fieldInfos = lobbyRoleToolComponent.Type2FieldInfos.GetValueOrDefault(componentType);
            if (fieldInfos == null)
            {
                continue;
            }

            foreach (var fieldInfo in fieldInfos)
            {
                var roleFieldAttribute = fieldInfo.GetCustomAttribute(typeof(RoleFieldAttribute)) as RoleFieldAttribute;
                if (roleFieldAttribute == null)
                {
                    continue;
                }

                var data = roleInfo.DataCollections.GetValueOrDefault(roleFieldAttribute.Field);
                if (data == null)
                {
                    continue;
                }

                var fieldIns = MongoHelper.Deserialize(fieldInfo.FieldType, data.ToBson());
                fieldInfo.SetValue(componentIns, fieldIns);
            }
        }
    }


    public static LobbyRoleInfo UnAttachRoleData(this LobbyRole lobbyRole)
    {
        LobbyRoleInfo roleInfo = new ();
        roleInfo.RoleId = roleInfo.RoleId;
        roleInfo.NickName = roleInfo.NickName;
        
        
        
        
        return roleInfo;
    }
}