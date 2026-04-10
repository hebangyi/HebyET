
using System.Collections.Generic;

namespace ET.Server;

[MatchRule(MatchRule.TimeOut)]
public class TimeOutMatchRuleFuncHandler : IMatchRuleFunc
{
    public void DoMatch()
    {
        long now = TimeInfo.Instance.NowSec();
        List<long> timeOutOrders = new List<long>();
        foreach (var orderKv in BattleMatchComponent.Instance.AllMatchOrders)
        {
            if (orderKv.Value.MatchTime + MatchGlobalConfigCategory.Instance.Config.MatchTimeOutSecond <= now)
            {
                timeOutOrders.Add(orderKv.Key);
            }
        }

        // 将timeOutOrder的订单单独匹配房间
        foreach (var timeOutOrderId in timeOutOrders)
        {
            MatchRoom matchRoom = new();
            matchRoom.RoomId = IdGenerater.Instance.GenerateId();

            var matchOrder = BattleMatchComponent.Instance.AllMatchOrders.GetValueOrDefault(timeOutOrderId);
            matchRoom.MatchOrders.Add(matchOrder);

            BattleMatchComponent.Instance.MatchSuccessRooms.Add(matchRoom.RoomId, matchRoom);
        }

        // 将timeOut的匹配订单移除
        foreach (var timeOutOrderId in timeOutOrders)
        {
            BattleMatchComponent.Instance.AllMatchOrders.Remove(timeOutOrderId);
        }
    }
}