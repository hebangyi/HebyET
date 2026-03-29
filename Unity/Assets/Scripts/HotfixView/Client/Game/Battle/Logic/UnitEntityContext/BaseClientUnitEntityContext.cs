namespace ET.Client
{
    public abstract class BaseClientUnitEntityContext : IClientUnitEntityContext
    {
        public virtual void Init(ClientUnitEntity unitEntity)
        {
        }

        public virtual void CreateView(ClientUnitEntity unitEntity)
        {
            var clientWorld = unitEntity.ClientWorld();
            GameObjectHelper.CreateGameObjectIns(clientWorld, unitEntity).Coroutine();
        }

        public virtual void InitView(ClientUnitEntity unitEntity)
        {
        }

        public virtual void Destroy(ClientUnitEntity unitEntity)
        {
        }
    }

    [ClientUnitEntityContext(UETypeEnum.None)]
    public class DefaultClientEntityContext : BaseClientUnitEntityContext
    {
    }
}