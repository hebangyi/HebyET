using MemoryPack;
using System.Collections.Generic;

namespace ET
{
    [MemoryPackable]
    [Message(ClientSyncData.TestClientData)]
    public partial class TestClientData : MessageObject
    {
        public static TestClientData Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(TestClientData), isFromPool) as TestClientData;
        }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }
            
            ObjectPool.Instance.Recycle(this);
        }
    }

    public static class ClientSyncData
    {
        public const ushort TestClientData = 30002;
    }
}