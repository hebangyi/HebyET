using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace ET.Server
{
    [EntitySystemOf(typeof(MongoDBComponent))]
    [FriendOf(typeof(MongoDBComponent))]
    public static partial class MongoDBComponentSystem
    {
        [EntitySystem]
        private static void Awake(this MongoDBComponent self)
        {
            self.Config = ProcessConfig.Instance.GetSceneComponentConfig<MongoDBComponentConfig>(self.Root());
            self.MongoClient = new MongoClient(self.Config.DbConnection);
            self.MongoDatabase = self.MongoClient.GetDatabase(self.Config.DbName);
        }

        [EntitySystem]
        private static void Destroy(this ET.Server.MongoDBComponent self)
        {
        }

        #region Query

        public static async ETTask<T> Query<T>(this MongoDBComponent self, long id) where T : MongoEntity
        {
            string collectionName = typeof(T).Name;
            IAsyncCursor<T> cursor = await self.MongoDatabase.GetCollection<T>(collectionName).FindAsync(d => d.Id == id);
            return await cursor.FirstOrDefaultAsync();
        }

        public static async ETTask<List<T>> Query<T>(this MongoDBComponent self, int zone, Expression<Func<T, bool>> filter)
                where T : MongoEntity
        {
            string collectionName = typeof(T).Name;
            IAsyncCursor<T> cursor = await self.MongoDatabase.GetCollection<T>(collectionName).FindAsync(filter);
            return await cursor.ToListAsync();
        }

        public static async ETTask<List<T>> QueryOne<T>(this MongoDBComponent self, Expression<Func<T, bool>> filter)
                where T : MongoEntity
        {
            string collectionName = typeof(T).Name;
            IAsyncCursor<T> cursor = await self.MongoDatabase.GetCollection<T>(collectionName).FindAsync(filter);
            return await cursor.ToListAsync();
        }

        public static async ETTask<List<T>> Query<T>(this MongoDBComponent self, Expression<Func<T, bool>> filter)
                where T : MongoEntity
        {
            string collectionName = typeof(T).Name;
            IAsyncCursor<T> cursor = await self.MongoDatabase.GetCollection<T>(collectionName).FindAsync(filter);
            return await cursor.ToListAsync();
        }

        public static async ETTask<List<T>> Query1<T>(this MongoDBComponent self, Expression<Func<T, bool>> exp)
                where T : MongoEntity
        {
            
            if (exp is LambdaExpression lambda)
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
            
            ExpressionFilterDefinition<T> filter = new ExpressionFilterDefinition<T>(exp);
            IBsonSerializerRegistry serializerRegistry = BsonSerializer.SerializerRegistry;
            IBsonSerializer<T> documentSerializer = serializerRegistry.GetSerializer<T>();
            string json = filter.Render(documentSerializer, serializerRegistry).ToJson();
            return null;
        }

        public static async ETTask<List<T>> QueryJson<T>(this MongoDBComponent self, string json) where T : Entity
        {
            string collectionName = typeof(T).Name;
            FilterDefinition<T> filterDefinition = new JsonFilterDefinition<T>(json);
            IAsyncCursor<T> cursor = await self.MongoDatabase.GetCollection<T>(collectionName).FindAsync(filterDefinition);
            return await cursor.ToListAsync();
        }

        #endregion

        #region Insert

        public static async ETTask InsertBatch<T>(this DBComponent self, int zone, IEnumerable<T> list, string collection = null) where T : Entity
        {
            if (collection == null)
            {
                collection = typeof(T).Name;
            }

            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.DB, RandomGenerator.RandInt64() % DBComponent.TaskCount))
            {
                await self.GetCollection(zone, collection).InsertManyAsync(list);
            }
        }

        #endregion

        #region Save

        public static async ETTask Save<T>(this DBComponent self, int zone, T entity, string collection = null) where T : Entity
        {
            if (entity == null)
            {
                Log.Error($"save entity is null: {typeof(T).Name}");

                return;
            }

            if (collection == null)
            {
                collection = entity.GetType().Name;
            }

            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.DB, entity.Id % DBComponent.TaskCount))
            {
                await self.GetCollection(zone, collection).ReplaceOneAsync(d => d.Id == entity.Id, entity, new ReplaceOptions { IsUpsert = true });
            }
        }

        public static async ETTask Save<T>(this DBComponent self, int zone, long taskId, T entity, string collection = null) where T : Entity
        {
            if (entity == null)
            {
                Log.Error($"save entity is null: {typeof(T).Name}");

                return;
            }

            if (collection == null)
            {
                collection = entity.GetType().Name;
            }

            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.DB, taskId % DBComponent.TaskCount))
            {
                await self.GetCollection(zone, collection).ReplaceOneAsync(d => d.Id == entity.Id, entity, new ReplaceOptions { IsUpsert = true });
            }
        }

        public static async ETTask Save(this DBComponent self, int zone, long id, List<Entity> entities)
        {
            if (entities == null)
            {
                Log.Error($"save entity is null");
                return;
            }

            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.DB, id % DBComponent.TaskCount))
            {
                foreach (Entity entity in entities)
                {
                    if (entity == null)
                    {
                        continue;
                    }

                    await self.GetCollection(zone, entity.GetType().Name)
                            .ReplaceOneAsync(d => d.Id == entity.Id, entity, new ReplaceOptions { IsUpsert = true });
                }
            }
        }

        public static async ETTask SaveNotWait<T>(this DBComponent self, int zone, T entity, long taskId = 0, string collection = null)
                where T : Entity
        {
            if (taskId == 0)
            {
                await self.Save(zone, entity, collection);

                return;
            }

            await self.Save(zone, taskId, entity, collection);
        }

        #endregion

        #region Remove

        public static async ETTask<long> Remove<T>(this DBComponent self, int zone, long id, string collection = null) where T : Entity
        {
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.DB, id % DBComponent.TaskCount))
            {
                DeleteResult result = await self.GetCollection<T>(zone, collection).DeleteOneAsync(d => d.Id == id);

                return result.DeletedCount;
            }
        }

        public static async ETTask<long> Remove<T>(this DBComponent self, int zone, long taskId, long id, string collection = null) where T : Entity
        {
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.DB, taskId % DBComponent.TaskCount))
            {
                DeleteResult result = await self.GetCollection<T>(zone, collection).DeleteOneAsync(d => d.Id == id);

                return result.DeletedCount;
            }
        }

        public static async ETTask<long> Remove<T>(this DBComponent self, int zone, Expression<Func<T, bool>> filter, string collection = null)
                where T : Entity
        {
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.DB, RandomGenerator.RandInt64() % DBComponent.TaskCount))
            {
                DeleteResult result = await self.GetCollection<T>(zone, collection).DeleteManyAsync(filter);

                return result.DeletedCount;
            }
        }

        public static async ETTask<long> Remove<T>(this DBComponent self, int zone, long taskId, Expression<Func<T, bool>> filter,
        string collection = null)
                where T : Entity
        {
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.DB, taskId % DBComponent.TaskCount))
            {
                DeleteResult result = await self.GetCollection<T>(zone, collection).DeleteManyAsync(filter);

                return result.DeletedCount;
            }
        }

        #endregion
    }
}