namespace ET.Client
{
    public struct ClientUpdateElementData
    {
        public ushort ComponentId;
        public ClientUnitEntity UnitEntity;
        public IUnitEntityElemData OldUnitEntityElemData;
        public IUnitEntityElemData NewUnitEntityElemData;
    }
}