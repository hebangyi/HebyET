namespace ET.Client
{
    public struct ClientUpdateElementData
    {
        public ushort ComponentId;
        public UnitEntity UnitEntity;
        public IUnitEntityElemData OldUnitEntityElemData;
        public IUnitEntityElemData NewUnitEntityElemData;
    }
}