using System.Net;
using System.Reflection;
using MongoDB.Bson;

namespace ET.Server;


public class GlobalConfig
{
    public int BigZone = 1;
    public int ProcessId = 1;
    public string InnerIp = "127.0.0.1";
    public int InnerPort = 20101;
}

public class SingletonConfig
{
    public Dictionary<string, string> Configs = new Dictionary<string, string>();
}

public class SceneConfig
{
    public int SceneId;
    public SceneType SceneType;
    public Dictionary<string, string> ModulesConfig = new Dictionary<string, string>();
}

public class ProcessConfig : Singleton<ProcessConfig>,ISingletonAwake
{
    public const string Global = "Global";
    public const string Singleton = "Singleton";
    public const string Scenes = "Scenes";
    
    public GlobalConfig GlobalConfig;
    public SingletonConfig SingletonConfig;
    public Dictionary<long, SceneConfig> SceneConfigs = new Dictionary<long, SceneConfig>();
    
    public void Awake()
    {
        Load();
    }
    
    public void Load()
    {
        var configPath = $"../Config/Process/{Options.Instance.ProcessConfig}";
        Log.Info($"开始加载服务器启动配置文件 : {configPath}");
        if (string.IsNullOrWhiteSpace(configPath))
        {
            throw new Exception("ServerConfig ConfigPath Is Null");
        }
        
        if (!File.Exists(configPath))
        {
            throw new FileNotFoundException($"Server Config File Not Found , Path {configPath}");
        }
        
        string json = File.ReadAllText(configPath);
        
        Dictionary<string, BsonDocument> configDict = JsonHelper.FromJson<Dictionary<string, BsonDocument>>(json);
        
        if (!configDict.ContainsKey(Global) || !configDict.ContainsKey(Singleton) || !configDict.ContainsKey(Scenes))
        {
            throw new Exception("ServerConfig Content Error...");
        }
        
        BsonDocument globalDoc = configDict[Global];
        BsonDocument singletonDoc = configDict[Singleton];
        BsonDocument scenesDoc = configDict[Scenes];

        // Global 配置
        this.GlobalConfig = JsonHelper.FromJson<GlobalConfig>(globalDoc.ToString());
        
        // 通过配置文件设置Options 参数
        Options.Instance.Process = this.GlobalConfig.ProcessId;
        Options.Instance.BigZone = this.GlobalConfig.BigZone;
        
        // Singleton 配置
        Dictionary<string, string> singletonConfigs = new Dictionary<string, string>();
        foreach (var componentConfig in singletonDoc.Elements)
        {
            singletonConfigs[componentConfig.Name.ToLower()] = componentConfig.Value.ToString();
        }

        SingletonConfig singletonConfig = new ();
        singletonConfig.Configs = singletonConfigs;
        this.SingletonConfig = singletonConfig;
        // 各个模块以及配置
        foreach (var serverSceneConfig in scenesDoc.Elements)
        {
            if (Enum.TryParse<SceneType>(serverSceneConfig.Name, out var sceneType))
            {
                var bsonDocument = serverSceneConfig.Value.AsBsonDocument;
                var sceneId = bsonDocument.GetValue("SceneId").AsInt32;
                var modulesConfigVal = bsonDocument.GetValue("ModulesConfig").ToString();
                var modulesConfigDict = JsonHelper.FromJson<Dictionary<string, BsonDocument>>(modulesConfigVal);
                
                SceneConfig sceneConfig = new SceneConfig();
                sceneConfig.SceneId = sceneId;
                sceneConfig.SceneType = sceneType;
                Dictionary<string, string> moduleConfigs = new Dictionary<string, string>();
                foreach (var moduleConfigKv in modulesConfigDict)
                {
                    moduleConfigs[moduleConfigKv.Key.ToLower()] = moduleConfigKv.Value.ToString();
                }
                sceneConfig.ModulesConfig = moduleConfigs;
                this.SceneConfigs[sceneConfig.SceneId] = sceneConfig;
            }
            else
            {
                Log.Error($"没有找到SceneType : [{serverSceneConfig.Name}] 加载配置失败");
            }
        }
    }

    /// <summary>
    /// 获得Scene的Component相应的配置
    /// </summary>
    /// <param name="scene"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetSceneComponentConfig<T>(Scene scene) where T : class, new()
    {
        long sceneId = scene.Id;
        var sceneConfigs = this.SceneConfigs.GetValueOrDefault(sceneId);
        if (sceneConfigs == null)
        {
            // 使用默认配置
            return new T();
        }
        
        ComponentConfigOfAttribute attribute = typeof(T).GetCustomAttribute(typeof(ComponentConfigOfAttribute)) as ComponentConfigOfAttribute;
        if (attribute == null)
        {
            Log.Error($"{typeof(T).Name} ComponentConfigOfAttribute Is Null");
            return new T();
        }
        
        string moduleName = attribute.Key;
        String str = sceneConfigs.ModulesConfig.GetValueOrDefault(moduleName);
        T t = MongoHelper.FromJson<T>(str);
        return t ?? new T();
    }

    public T GetSingletonConfig<T>() where T : class, new()
    {
        
        SingletonConfigOfAttribute attribute = typeof(T).GetCustomAttribute(typeof(SingletonConfigOfAttribute)) as SingletonConfigOfAttribute;
        if (attribute == null)
        {
            Log.Error($"{typeof(T).Name} ComponentConfigOfAttribute Is Null");
            return new T();
        }
        
        string str = this.SingletonConfig.Configs.GetValueOrDefault(attribute.Key);
        T t = MongoHelper.FromJson<T>(str);
        return t ?? new T();
    }
}