namespace ET.Client
{
    public abstract class BaseUnitEntity : IClientLifeCycle
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
}

