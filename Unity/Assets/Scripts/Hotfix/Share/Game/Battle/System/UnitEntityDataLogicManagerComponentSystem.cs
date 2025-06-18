using System;
using System.Collections.Generic;
using System.Linq;

namespace ET
{
    [EntitySystemOf(typeof(UnitEntityDataLogicManagerComponent))]
    [FriendOf(typeof(UnitEntityDataLogicManagerComponent))]
    public static partial class UnitEntityDataLogicManagerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UnitEntityDataLogicManagerComponent self)
        {
            var logicTypes = CodeTypes.Instance.GetAttributeTypes(typeof(UnitEntityDataLogicAttribute));

            foreach (var type in logicTypes)
            {
                if (Activator.CreateInstance(type) is IUnitEntityDataLogic dataLogic)
                {
                    foreach (ushort componentId in dataLogic.WatchComponentIds())
                    {
                        var logics = self.ComId2Logics.GetValueOrDefault(componentId);
                        if (logics == null)
                        {
                            logics = new List<IUnitEntityDataLogic>();
                            self.ComId2Logics.Add(componentId, logics);
                        }

                        logics.Add(dataLogic);
                    }
                }
            }

            UnitEntityDataLogicManagerComponent.Instance = self;
        }

        public static List<IUnitEntityDataLogic> GetLogicByComponentId(this UnitEntityDataLogicManagerComponent self, ushort componentId)
        {
            var logics = self.ComId2Logics.GetValueOrDefault(componentId);
            if (logics == null)
            {
                logics = new List<IUnitEntityDataLogic>();
                self.ComId2Logics.Add(componentId, logics);
            }

            return logics;
        }
    }
}