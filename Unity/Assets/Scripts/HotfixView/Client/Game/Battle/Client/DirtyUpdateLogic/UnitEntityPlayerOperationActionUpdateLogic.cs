namespace ET.Client
{
    [UnitEntityViewLogic]
    public class UnitEntityPlayerOperationActionUpdateLogic: IClientEleUpdate
    {
        public ushort WatchComponentId()
        {
            return OpcodeType.Instance.GetOpcode(typeof(UnitEntityPlayerOperationAction));
        }

        public void OnUpdate(UnitEntity unitEntity, IUnitEntityElemData oldData, IUnitEntityElemData newData)
        {
            var unitEntityPlayerOperationAction = newData as UnitEntityPlayerOperationAction;
            if (unitEntityPlayerOperationAction == null)
            {
                return;
            }
            
            Log.Info($"MoveAngle : {unitEntityPlayerOperationAction.MoveAngle}");
            
            
        }
    }
}

