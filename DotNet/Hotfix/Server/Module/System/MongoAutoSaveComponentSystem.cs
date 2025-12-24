using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;

namespace ET.Server;

[EntitySystemOf(typeof(MongoAutoSaveComponent))]
[FriendOf(typeof(MongoAutoSaveComponent))]
public static partial class MongoAutoSaveComponentSystem
{
    [Invoke(TimerInvokeType.MongoCacheCheckerTimer)]
    public class MongoCacheAgentComponentTimer : ATimer<MongoAutoSaveComponent>
    {
        protected override void Run(MongoAutoSaveComponent self)
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

    private static void CheckTimer(this MongoAutoSaveComponent self)
    {
        self.saveCacheData().Coroutine();
    }

    [EntitySystem]
    private static void Awake(this MongoAutoSaveComponent self)
    {
        self.TryAddComponent<MongoDBComponent>();
        self.Root().GetComponent<TimerComponent>()
                .NewRepeatedTimer(1 * 1000, TimerInvokeType.MongoCacheCheckerTimer, self);
    }

    public static void AddSaveEntity(this MongoAutoSaveComponent self, MongoEntity mongoEntity)
    {
        self.SaveMongoEntities.Add(mongoEntity.Id, mongoEntity);
    }

    private static async ETTask saveCacheData(this MongoAutoSaveComponent self)
    {
        try
        {
            if (self.isSaving)
            {
                return;
            }

            if (self.SaveMongoEntities.Count <= 0)
            {
                return;
            }

            Dictionary<Type, Queue<MongoEntity>> type2MongoEntities = new Dictionary<Type, Queue<MongoEntity>>();
            foreach (MongoEntity mongoEntity in self.SaveMongoEntities.Values)
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
                        await self.Root().GetComponent<MongoDBComponent>().SaveBatch(type.Name, batchSaveEntities);
                        EventSystem.Instance.Publish(self.Root(),
                            new MongoAutoSaveEvent() { EntityType = type, Ids = batchSaveEntities.Select(x => x.Id).ToList() });
                        Log.Info($"数据落地 : {type.Name} 数量 :{batchSaveEntities.Count}");
                    }
                }
            }

            self.SaveMongoEntities.Clear();
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

    public static async ETTask ServerExit(this MongoAutoSaveComponent self)
    {
        Log.Info("程序退出保存缓存");
        await self.saveCacheData();
        Log.Info("缓存保存完毕");
        await ETTask.CompletedTask;
    }
}