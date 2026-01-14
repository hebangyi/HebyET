using System;
using System.Collections.Generic;
using System.IO;

namespace ET
{
    [Invoke]
    public class GetAllConfigTypes : AInvokeHandler<ConfigLoader.GetAllConfigTypes, ETTask<List<Type>>>
    {
        public override async ETTask<List<Type>> Handle(ConfigLoader.GetAllConfigTypes args)
        {
            List<Type> ret = new List<Type>();
            HashSet<Type> configTypes = CodeTypes.Instance.GetAttributeTypes(typeof(ConfigAttribute));
            foreach (Type configType in configTypes)
            {
                string configFilePath = null;
                configFilePath = $"../Config/Excel/s/{configType.Name}.bytes";
                if (!File.Exists(configFilePath))
                {
                    configFilePath = $"../Config/Excel/cs/{configType.Name}.bytes";
                }

                if (!File.Exists(configFilePath))
                {
                    continue;
                }

                ret.Add(configType);
            }

            await ETTask.CompletedTask;
            return ret;
        }
    }

    [Invoke]
    public class GetOneConfigBytes : AInvokeHandler<ConfigLoader.GetOneConfigBytes, ETTask<byte[]>>
    {
        public override async ETTask<byte[]> Handle(ConfigLoader.GetOneConfigBytes args)
        {
            string configFilePath = null;
            configFilePath = $"../Config/Excel/s/{args.Type.Name}.bytes";
            if (!File.Exists(configFilePath))
            {
                configFilePath = $"../Config/Excel/cs/{args.Type.Name}.bytes";
            }

            if (!File.Exists(configFilePath))
            {
                return new byte[] { };
            }

            byte[] configBytes = File.ReadAllBytes(configFilePath);
            await ETTask.CompletedTask;
            return configBytes;
        }
    }
}