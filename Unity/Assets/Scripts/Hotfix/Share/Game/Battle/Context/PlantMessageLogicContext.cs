namespace ET
{
    [LogicUnitEntityContext(UETypeEnum.PlantMessage)]
    public class PlantMessageLogicContext: ILogicUnitEntityContext
    {
        public void InitElementData(UnitEntity unitEntity)
        {
            var playerInitContext = unitEntity.GetComponent<PlaneInitContext>();
            
            var plantGenContext = playerInitContext.PlantGenContext;
            var commonData = unitEntity.CreateUnitEntityElemData<UnitEntityCommonData>();
            commonData.UnitEntityType = UETypeEnum.PlantMessage;
            
            var unitEntityMapMessage = unitEntity.CreateUnitEntityElemData<UnitEntityMapMessage>();
            unitEntityMapMessage.AreaSize = plantGenContext.InitData.AreaSize;
            
            PlantInfo plantInfo = PlantInfo.Create();
            foreach (var cell in plantGenContext.PlantData.GenCells)
            {
                var cellInfo = CellInfo.Create();
                cellInfo.CenterPoint = cell.Center;
                cellInfo.Borders.AddRange(cell.Borders);
                plantInfo.CellInfos.Add(cellInfo);
            }
            unitEntityMapMessage.PlantInfo = plantInfo;
        }

        public void InitLogicElementData(UnitEntity unitEntity)
        {
        }

        public void Init(UnitEntity unitEntity)
        {
            unitEntity.LogicWorld().PlantMessageUnitEntity = unitEntity;
        }

        public void Destroy(UnitEntity unitEntity)
        {
            unitEntity.LogicWorld().PlantMessageUnitEntity = null;
        }
    }
}