namespace ET
{
    public struct UpdateUnitEntityElementDirtyData
    {
        public ushort ComponentId;
        public UnitEntity UnitEntity;
        public IUnitEntityElemData OldUnitEntityElemData;
        public IUnitEntityElemData NewUnitEntityElemData;
    }
}