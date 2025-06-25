using System;

namespace ET.Server
{
    
    [Invoke(TimerInvokeType.BattleMatchTimer)]
    public class MongoCacheAgentComponentTimer : ATimer<BattleMatchComponent>
    {
        protected override void Run(BattleMatchComponent self)
        {
            try
            {
                self.CheckTimer();
            }
            catch (Exception e)
            {
                Log.Error($"move timer error: {self.Id}\n{e}");
            }
        }
    }
    
    
    [EntitySystemOf(typeof(BattleMatchComponent))]
    [FriendOf(typeof(BattleMatchComponent))]
    public static partial class BattleMatchComponentSystem
    {
        [EntitySystem]
        private static void Awake(this BattleMatchComponent self)
        {
            BattleMatchComponent.Instance = self;
            var types = CodeTypes.Instance.GetAttributeTypes(typeof(MatchRuleAttribute));
            foreach (Type type in types)
            {
                var instance = Activator.CreateInstance(type) as IMatchRuleFunc;
                self.MatchRules[type] = instance;
            }
            
            self.Root().GetComponent<TimerComponent>()
                    .NewRepeatedTimer(1 * 1000, TimerInvokeType.BattleMatchTimer, self);
        }

        public static bool HasMatchOrder(this BattleMatchComponent self, long playerId)
        {
            return self.AllMatchOrders.ContainsKey(playerId);
        }
        
        public static void AddMatchOrder(this BattleMatchComponent self, MatchOrder matchOrder)
        {
            self.AllMatchOrders.Add(matchOrder.PlayerId, matchOrder);
        }
        

        public static void CheckTimer(this BattleMatchComponent self)
        {
            // 执行匹配规则
            foreach (var matchRulesValue in self.MatchRules.Values)
            {
                matchRulesValue.DoMatch();
            }
            
            if (self.MatchSuccessRooms.Count > 0)
            {
                foreach (var room in self.MatchSuccessRooms.Values)
                {
                    // 通知 LobbyRole 房间创建
                    var world = BattleWorldManagerComponent.Instance.CreateWorld(room);
                    long worldId = world.Id;
                    var message = L2B_PlayerMatchSuccessNotify.Create();
                    message.worldId = worldId;
                    foreach (var matchOrder in room.MatchOrders)
                    {
                        self.Root().GetComponent<MessageSender>().Send(matchOrder.ActorId, message);
                    }
                }
                self.MatchSuccessRooms.Clear();
            }
        }
    }
}

