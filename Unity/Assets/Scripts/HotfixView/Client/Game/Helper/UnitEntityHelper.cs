namespace ET.Client
{
    public static class UnitEntityHelper
    {
        public static ClientWorld ClientWorld(this UnitEntity unitEntity)
        {
            return unitEntity.GetParent<ClientWorld>();
        }
        
    }    
}
