using System.Collections.Generic;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MongoDB.Bson;

namespace ET.Server;

[FriendOf(typeof(MongoFieldAutoLoadComponent))]
[FriendOf(typeof(MongoEntity))]
public static class MongoEntityHelper
{
    public static void AttachData(this Entity entity, MongoEntity mongoEntity)
    {
        var mongoFieldAutoLoadComponent = entity.Fiber().Root.GetComponent<MongoFieldAutoLoadComponent>();
        if (mongoFieldAutoLoadComponent == null)
        {
            Log.Error("Attach RoleData Not Found LobbyRoleToolComponent");
            return;
        }

        foreach (var componentKv in entity.Components)
        {
            var componentType = componentKv.Value.GetType();
            var fieldInfos = mongoFieldAutoLoadComponent.Type2FieldInfos.GetValueOrDefault(componentType);
            if (fieldInfos == null)
            {
                continue;
            }

            foreach (var fieldInfo in fieldInfos)
            {
                var mongoFieldAttribute = fieldInfo.GetCustomAttribute(typeof(MongoFieldAttribute)) as MongoFieldAttribute;
                if (mongoFieldAttribute == null)
                {
                    continue;
                }

                var data = mongoEntity.DataCollections.GetValueOrDefault(mongoFieldAttribute.Field);
                if (data == null)
                {
                    continue;
                }

                var fieldIns = MongoHelper.Deserialize(fieldInfo.FieldType, data.ToBson());
                fieldInfo.SetValue(componentKv.Value, fieldIns);
            }
        }
    }

    public static T UnAttachData<T>(this Entity entity) where T : MongoEntity, new()
    {
        var mongoFieldAutoLoadComponent = entity.Fiber().Root.GetComponent<MongoFieldAutoLoadComponent>();
        if (mongoFieldAutoLoadComponent == null)
        {
            Log.Error("UnAttachRoleData RoleData Not Found LobbyRoleToolComponent");
            return null;
        }

        T mongoEntity = new();
        foreach (var componentKv in entity.Components)
        {
            var componentIns = componentKv.Value;
            var componentType = componentKv.Value.GetType();
            var fieldInfos = mongoFieldAutoLoadComponent.Type2FieldInfos.GetValueOrDefault(componentType);
            if (fieldInfos == null)
            {
                continue;
            }

            foreach (var fieldInfo in fieldInfos)
            {
                var mongoFieldAttribute = fieldInfo.GetCustomAttribute(typeof(MongoFieldAttribute)) as MongoFieldAttribute;
                if (mongoFieldAttribute == null)
                {
                    continue;
                }

                var fieldVal = fieldInfo.GetValue(componentIns);
                mongoEntity.DataCollections[mongoFieldAttribute.Field] = fieldVal;
            }
        }
        
        return mongoEntity;
    }
}