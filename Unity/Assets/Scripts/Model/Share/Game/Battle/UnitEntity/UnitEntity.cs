using System.Collections.Generic;

namespace ET
{
    [ChildOf(typeof(World))]
    public class UnitEntity : Entity, IAwake, IDestroy
    {
        public long InsId;
        // 客户端和服务器通用的逻辑数据
        public Dictionary<ushort, IUnitEntityElemData> UnitEntityData;
# if DOTNET
        // 等待同步的数据队列
        public Dictionary<ushort, IUnitEntityElemData> DirtySyncUnitEntityData;

# endif
    }
} 