using System;

namespace ET
{
    public static class UnitEntityElemDataHelper
    {
        public static IUnitEntityElemData FromBytes(long compId, byte[] bytes)
        {
            Type unitDataType = OpcodeType.Instance.GetType((ushort)compId);
            if (unitDataType == null)
            {
                Log.Error($"转换类型异常 compId {compId} 无法找到 转换 类型");
                return null;
            }
        
            var elemData = MemoryPackHelper.Deserialize(unitDataType, bytes, 0, bytes.Length) as IUnitEntityElemData;
            return elemData;
        }
    }
}