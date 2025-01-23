using System.Collections.Generic;
using System.ComponentModel;

namespace ET.Server;


[ComponentOf(typeof(Scene))]
public class LobbyRoleComponent :Entity, IAwake, IDestroy
{
}