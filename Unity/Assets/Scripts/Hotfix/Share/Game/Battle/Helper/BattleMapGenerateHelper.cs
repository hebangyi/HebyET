using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;

namespace ET
{
    public static class BattleMapGenerateHelper
    {
        public static void GenerateMethods1(PlantGenContext plantGenContext)
        {
            var initData = plantGenContext.InitData;
            var plantData = plantGenContext.PlantData;
            var areaSize = initData.AreaSize;
            var cellDataList = plantData.AllCells;
            var genCells = plantData.GenCells;
         
            Dictionary<int, GenCellData> allGenCellDataDict = new ();
            HashSet<int> generateCellIds = new HashSet<int>();
            
            // 生成所有的GenCellData
            foreach (var cellData in cellDataList)
            {
                GenCellData genCellData = new ();
                genCellData.Id = cellData.Id;
                genCellData.IsInMap = false;
                genCellData.CellData = cellData;

                foreach (var nearCellData in cellData.NearCellDataSet)
                {
                    genCellData.NearCellIds.Add(nearCellData.Id);
                }
                
                allGenCellDataDict[cellData.Id] = genCellData;
            }
            
            // 找到距离中心比较近的地块
            CellData centerCellData = null;
            float minDistance = float.MaxValue;
            float2 centerPoint = new float2((float)areaSize/ 2, (float)areaSize/2);
            foreach (var cell in cellDataList)
            {
                if (centerCellData == null)
                {
                    centerCellData = cell;
                    minDistance = (cell.Center.x - centerPoint.x) * (cell.Center.x - centerPoint.x) + (cell.Center.y - centerPoint.y) * (cell.Center.y - centerPoint.y);
                    continue;
                }
                
                var distance = (cell.Center.x - centerPoint.x) * (cell.Center.x - centerPoint.x) + (cell.Center.y - centerPoint.y) * (cell.Center.y - centerPoint.y);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    centerCellData = cell;
                }
            }

            var centerGenCellData = allGenCellDataDict.GetValueOrDefault(centerCellData.Id);
            centerGenCellData.IsInMap = true;
            generateCellIds.Add(centerCellData.Id);
            
            // 在总地块中生成地块
            Dictionary<int, GenCellData> nextGenCellDataDict = new ();
            List<int> weightList = new List<int>();
            for (int i = 0; i < initData.GenCellCount -1; i++)
            {
                nextGenCellDataDict.Clear();
                weightList.Clear();
                
                foreach (var genCellId in generateCellIds)
                {
                    var genCellData = allGenCellDataDict.GetValueOrDefault(genCellId);
                    foreach (var nearCellId in genCellData.NearCellIds)
                    {
                        if (generateCellIds.Contains(nearCellId))
                        {
                            continue;
                        }
                        
                        var nearCellData = allGenCellDataDict.GetValueOrDefault(nearCellId);
                        if (nearCellData.IsInMap)
                        {
                            continue;
                        }
                        
                        nextGenCellDataDict[nearCellId] = nearCellData;
                    }
                }
                
                var nextCells = nextGenCellDataDict.Values.ToList();
                
                for(int z = 0; z < nextCells.Count; z++)
                {
                    var nextGenCellData = nextCells[z];
                    int inMapCellCount = 0;

                    foreach (var nearCellId in nextGenCellData.NearCellIds)
                    {
                        var nearCellData = allGenCellDataDict.GetValueOrDefault(nearCellId);
                        if(nearCellData.IsInMap)
                        {
                            inMapCellCount++;
                        }
                    }

                    int weight = 0;
                    if (inMapCellCount == 1)
                    {
                        weight = 50;
                    }else if (inMapCellCount == 2)
                    {
                        weight = 30;
                    }
                    else
                    {
                        weight = 20;
                    }
                    weightList.Add(weight);
                }

                var index = RandomHelper.RandomByWeight(weightList);
                var nextCell = nextCells[index];
                nextCell.IsInMap = true;
                generateCellIds.Add(nextCell.Id);
            }


            foreach (var generateCellId in generateCellIds)
            {
                var genCellData = allGenCellDataDict.GetValueOrDefault(generateCellId);
                genCells.Add(genCellData.CellData);
            }
        }
    }
}
