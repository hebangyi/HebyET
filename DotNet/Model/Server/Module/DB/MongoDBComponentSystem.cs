using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
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

        public static async ETTask<List<string>> IndexList(this MongoDBComponent self, string collectionName)
        {
            var indexes = await self.MongoDatabase.GetCollection<BsonDocument>(collectionName).Indexes.ListAsync();
            // 将索引转换为 List<BsonDocument>
            List<BsonDocument> indexBsonDocumentList = await indexes.ToListAsync();
            // 输出所有索引的信息
            List<string> indexList = new List<string>();
            foreach (var index in indexBsonDocumentList)
            {
                indexList.Add(index.ToJson());
            }

            return indexList;
        }

        public static async ETTask TryCreateCollectionIndex(this MongoDBComponent self, string collectionName, List<string> fields)
        {
            try
            {
                if (fields == null || fields.Count == 0)
                {
                    return;
                }

                if (fields.Contains("_id"))
                {
                    return;
                }

                StringBuilder sbKey = new StringBuilder();
                // 1 表示升序 -1 表示降序
                // TODO 能不能优化字符串
                List<string> mongoKeys = fields.Select(s => s + "_1").ToList();
                sbKey = sbKey.AppendJoin('_', mongoKeys);
                string indexQuery = sbKey.ToString();

                if (self.QueryIndexs.Contains(indexQuery))
                {
                    return;
                }

                // 创建索引
                var collection = self.MongoDatabase.GetCollection<BsonDocument>(collectionName);
                IndexKeysDefinition<BsonDocument> indexKeysDef = null;
                foreach (var x in fields)
                {
                    if (indexKeysDef == null)
                    {
                        indexKeysDef = Builders<BsonDocument>.IndexKeys.Ascending(x);
                    }
                    else
                    {
                        indexKeysDef = indexKeysDef.Ascending(x);
                    }
                }

                var indexModel = new CreateIndexModel<BsonDocument>(indexKeysDef);
                await collection.Indexes.CreateOneAsync(indexModel);
                self.QueryIndexs.Add(indexQuery);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
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

        public static async ETTask Save(this MongoDBComponent self, MongoEntity entity)
        {
            if (entity == null)
            {
                Log.Error($"save entity is null: {entity.GetType().Name}");
                return;
            }

            var collectionName = entity.GetType().Name;
            await self.MongoDatabase.GetCollection<MongoEntity>(collectionName)
                    .ReplaceOneAsync(d => d.Id == entity.Id, entity, new ReplaceOptions { IsUpsert = true });
        }
        
        public static async ETTask SaveBatch(this MongoDBComponent self, string collectionName, List<MongoEntity> entities)
        {
            if (entities == null || entities.Count == 0)
            {
                Log.Error($"save entity is null");
                return;
            }

            var bulkOps = new List<WriteModel<MongoEntity>>();
            foreach (MongoEntity entity in entities)
            {
                if (entity == null)
                {
                    continue;
                }

                var filter = Builders<MongoEntity>.Filter.Eq(p => p.Id, entity.Id);
                var replace = new ReplaceOneModel<MongoEntity>(filter, entity);
                bulkOps.Add(replace);
            }
            await self.MongoDatabase.GetCollection<MongoEntity>(collectionName).BulkWriteAsync(bulkOps);
        }

        #endregion

        #region Delete

        public static async ETTask<long> Delete(this MongoDBComponent self, string collectionName, long id)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            DeleteResult result = await self.MongoDatabase.GetCollection<BsonDocument>(collectionName).DeleteOneAsync(filter);
            return result.DeletedCount;
        }

        public static async ETTask<long> Delete<T>(this MongoDBComponent self, long id) where T : MongoEntity
        {
            string collectionName = typeof(T).Name;
            DeleteResult result = await self.MongoDatabase.GetCollection<T>(collectionName).DeleteOneAsync(d => d.Id == id);
            return result.DeletedCount;
        }
        
        public static async ETTask<long> Delete<T>(this MongoDBComponent self, Expression<Func<T, bool>> filter)
                where T : MongoEntity
        {
            string collectionName = typeof(T).Name;
            DeleteResult result = await self.MongoDatabase.GetCollection<T>(collectionName).DeleteManyAsync(filter);
            return result.DeletedCount;
        }
        

        public static async ETTask<long> DeleteByIds(this MongoDBComponent self, string collectionName, params long[] ids)
        {
            var bulkOps = new List<WriteModel<BsonDocument>>();
            foreach (var id in ids)
            {
                var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
                var delete = new DeleteOneModel<BsonDocument>(filter);
                bulkOps.Add(delete);
            }

            var result = await self.MongoDatabase.GetCollection<BsonDocument>(collectionName).BulkWriteAsync(bulkOps);
            return result.DeletedCount;
        }

        public static async ETTask<long> DeleteByIds<T>(this MongoDBComponent self, params long[] ids) where T : MongoEntity
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
        #endregion
    }
}