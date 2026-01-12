namespace ET
{
    public abstract class BaseCategory<T> : Singleton<T>, IMerge, IBaseCategory where T : Singleton<T>
    {
        public abstract void Merge(object o);

        public virtual void AfterLoadData()
        {
        }
    }

    public interface IBaseCategory
    {
        void AfterLoadData();
    }
}