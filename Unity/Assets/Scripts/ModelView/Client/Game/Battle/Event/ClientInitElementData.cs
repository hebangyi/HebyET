namespace ET.Client
{
    /// <summary>
    /// 初始化绑定 UnitEntity Element Data数据事件
    /// </summary>
    public struct ClientInitElementData
    {
        public ushort ComponentId;
        public UnitEntity UnitEntity;
        public IUnitEntityElemData UnitEntityElemData;
    }
}