using System;

namespace ET
{
    [ChildOf(typeof(CoroutineLockQueue))]
    public class CoroutineLock: Entity, IAwake<int, long, int>, IDestroy
    {
        public int type;
        public long key;
        public int level;
    }
}