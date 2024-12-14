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

        public static async ETTask<List<T>> QueryJson<T>(this MongoDBComponent self, string json) where T : MongoEntity
        {
            string collectionName = typeof(T).Name;
            FilterDefinition<T> filterDefinition = new JsonFilterDefinition<T>(json);
            IAsyncCursor<T> cursor = await self.MongoDatabase.GetCollection<T>(collectionName).FindAsync(filterDefinition);
            return await cursor.ToListAsync();
        }

        #endregion

        #region Insert

        public static async ETTask InsertBatch<T>(this MongoDBComponent self, int zone, IEnumerable<T> list) where T : MongoEntity
        {
            string collectionName = typeof(T).Name;
            await self.MongoDatabase.GetCollection<T>(collectionName).InsertManyAsync(list);
        }

        #endregion

        #region Save

        public static async ETTask Save<T>(this MongoDBComponent self, T entity) where T : MongoEntity
        {
            if (entity == null)
            {
                Log.Error($"save entity is null: {typeof(T).Name}");

                return;
            }

            var collectionName = entity.GetType().Name;
            await self.MongoDatabase.GetCollection<T>(collectionName)
                    .ReplaceOneAsync(d => d.Id == entity.Id, entity, new ReplaceOptions { IsUpsert = true });
        }

        public static void SaveNotWait<T>(this MongoDBComponent self, T entity)
                where T : MongoEntity
        {
            self.Save(entity).Coroutine();
        }

        public static async ETTask SaveBatch<T>(this MongoDBComponent self, List<T> entities) where T: MongoEntity
        {
            if (entities == null)
            {
                Log.Error($"save entity is null");
                return;
            }

            string collectionName = typeof(T).Name;
            var bulkOps = new List<WriteModel<Entity>>();
            foreach (Entity entity in entities)
            {
                if (entity == null)
                {
                    continue;
                }

                var filter = Builders<Entity>.Filter.Eq(p => p.Id, entity.Id);
                var replace = new ReplaceOneModel<Entity>(filter, entity);
                bulkOps.Add(replace);
            }

            var result = await self.MongoDatabase.GetCollection<Entity>(collectionName).BulkWriteAsync(bulkOps);
        }

        #endregion

        #region Delete

        public static async ETTask<long> Delete<T>(this MongoDBComponent self, long id) where T : MongoEntity
        {
            string collectionName = typeof(T).Name;
            DeleteResult result = await self.MongoDatabase.GetCollection<T>(collectionName).DeleteOneAsync(d => d.Id == id);
            return result.DeletedCount;
        }

        public static async ETTask<long> DeleteByIds<T>(this MongoDBComponent self, params long[] ids) where T: MongoEntity
        {
            string collectionName = typeof(T).Name;
            var bulkOps = new List<WriteModel<T>>();
            
            foreach (var id in ids)
            {
                var filter = Builders<T>.Filter.Eq(p => p.Id, id);
                var delete = new DeleteOneModel<T>(filter);
                bulkOps.Add(delete);
            }
            
            var result = await self.MongoDatabase.GetCollection<T>(collectionName).BulkWriteAsync(bulkOps);
            return result.DeletedCount;
        }
        
        public static async ETTask<long> Delete<T>(this MongoDBComponent self, Expression<Func<T, bool>> filter)
                where T : MongoEntity
        {
            string collectionName = typeof(T).Name;
            DeleteResult result = await self.MongoDatabase.GetCollection<T>(collectionName).DeleteManyAsync(filter);
            return result.DeletedCount;
        }

        #endregion
    }
}