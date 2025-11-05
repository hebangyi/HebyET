using System;
using System.Reflection;

namespace ET.Client
{
    [EntitySystemOf(typeof(UnitySceneManagerComponent))]
    public static partial class UnitySceneManagerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UnitySceneManagerComponent self)
        {
            UnitySceneManagerComponent.Instance = self;
            
            var unitySceneContexts = CodeTypes.Instance.GetAttributeTypes(typeof(UnitySceneContext));
            foreach (var type in unitySceneContexts)
            {
                var instance = Activator.CreateInstance(type);
                if (instance is IUnitySceneContext unitySceneContext)
                {
                    var attributes = type.GetCustomAttributes(typeof(UnitySceneContext));
                    foreach (var attribute in attributes)
                    {
                        var unitySceneContextAttr = attribute as UnitySceneContext;
                        var unitySceneEnum = unitySceneContextAttr.UnitySceneEnum;
                        self.UnitySceneContexts[unitySceneEnum] = unitySceneContext;
                    }
                }
            }
        }

        [EntitySystem]
        private static void Update(this UnitySceneManagerComponent self)
        {
            
        }

        private static void StartLoading(this UnitySceneManagerComponent self)
        {
            
        }
    }
}

