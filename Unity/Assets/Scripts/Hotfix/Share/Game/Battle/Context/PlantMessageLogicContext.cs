using System;
using Unity.Mathematics;

namespace ET
{
    [LogicUnitEntityContext(UELayerTypeEnum.Plant, UETypeEnum.PlantMessage)]
    public class PlantMessageLogicContext : BaseLogicUnitEntityContext
    {
        public override void InitCustomData(UnitEntity unitEntity)
        {
            var playerInitContext = unitEntity.GetComponent<UnitEntityInitContext>();
            var plantGenContext = playerInitContext.Params as PlantGenContext;

            var unitEntityMapMessage = unitEntity.CreateUnitEntityElemData<UnitEntityMapMessage>();
            unitEntityMapMessage.AreaSize = plantGenContext.InitData.AreaSize;

            var plantRuntimeData = unitEntity.CreateUnitEntityLogicElemData<PlantRuntimeData>();
            int unitSize = BattleGlobalConfigCategory.Instance.Config.TileMapUnitSize;
            int unitRadius = unitSize / 2;

            plantRuntimeData.TitleMaps = new bool[plantGenContext.PlantData.MaxXTileCount, plantGenContext.PlantData.MaxYTileCount];
            plantRuntimeData.TileMapUnitSize = BattleGlobalConfigCategory.Instance.Config.TileMapUnitSize;
            plantRuntimeData.MaxXTileCount = plantGenContext.PlantData.MaxXTileCount;
            plantRuntimeData.MaxYTileCount = plantGenContext.PlantData.MaxYTileCount;

            PlantInfo plantInfo = PlantInfo.Create();
            foreach (var cell in plantGenContext.PlantData.RealCells)
            {
                var cellInfo = CellInfo.Create();
                cellInfo.CenterPoint = cell.Center;
                cellInfo.Borders.AddRange(cell.Borders);
                plantInfo.CellInfos.Add(cellInfo);
            }

            unitEntityMapMessage.PlantInfo = plantInfo;

            for (int i = 0; i < plantInfo.CellInfos.Count; i++)
            {
                var cellInfo = plantInfo.CellInfos[i];
                float plantMinX = float.MaxValue;
                float plantMinY = float.MaxValue;
                float plantMaxX = 0;
                float plantMaxY = 0;

                foreach (var border in cellInfo.Borders)
                {
                    if (border.x > plantMaxX)
                    {
                        plantMaxX = border.x;
                    }

                    if (border.x < plantMinX)
                    {
                        plantMinX = border.x;
                    }

                    if (border.y > plantMaxY)
                    {
                        plantMaxY = border.y;
                    }

                    if (border.y < plantMinY)
                    {
                        plantMinY = border.y;
                    }

                    if (border.z > plantMaxX)
                    {
                        plantMaxX = border.z;
                    }

                    if (border.z < plantMinX)
                    {
                        plantMinX = border.z;
                    }

                    if (border.w > plantMaxY)
                    {
                        plantMaxY = border.w;
                    }

                    if (border.w < plantMinY)
                    {
                        plantMinY = border.w;
                    }
                }

                var formTileX = (int)(plantMinX / unitSize);
                var toTileX = (int)(plantMaxX / unitSize) + 1;
                var formTileY = (int)(plantMinY / unitSize);
                var toTileY = (int)(plantMaxY / unitSize) + 1;

                for (var xTile = formTileX; xTile < toTileX; xTile++)
                {
                    for (var yTile = formTileY; yTile < toTileY; yTile++)
                    {
                        var centerX = xTile * unitSize + unitRadius;
                        var centerY = yTile * unitSize + unitRadius;

                        if (BattleMapHelper.IsPointInPolygon(new float2(centerX, centerY), cellInfo.Borders))
                        {
                            plantRuntimeData.TitleMaps[xTile, yTile] = true;
                        }
                    }
                }
            }
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