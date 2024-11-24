
using System;

namespace ET;

public class JsonHelper
{
    public static string ToJson(object o)
    {
        return MongoHelper.ToJson(o);
    }
        
    public static object FromJson(Type type, string json)
    {
        if (json == null)
        {
            return null;
        }
        return MongoHelper.FromJson(type, json);
    }
        
    public static T FromJson<T>(string json)
    {
        if (json == null)
        {
            return default(T);
        }
        
        return MongoHelper.FromJson<T>(json);
    }
}