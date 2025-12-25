using System;
using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    public class CoroutineLockComponent: Entity, IAwake, IScene, IUpdate
    {
        public Fiber Fiber { get; set; }
        public SceneType SceneType { get; set; }
        
        public readonly Queue<(int, long, int)> nextFrameRun = new();
    }
}