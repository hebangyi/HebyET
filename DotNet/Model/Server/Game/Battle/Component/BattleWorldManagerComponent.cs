using System.Collections.Generic;

namespace ET.Server;

[ComponentOf(typeof(Scene))]
public class BattleWorldManagerComponent: Entity, IAwake, IUpdate
{
    public static BattleWorldManagerComponent Instance;
    
    // 所有在战斗中托管的世界
    public Dictionary<long, LogicWorld> Worlds = new ();

    public LinkedList<LogicWorld> UpdateQueue = new();
}