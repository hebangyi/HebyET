using Unity.Mathematics;

namespace ET
{
    [LogicUnitEntityContext(UELayerTypeEnum.Env, UETypeEnum.Tree)]
    public class TreeLogicContext: BaseLogicUnitEntityContext
    {
        public override void InitCustomData(UnitEntity unitEntity)
        {
            var unitEntityInitContext = unitEntity.GetComponent<UnitEntityInitContext>();
            
            var unitEntityPosition = unitEntity.GetUnitEntityElemData<UnitEntityPosition>();
            unitEntityPosition.Position = unitEntityInitContext.Params is float2 float2 ? float2 : default;
        }

        public override void Init(UnitEntity unitEntity)
        {
            var unitEntityPosition = unitEntity.GetUnitEntityElemData<UnitEntityPosition>();
            unitEntity.AddComponent<AOIUnitEntity, float2, UETypeEnum>(unitEntityPosition.Position, UETypeEnum.Tree);
        }
    }
}

