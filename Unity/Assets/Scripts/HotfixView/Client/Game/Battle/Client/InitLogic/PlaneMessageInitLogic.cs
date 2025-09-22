using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ET.Client
{
    [UnitEntityViewLogic]
    public class PlaneMessageLogicInitLogic  : IClientWorldLogicInitLogic
    {
        public void OnInit(UnitEntity unitEntity)
        {
            unitEntity.ClientWorld().UnitEntityMap = unitEntity;

            var unitEntityGameObjectComponent = unitEntity.GetComponent<UnitEntityGameObjectComponent>();
            var go = unitEntityGameObjectComponent.GameObject;
            
            var groundTile = go.Get<TileBase>("Ground");
            var tilemap = go.Get<GameObject>("TileMap").GetComponent<Tilemap>();

            var unitEntityMapMessage = unitEntity.GetUnitEntityElemData<UnitEntityMapMessage>();
            var areaSize = unitEntityMapMessage.AreaSize;
            var unitSize = 1f;
            var unitRadius  = unitSize / 2;
            
            var plantInfo = unitEntityMapMessage.PlantInfo;
            long titleMapCount = 0;
            
            /*foreach (var cellInfo in plantInfo.CellInfos)
            {
                float plantMinX = float.MaxValue;
                float plantMinY = float.MaxValue;
                float plantMaxX = 0;
                float plantMaxY = 0;
                
                foreach (var border in cellInfo.Borders)
                {
                    if (border.x > plantMaxX){
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
                
                var formX = (int)(plantMinX / unitSize) * unitSize + unitRadius;
                var toX = (int)(plantMaxX / unitSize) * unitSize + unitRadius;
                var formY = (int)(plantMinY / unitSize) * unitSize + unitRadius;
                var toY = (int)(plantMaxY / unitSize) * unitSize + unitRadius;

                for (var x = formX; x <= toX; x+= unitSize)
                {
                    for (var y = formY; y <= toY; y+= unitSize)
                    {
                        if (BattleMapHelper.IsPointInPolygon(new float2(x, y), cellInfo.Borders))
                        {
                            Vector3Int position = new Vector3Int((int)x, (int)y, 0);
                            tilemap.SetTile(position, groundTile);
                            titleMapCount++;
                        }
                    }
                }

                break;
            }*/
            
            Log.Error($"创建 TileMap 数量 : {titleMapCount}");
            
            /*
            PlantData plantInfo = BattleMapHelper.GenPlantInfo(unitEntityMapMessage.PlantInfo);
            for (int x = 0; x < plantInfo.xPlantCount; x++)
            {
                for (int y = 0; y < plantInfo.yPlantCount; y++)
                {
                    var target = x * plantInfo.xPlantCount + y;
                    if (plantInfo.mapData[target])
                    {
                        Vector3Int position = new Vector3Int(x, y, 0);
                        tilemap.SetTile(position, groundTile);
                    }
                }
            }
            tilemap.RefreshAllTiles();
            */
            
        }

        public void OnDestroy(UnitEntity unitEntity)
        {
            unitEntity.ClientWorld().UnitEntityMap = null;
        }

        public ushort WatchComponentId()
        {
            return OpcodeType.Instance.GetOpcode(typeof(UnitEntityMapMessage));
        }
    }    
}
