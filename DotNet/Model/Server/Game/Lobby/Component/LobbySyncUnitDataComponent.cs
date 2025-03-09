

using System;
using System.Collections.Generic;

namespace ET.Server;

[AutoAddComponent([typeof(LobbyRole)])]
[ComponentOf(typeof(LobbyRole))]
public class LobbySyncUnitDataComponent : Entity, IDestroy
{
    public uint frame;
    public Dictionary<Type, object> CacheDirtyData = new Dictionary<Type, object>();
}