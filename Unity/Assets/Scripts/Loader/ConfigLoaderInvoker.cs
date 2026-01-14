using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ET
{
    [Invoke]
    public class GetAllConfigBytes : AInvokeHandler<ConfigLoader.GetAllConfigTypes, ETTask<List<Type>>>
    {
        public override async ETTask<List<Type>> Handle(ConfigLoader.GetAllConfigTypes args)
        {
            List<Type> ret = new List<Type>();
            HashSet<Type> configTypes = CodeTypes.Instance.GetAttributeTypes(typeof(ConfigAttribute));

            foreach (var configType in configTypes)
            {
                if (ResourcesComponent.Instance.IsAssetExist($"Assets/Bundles/Config/{configType.Name}.bytes"))
                {
                    ret.Add(configType);
                }
            }

            return ret;
        }
    }

    [Invoke]
    public class GetOneConfigBytes : AInvokeHandler<ConfigLoader.GetOneConfigBytes, ETTask<byte[]>>
    {
        public override async ETTask<byte[]> Handle(ConfigLoader.GetOneConfigBytes args)
        {
            TextAsset v = await ResourcesComponent.Instance.LoadAssetAsync<TextAsset>($"Assets/Bundles/Config/{args.Type.Name}.bytes");
            return v.bytes;
        }
    }
}