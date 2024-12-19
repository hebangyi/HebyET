using System;
using System.Collections.Generic;
using System.Reflection;

namespace ET.Server;

[ComponentOf(typeof(Scene))]
public class LobbyRoleToolComponent : Entity, IAwake
{
    public Dictionary<Type, List<FieldInfo>> Type2FieldInfos = new ();
}