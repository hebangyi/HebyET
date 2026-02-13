using System;
using System.Collections.Generic;

namespace ET.Server;

[ComponentOf(typeof(Scene))]
public class BattleMatchComponent: Entity, IAwake
{
    public static BattleMatchComponent Instance;
    public Dictionary<long, MatchOrder> AllMatchOrders = new ();
    public Dictionary<long, MatchRoom> MatchSuccessRooms = new ();
    public Dictionary<Type, IMatchRuleFunc> MatchRules = new ();
}


public enum MatchRule
{
    None = 0,
    TimeOut = 1,
    TestMatchPlayer = 2,
}

public class MatchOrder
{
    // 玩家ID
    public long PlayerId;
    // 匹配时间
    public long MatchTime;
    // 物理地址通知匹配成功
    public ActorId ActorId;
}

public class MatchRoom
{
    public long RoomId;
    public List<MatchOrder> MatchOrders = new ();
}

public class MatchRuleAttribute(MatchRule matchRule) : BaseAttribute
{
    public MatchRule MatchRule = matchRule;
}

public interface IMatchRuleFunc
{
    void DoMatch();
}
