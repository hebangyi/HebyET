using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace ET.Server;

[EntitySystemOf(typeof(MongoCacheAgentComponent))]
[FriendOf(typeof(MongoCacheAgentComponent))]
public static partial class MongoCacheAgentComponentSystem
{
    [Invoke(TimerInvokeType.MongoCacheCheckerTimer)]
    public class MongoCacheAgentComponentTimer : ATimer<MongoCacheAgentComponent>
    {
        protected override void Run(MongoCacheAgentComponent self)
        {
            try
            {
                self.CheckTimer();
            }
            catch (Exception e)
            {
                Log.Error($"move timer error: {self.Id}\n{e}");
            }
        }
    }

    private static void CheckTimer(this MongoCacheAgentComponent self)
    {
        self.saveCacheData().Coroutine();
    }

    [EntitySystem]
    private static void Awake(this MongoCacheAgentComponent self)
    {
        self.TryAddComponent<MongoDBComponent>();
        self.CheckTimerId = self.Root().GetComponent<TimerComponent>()
                .NewRepeatedTimer(60 * 1000, TimerInvokeType.MongoCacheCheckerTimer, self);
    }

    [EntitySystem]
    private static void Destroy(this MongoCacheAgentComponent self)
    {
        self.Root().GetComponent<TimerComponent>()?.Remove(ref self.CheckTimerId);
    }

    public static void AttachCache(this MongoCacheAgentComponent self, MongoEntity mongoEntity)
    {
        self.CacheMongoEntities.Add(mongoEntity.Id, mongoEntity);
    }

    public static void UnAttachCache(this MongoCacheAgentComponent self, long id)
    {
        if (!self.CacheMongoEntities.ContainsKey(id))
        {
            return;
        }

        if (self.CacheMongoEntities.Remove(id, out var entity))
        {
            self.Fiber().Root.GetComponent<MongoDBComponent>().Save(entity).Coroutine();    
        }
    }


    private static async ETTask saveCacheData(this MongoCacheAgentComponent self)
    {
        try
        {
            if (self.isSaving)
            {
                return;
            }
            
            if (self.CacheMongoEntities.Count <= 0)
            {
                return;
            }
            
            Dictionary<Type, Queue<MongoEntity>> type2MongoEntities = new Dictionary<Type, Queue<MongoEntity>>();
            foreach (MongoEntity mongoEntity in self.CacheMongoEntities.Values)
            {
                var type = mongoEntity.GetType();
                var saveEntities = type2MongoEntities.GetValueOrDefault(type);
                if (saveEntities == null)
                {
                    saveEntities = new Queue<MongoEntity>();
                    type2MongoEntities[type] = saveEntities;
                }
                saveEntities.Enqueue(mongoEntity);
            }

            foreach (var type2MongoEntity in type2MongoEntities)
            {
                var type = type2MongoEntity.Key;
                var queue = type2MongoEntity.Value;
                while (queue.Count > 0)
                {
                    List<MongoEntity> batchSaveEntities = new List<MongoEntity>();
                    while (queue.TryDequeue(out var entity))
                    {
                        batchSaveEntities.Add(entity);
                        if (batchSaveEntities.Count >= GameServerConstant.MongoDBCacheUpdateCount)
                        {
                            break;
                        }
                    }

                    if (batchSaveEntities.Count > 0)
                    {
                        await self.Root().GetComponent<MongoDBComponent>().SaveBatch(type.Name , batchSaveEntities);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Log.Error(e);
        }
        finally
        {
            self.isSaving = false;
        }
        await ETTask.CompletedTask;
    }
    
    public static async ETTask ServerExit(this MongoCacheAgentComponent self)
    {
        Log.Info("程序退出保存缓存");
        await self.saveCacheData();
        Log.Info("缓存保存完毕");
        await ETTask.CompletedTask;
    }
}