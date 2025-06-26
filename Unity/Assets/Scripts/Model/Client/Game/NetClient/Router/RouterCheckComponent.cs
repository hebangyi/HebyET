using System.Net;

namespace ET.Client
{
    [ComponentOf(typeof(Session))]
    public class RouterCheckComponent : Entity, IAwake<IPEndPoint>
    {
        public IPEndPoint RouterAddress;
    }
}