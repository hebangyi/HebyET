

using System;
using System.Collections.Generic;

namespace ET.Server;

[ComponentOf(typeof(LobbyRole))]
public class LobbySyncUnitDataComponent : Entity, IDestroy, IAwake
{
    public uint frame;
    public Dictionary<Type, IServerData> CacheDirtyData = new Dictionary<Type, IServerData>();
}