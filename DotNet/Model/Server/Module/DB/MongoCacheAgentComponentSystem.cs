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
        Log.Info("CheckTimer");
    }

    [EntitySystem]
    private static void Awake(this MongoCacheAgentComponent self)
    {
        self.TryAddComponent<MongoDBComponent>();
        self.CheckTimerId = self.Root().GetComponent<TimerComponent>()
                .NewRepeatedTimer(10 * 1000, TimerInvokeType.MongoCacheCheckerTimer, self);
    }

    [EntitySystem]
    private static void Destroy(this MongoCacheAgentComponent self)
    {
        self.Root().GetComponent<TimerComponent>()?.Remove(ref self.CheckTimerId);
    }

    public static void AddInCache(this MongoCacheAgentComponent self, MongoEntity mongoEntity)
    {
        self.CacheMongoEntities.Add(mongoEntity.Id, mongoEntity);
    }

    public static void RemoveInCache(this MongoCacheAgentComponent self, long id)
    {
        if (!self.CacheMongoEntities.ContainsKey(id))
        {
            return;
        }

        var mongoEntity = self.CacheMongoEntities.Remove(id);
        self.Fiber().Root.GetComponent<MongoDBComponent>().Save(mongoEntity).Coroutine();
    }

    public static async ETTask ServerExit(this MongoCacheAgentComponent self)
    {
        Log.Info("程序退出保存缓存");
        if (self.CacheMongoEntities.Count <= 0)
        {
            return;
        }
        
        List<MongoEntity> batches = new List<MongoEntity>();
        foreach (MongoEntity mongoEntity in self.CacheMongoEntities.Values)
        {
            batches.Add(mongoEntity);
            if (batches.Count >= GameConstant.MongoDBCacheUpdateCount)
            {
                await self.Fiber().Root.GetComponent<MongoDBComponent>().SaveBatch(batches);
                batches.Clear();
            }
        }

        if (batches.Count > 0)
        {
            await self.Fiber().Root.GetComponent<MongoDBComponent>().SaveBatch(batches);
        }
        Log.Info("缓存保存完毕!");
        await ETTask.CompletedTask;
    }
}