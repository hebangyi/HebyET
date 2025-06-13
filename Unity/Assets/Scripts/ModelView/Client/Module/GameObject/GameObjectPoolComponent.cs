using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [ChildOf(typeof(GameObjectPoolComponent))]
    public class GameObjectLoadContext
    {
        public long InstanceId;
        public string Path;
        public long FormId;
        public Action<GameObject> DoLoadFinish;

        public struct GameObjectLoadHandler
        {
            public GameObject GameObject;
            public long Id;
        }

        public void LoadedFinish(GameObject gameObject)
        {
            DoLoadFinish?.Invoke(gameObject);
        }
    }

    [ComponentOf(typeof(UnityScene))]
    public class GameObjectPoolComponent : Entity, IAwake,IUpdate
    {
        public Dictionary<string, List<GameObject>> Pools = new();

        public Queue<GameObjectLoadContext> WaitLoadingQueue = new();
        public Dictionary<long, GameObjectLoadContext> LoadingContext = new();
    }
}