namespace ET
{
    public interface IDirtyHandler
    {
        void Dirty(long insId, IUnitEntityElemData elemData);
    }
}
