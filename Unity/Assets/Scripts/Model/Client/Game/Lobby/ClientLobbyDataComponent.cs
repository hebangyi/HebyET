using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ET.Client
{
    // 玩家大厅数据
    [ComponentOf(typeof(Scene))]
    public class ClientLobbyDataComponent : Entity, IAwake
    {
        // 帧数
        public uint Frame;
        // 客户端数据 同步服务器原始数据
        public Dictionary<Type, IClientData> ClientData = new ();
    }
}