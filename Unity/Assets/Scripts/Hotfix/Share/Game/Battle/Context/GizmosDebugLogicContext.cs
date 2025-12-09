namespace ET
{
    [LogicUnitEntityContext(UELayerTypeEnum.Plant, UETypeEnum.GizmosDebug)]
    public class GizmosDebugLogicContext: BaseLogicUnitEntityContext
    {
        public override void InitCustomData(UnitEntity unitEntity)
        {
            var playerInitContext = unitEntity.GetComponent<PlaneInitContext>();
            var plantGenContext = playerInitContext.PlantGenContext;
            
            // 计算总的Cell数量
            var gizmosDebugInfo = unitEntity.CreateUnitEntityElemData<GizmosPlantInfo>();
            
            foreach (var cell in plantGenContext.PlantData.GenCells)
            {
                gizmosDebugInfo.CenterPoints.Add(cell.Center);
            }

            foreach (var cell in plantGenContext.PlantData.GenCells)
            {
                gizmosDebugInfo.Borders.AddRange(cell.Borders);
            }
            gizmosDebugInfo.AreaSize = plantGenContext.InitData.AreaSize;
        }
        

        public override void Init(UnitEntity unitEntity)
        {
            unitEntity.LogicWorld().GizmosDebugUnitEntity = unitEntity;
        }

        public override void Destroy(UnitEntity unitEntity)
        {
        }
    }
}