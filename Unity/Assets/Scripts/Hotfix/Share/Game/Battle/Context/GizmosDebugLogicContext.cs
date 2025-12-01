namespace ET
{
    [LogicUnitEntityContext(UETypeEnum.GizmosDebug)]
    public class GizmosDebugLogicContext: ILogicUnitEntityContext
    {
        public void InitElementData(UnitEntity unitEntity)
        {
            var playerInitContext = unitEntity.GetComponent<PlaneInitContext>();
            var plantGenContext = playerInitContext.PlantGenContext;
            
            // 计算总的Cell数量
            var unitEntityCommonData1 = unitEntity.CreateUnitEntityElemData<UnitEntityCommonData>();
            unitEntityCommonData1.UnitEntityType = UETypeEnum.GizmosDebug;
            unitEntityCommonData1.UELayerTypeEnum = UELayerTypeEnum.Plant;
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

        public void InitLogicElementData(UnitEntity unitEntity)
        {
        }

        public void Init(UnitEntity unitEntity)
        {
            unitEntity.LogicWorld().GizmosDebugUnitEntity = unitEntity;
        }

        public void Destroy(UnitEntity unitEntity)
        {
        }
    }
}