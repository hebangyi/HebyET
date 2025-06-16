using MemoryPack;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;

namespace ET
{
    public struct EntryEvent1
    {
    }   
    
    public struct EntryEvent2
    {
    } 
    
    public struct EntryEvent3
    {
    }
    
    public static class Entry
    {
        public static void Init()
        {
            
        }
        
        public static void Start()
        {
            StartAsync().Coroutine();
        }
        
        private static async ETTask StartAsync()
        {
            WinPeriod.Init();

            // 注册Mongo type
            MongoRegister.Init();
            // 注册Entity序列化器
            EntitySerializeRegister.Init();
            ApplicationContext.Instance.AddSingleton<IdGenerater>();
            ApplicationContext.Instance.AddSingleton<OpcodeType>();
            ApplicationContext.Instance.AddSingleton<ObjectPool>();
            ApplicationContext.Instance.AddSingleton<MessageQueue>();
            ApplicationContext.Instance.AddSingleton<NetServices>();
            ApplicationContext.Instance.AddSingleton<NavmeshComponent>();
            ApplicationContext.Instance.AddSingleton<LogMsg>();
            
            // 创建需要reload的code singleton
            CodeTypes.Instance.CreateCode();
            
            await ApplicationContext.Instance.AddSingleton<ConfigLoader>().LoadAsync();

            await FiberManager.Instance.Create(SchedulerType.Main, ConstFiberId.Main, 0, SceneType.Main, "");
        }
    }
}