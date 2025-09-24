using System;
using System.Collections.Generic;
using System.Linq;

namespace ET
{
    [EntitySystemOf(typeof(BattleUnitEntityLogicManagerComponent))]
    [FriendOf(typeof(BattleUnitEntityLogicManagerComponent))]
    public static partial class BattleUnitEntityLogicManagerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this BattleUnitEntityLogicManagerComponent self)
        {
            BattleUnitEntityLogicManagerComponent.Instance = self;

            var logicTypes = CodeTypes.Instance.GetAttributeTypes(typeof(UnitEntityLogicAttribute));
            foreach (var type in logicTypes)
            {
                var handler = Activator.CreateInstance(type);
                if (handler is IBattleInit dataLogic)
                {
                    var componentId = dataLogic.WatchComponentId();

                    var logics = self.CompId2InitLogics.GetValueOrDefault(componentId);
                    if (logics == null)
                    {
                        logics = new List<IBattleInit>();
                        self.CompId2InitLogics.Add(componentId, logics);
                    }

                    logics.Add(dataLogic);
                }

                if (handler is IBattleTick tickLogic)
                {
                    self.Type2TickLogics[handler.GetType()] = tickLogic;
                }
            }
        }

        public static List<IBattleInit> GetInitLogicByComponentId(this BattleUnitEntityLogicManagerComponent self, ushort componentId)
        {
            return self.CompId2InitLogics.GetValueOrDefault(componentId);
        }

        public static IBattleTick GetTickLogicByType(this BattleUnitEntityLogicManagerComponent self, Type logicType)
        {
            return self.Type2TickLogics.GetValueOrDefault(logicType);
        }
    }
}