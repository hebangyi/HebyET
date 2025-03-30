using System;
using System.Collections.Generic;

namespace ET.Server;


[ComponentOf(typeof (Scene))]
public class MongoAutoSaveComponent: Entity, IAwake
{
    public bool isSaving = false;
    // 保存的 Entity
    public Dictionary<long, MongoEntity> SaveMongoEntities = new();
}

public struct MongoAutoSaveEvent
{
    public Type EntityType;
    public List<long> Ids;
}