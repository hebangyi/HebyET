namespace ET.Server
{
    [MatchRule(MatchRule.TestMatchPlayer)]
    public class TestMatchPlayerRuleFuncHandler: IMatchRuleFunc
    {
        public void DoMatch()
        {
            if (BattleMatchComponent.Instance.AllMatchOrders.Count < MatchGlobalConfigCategory.Instance.Config.TestMatchPlayerCount)
            {
                return;
            }
            

            MatchRoom matchRoom = new ();
            matchRoom.RoomId = IdGenerater.Instance.GenerateId();
            foreach (var matchOrder in BattleMatchComponent.Instance.AllMatchOrders.Values)
            {
                matchRoom.MatchOrders.Add(matchOrder);
            }
            
            
            BattleMatchComponent.Instance.MatchSuccessRooms.Add(matchRoom.RoomId, matchRoom);
            BattleMatchComponent.Instance.AllMatchOrders.Clear();
        }
    }  
}

