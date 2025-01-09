using System.Collections.Generic;

namespace ET.Server;


[ComponentOf(typeof (Scene))]
public class MongoCacheAgentComponent: Entity, IAwake, IDestroy
{
    public bool isSaving = false;
    public long CheckTimerId;
    public Dictionary<long, EntityRef<MongoEntity>> CacheMongoEntities = new();
}