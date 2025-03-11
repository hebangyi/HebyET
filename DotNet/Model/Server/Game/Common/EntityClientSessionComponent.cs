using System.ComponentModel;

namespace ET.Server
{
    /// <summary>
    /// 玩家 Entity绑定的Session
    /// </summary>
    [ComponentOf(typeof(LobbyRole))]
    public class EntityClientSessionComponent: Entity, IAwake
    {
        private EntityRef<Session> session;

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