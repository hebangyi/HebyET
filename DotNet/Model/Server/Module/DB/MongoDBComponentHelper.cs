using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace ET.Server;
[FriendOf(typeof(MongoDBComponent))]
public static class MongoDBComponentHelper
{
    public static List<string> GetFieldsFromExpression(Expression expression)
    {
        var fields = new List<string>();

        // 解析表达式树
        if (expression is LambdaExpression lambda)
        {
            var body = lambda.Body;

            // 检查是否是一个成员访问（例如 d.Id）
            if (body is MemberExpression member)
            {
                fields.Add(member.Member.Name);
            }
            // 处理其他表达式类型（例如复合表达式）
            else if (body is BinaryExpression binary)
            {
                // 如果是复合表达式，可以递归解析
                fields.AddRange(GetFieldsFromExpression(binary.Left));
                fields.AddRange(GetFieldsFromExpression(binary.Right));
            }
        }

        return fields;
    }

    public static List<string> GetFieldsFromBson<T>(Expression<Func<T, bool>> exp)
    {
        ExpressionFilterDefinition<T> filter = new ExpressionFilterDefinition<T>(exp);
        var fields = new List<string>();

        // 渲染 filter 为 BSON 查询
        var bson = filter.Render(BsonSerializer.SerializerRegistry.GetSerializer<T>(), BsonSerializer.SerializerRegistry);

        // 转换 BSON 为 JSON 字符串
        var json = bson.ToJson();

        // 在 JSON 字符串中提取字段名
        var document = BsonDocument.Parse(json);
        foreach (var element in document.Elements)
        {
            // 添加字段名
            fields.Add(element.Name);
        }

        return fields;
    }

}