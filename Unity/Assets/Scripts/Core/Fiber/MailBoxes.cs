using System.Collections.Generic;

namespace ET
{
    public class Mailboxes
    {
        private readonly Dictionary<long, EntityRef<Entity>> mailboxes = new();
        
        public void Add(Entity entity)
        {
            this.mailboxes.Add(entity.InstanceId, entity);
        }
        
        public void Remove(long instanceId)
        {
            this.mailboxes.Remove(instanceId);
        }

        public Entity Get(long instanceId)
        {
            Entity entity = this.mailboxes.GetValueOrDefault(instanceId);
            return entity;
        }
    }
}