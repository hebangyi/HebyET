namespace ET.Client
{
    public abstract class BaseClientUnitEntityContext : IClientUnitEntityContext
    {
        public virtual void Init(UnitEntity unitEntity)
        {
        }

        public virtual  void CreateView(UnitEntity unitEntity)
        {
            var clientWorld = unitEntity.ClientWorld();
            GameObjectHelper.CreateGameObjectIns(clientWorld, unitEntity);
        }

        public virtual  void Destroy(UnitEntity unitEntity)
        {
        }
    }

    [ClientUnitEntityContext(UETypeEnum.None)]
    public class DefaultClientEntityContext : BaseClientUnitEntityContext
    {
        
    }
    
}

