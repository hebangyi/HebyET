using System.ComponentModel;

namespace ET.Server
{
    /// <summary>
    /// 玩家 Entity绑定的Session
    /// </summary>
    [ComponentOf]
    public class EntityClientSessionComponent: Entity, IAwake
    {
        private EntityRef<Session> session;

        // 当Session 销毁的时候 这个地方获得不了
        public Session Session
        {
            get
            {
                return this.session;
            }
            set
            {
                this.session = value;
            }
        }
    }
}