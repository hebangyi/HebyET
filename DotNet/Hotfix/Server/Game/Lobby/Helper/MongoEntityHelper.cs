using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MongoDB.Bson;

namespace ET.Server;

[FriendOf(typeof(MongoEntity))]
public static class MongoEntityHelper
{
    public static void AttachData(Entity entity, MongoEntity mongoEntity)
    {
        foreach (var componentKv in entity.Components)
        {
            var componentType = componentKv.Value.GetType();
            var fieldInfos = componentType.GetFields().Where(field => field.GetCustomAttribute<MongoFieldAttribute>() != null).ToList();
            
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

    public static T UnAttachData<T>(Entity entity) where T : MongoEntity, new()
    {
        T mongoEntity = new();
        foreach (var componentKv in entity.Components)
        {
            var componentIns = componentKv.Value;
            var componentType = componentKv.Value.GetType();
            
            var fieldInfos = componentType.GetFields().Where(field => field.GetCustomAttribute<MongoFieldAttribute>() != null).ToList();

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