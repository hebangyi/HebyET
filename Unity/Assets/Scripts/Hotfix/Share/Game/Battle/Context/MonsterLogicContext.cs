using Unity.Mathematics;

namespace ET
{
    [LogicUnitEntityContext(UELayerTypeEnum.Monster, UETypeEnum.Monster)]
    public class MonsterLogicContext : BaseLogicUnitEntityContext
    {
        public override void InitCustomData(UnitEntity unitEntity)
        {
            var unitEntityInitContext = unitEntity.GetComponent<UnitEntityInitContext>();
            
            var unitEntityPosition = unitEntity.CreateUnitEntityElemData<UnitEntityPosition>();
            var genData = unitEntityInitContext.Params as MonsterGenData;
            
            unitEntityPosition.Position = genData.Position;
            unitEntityPosition.Position += new float2(-10, 10);

            var monsterRuntimeAIData = unitEntity.CreateUnitEntityLogicElemData<MonsterRuntimeData>();
            monsterRuntimeAIData.BornPosition = unitEntityPosition.Position;
        }

        public override void Init(UnitEntity unitEntity)
        {
            var unitEntityPosition = unitEntity.GetUnitEntityElemData<UnitEntityPosition>();
            unitEntity.AddComponent<AOIUnitEntity, float2, UETypeEnum>(unitEntityPosition.Position, UETypeEnum.Monster);
            unitEntity.AddComponent<MonsterAIComponent>();
            // 怪物状态机
            unitEntity.AddComponent<MonsterStateMachineComponent>();
            
            var logicWorld = unitEntity.LogicWorld();
            logicWorld.Monsters[unitEntity.InsId] = unitEntity;
        }

        public override void Destroy(UnitEntity unitEntity)
        {
            var logicWorld = unitEntity.LogicWorld();
            logicWorld.Monsters.Remove(unitEntity.InsId);
        }
    }
}