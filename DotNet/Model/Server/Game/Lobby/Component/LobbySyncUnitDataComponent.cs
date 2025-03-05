

using System;
using System.Collections.Generic;

namespace ET.Server;

[AutoAddComponent([typeof(LobbyRole)])]
[ComponentOf(typeof(LobbyRole))]
public class LobbySyncUnitDataComponent : Entity, IAwake, IDestroy
{
    public uint frame;
    public Dictionary<Type, object> CacheDirtyData = new Dictionary<Type, object>();
}