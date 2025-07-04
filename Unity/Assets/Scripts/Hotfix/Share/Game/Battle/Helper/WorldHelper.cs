using System.Collections.Generic;

namespace ET
{
    public static class WorldHelper
    {
        public static UnitEntity CreateEntity(this World self)
        {
            var unitEntity = self.AddChild<UnitEntity>();
            unitEntity.InsId = unitEntity.InstanceId;
            self.AllEntity[unitEntity.InsId] = unitEntity;
            
            self.PublishEvent(new CreateUnitEntityEvent(){UnitEntity = unitEntity});
            return unitEntity;
        }

        public static void PublishEvent<T>(this World self, T args) where T : struct
        {
            var events = BattleEventManagerComponent.Instance.AllEvents.GetValueOrDefault(typeof(T));
            if (events == null)
            {
                return;
            }

            foreach (var e in events)
            {
                if (e.WorldMode != self.WorldMode)
                {
                    continue;
                }
                
                if (!(e.BattleEvent is ABattleEvent<T> aEvent))
                {
                    Log.Error($"Battle Event Error: {e.GetType().FullName}");
                    continue;
                }
                
                aEvent.Handle(self, args);
            }
        }

        public static void RemoveEntity(this World self, UnitEntity unitEntity)
        {
            self.PublishEvent(new RemoveUnitEntity(){UnitEntity = unitEntity});
            self.AllEntity.Remove(unitEntity.InsId);
            unitEntity.Dispose();
        }
        
        public static BattleWorld ToBattleWorld(this World world)
        {
            BattleWorld battleWorld = BattleWorld.Create();
            battleWorld.WorldStatus = world.WorldStatusEnum;
            battleWorld.Frame = world.Frame;
            return battleWorld;
        }
    }
}