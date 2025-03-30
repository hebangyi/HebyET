using System.Collections.Generic;

namespace ET.Server;


[ComponentOf(typeof (Scene))]
public class MongoAutoSaveComponent: Entity, IAwake
{
    public bool isSaving = false;
    public long CheckTimerId;
    // 保存的 Entity
    public Dictionary<long, MongoEntity> SaveMongoEntities = new();
}