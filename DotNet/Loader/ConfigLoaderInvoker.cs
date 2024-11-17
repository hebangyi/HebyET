using System;
using System.Collections.Generic;
using System.IO;

namespace ET
{
    [Invoke]
    public class GetAllConfigBytes: AInvokeHandler<ConfigLoader.GetAllConfigBytes, ETTask<Dictionary<Type, byte[]>>>
    {
        public override async ETTask<Dictionary<Type, byte[]>> Handle(ConfigLoader.GetAllConfigBytes args)
        {
            Dictionary<Type, byte[]> output = new Dictionary<Type, byte[]>();
            
            // TODO 以后通过地区读取配置
            HashSet<Type> configTypes = CodeTypes.Instance.GetAttributeTypes(typeof (ConfigAttribute));
            foreach (Type configType in configTypes)
            {
                string configFilePath = $"../Config/Excel/s/{configType.Name}.bytes";
                output[configType] = File.ReadAllBytes(configFilePath);
            }

            await ETTask.CompletedTask;
            return output;
        }
    }
    
    [Invoke]
    public class GetOneConfigBytes: AInvokeHandler<ConfigLoader.GetOneConfigBytes, byte[]>
    {
        public override byte[] Handle(ConfigLoader.GetOneConfigBytes args)
        {
            byte[] configBytes = File.ReadAllBytes($"../Config/Excel/s/{args.ConfigName}.bytes");
            return configBytes;
        }
    }
}