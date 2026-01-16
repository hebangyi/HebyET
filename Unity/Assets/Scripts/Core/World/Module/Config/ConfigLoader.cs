using System;
using System.Collections.Generic;
#if DOTNET || UNITY_STANDALONE
using System.Threading.Tasks;
#endif

namespace ET
{
    /// <summary>
    /// ConfigLoader会扫描所有的有ConfigAttribute标签的配置,加载进来
    /// </summary>
    public class ConfigLoader : Singleton<ConfigLoader>, ISingletonAwake
    {
        // 全量加载
        public struct GetAllConfigBytes
        {
            
        }
        
        public struct GetAllConfigTypes
        {
        }

        public struct GetOneConfigBytes
        {
            public Type Type { get; set; }
        }

        public void Awake()
        {
        }

        public async ETTask Reload(Type configType)
        {
            GetOneConfigBytes getOneConfigBytes = new() { Type = configType};
            byte[] oneConfigBytes = await EventSystem.Instance.Invoke<GetOneConfigBytes, ETTask<byte[]>>(getOneConfigBytes);
            LoadOneConfig(configType, oneConfigBytes);
        }

        public async ETTask LoadAsync()
        {
            
            List<Type> configTypes = await EventSystem.Instance.Invoke<GetAllConfigTypes, ETTask<List<Type>>>(new GetAllConfigTypes());
            foreach (var configType in configTypes)
            {
                var bytes = await EventSystem.Instance.Invoke<GetOneConfigBytes, ETTask<byte[]>>(new GetOneConfigBytes(){ Type = configType});
                LoadOneConfig(configType, bytes);
            }
        }

        private static void LoadOneConfig(Type configType, byte[] oneConfigBytes)
        {
            object category = MongoHelper.Deserialize(configType, oneConfigBytes, 0, oneConfigBytes.Length);
            
            if (category is IBaseCategory baseCategory)
            {
                Log.Info($"加载配置 [{configType.Name} 配置size [{oneConfigBytes.Length}]] 配置条数 [{baseCategory.Count()}]");
                baseCategory.AfterLoadData();
            }
            
            ASingleton singleton = category as ASingleton;
            ApplicationContext.Instance.AddSingleton(singleton);
        }
    }
}