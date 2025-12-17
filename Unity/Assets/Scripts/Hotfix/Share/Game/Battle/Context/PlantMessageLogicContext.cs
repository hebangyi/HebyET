namespace ET
{
    [LogicUnitEntityContext(UELayerTypeEnum.Plant, UETypeEnum.PlantMessage)]
    public class PlantMessageLogicContext: BaseLogicUnitEntityContext
    {
        public override void InitCustomData(UnitEntity unitEntity)
        {
            var playerInitContext = unitEntity.GetComponent<UnitEntityInitContext>();
            var plantGenContext = playerInitContext.Params as PlantGenContext;
            
            var unitEntityMapMessage = unitEntity.CreateUnitEntityElemData<UnitEntityMapMessage>();
            unitEntityMapMessage.AreaSize = plantGenContext.InitData.AreaSize;
            
            PlantInfo plantInfo = PlantInfo.Create();
            foreach (var cell in plantGenContext.PlantData.RealCells)
            {
                var cellInfo = CellInfo.Create();
                cellInfo.CenterPoint = cell.Center;
                cellInfo.Borders.AddRange(cell.Borders);
                plantInfo.CellInfos.Add(cellInfo);
            }
            unitEntityMapMessage.PlantInfo = plantInfo;
        }

        public override void Init(UnitEntity unitEntity)
        {
            unitEntity.LogicWorld().PlantMessageUnitEntity = unitEntity;
        }

        public override void Destroy(UnitEntity unitEntity)
        {
            unitEntity.LogicWorld().PlantMessageUnitEntity = null;
        }
    }
}