using System.Collections.Generic;

namespace ET.Server;


[ComponentOf(typeof (Scene))]
public class MongoCacheAgentComponent: Entity, IAwake, IDestroy
{
    public long CheckTimerId;
    public Dictionary<long, EntityRef<MongoEntity>> CacheMongoEntities = new();
    public HashSet<long> removeIds = new ();
}