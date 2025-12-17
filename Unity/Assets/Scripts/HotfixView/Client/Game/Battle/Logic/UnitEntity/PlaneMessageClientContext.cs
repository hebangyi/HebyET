using NativeCollection;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ET.Client
{
    [ClientUnitEntityContext(UETypeEnum.PlantMessage)]
    public class PlaneMessageClientContext : BaseClientUnitEntityContext
    {
        public override void CreateView(UnitEntity unitEntity)
        {
            base.CreateView(unitEntity);

            unitEntity.ClientWorld().UnitEntityMap = unitEntity;
            var unitEntityGameObjectComponent = unitEntity.GetComponent<UnitEntityGameObjectComponent>();
            var go = unitEntityGameObjectComponent.GameObject;

            var plant_ground_green = go.Get<TileBase>("plant_ground_green");
            var plant_ground_yellow = go.Get<TileBase>("plant_ground_yellow");
            var plant_ground_red = go.Get<TileBase>("plant_ground_red");
            var tilemap = go.Get<GameObject>("TileMap").GetComponent<Tilemap>();
            var boarderTilemap = go.Get<GameObject>("Boarder").GetComponent<Tilemap>();
            
            List<Vector3Int> CellBoarderList = new List<Vector3Int>();
            
            var unitEntityMapMessage = unitEntity.GetUnitEntityElemData<UnitEntityMapMessage>();
            int unitSize = BattleGlobalConfigCategory.Instance.Config.TileMapUnitSize;
            int unitRadius = unitSize / 2;

            var plantInfo = unitEntityMapMessage.PlantInfo;
            long titleMapCount = 0;
            
            for (int i = 0; i < plantInfo.CellInfos.Count; i++)
            {
                var cellInfo = plantInfo.CellInfos[i];
                float plantMinX = float.MaxValue;
                float plantMinY = float.MaxValue;
                float plantMaxX = 0;
                float plantMaxY = 0;
                var current_plant = plant_ground_green;
                if (i % 2 == 0)
                {
                    current_plant = plant_ground_yellow;
                }

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

                var formX = (int)(plantMinX / unitSize) - 1;
                var toX = (int)(plantMaxX / unitSize) + 2;
                var formY = (int)(plantMinY / unitSize) - 1;
                var toY = (int)(plantMaxY / unitSize) + 2;
                
                for (var x = formX; x < toX; x++)
                {
                    for (var y = formY; y < toY; y++)
                    {
                        var centerX = x * unitSize + unitRadius;
                        var centerY = y * unitSize + unitRadius;
                        Vector3Int position = new Vector3Int(x, y, 0);
                        
                        if (BattleMapHelper.IsPointInPolygon(new float2(centerX, centerY), cellInfo.Borders))
                        {
                            tilemap.SetTile(position, current_plant);
                            titleMapCount++;
                        }
                        else
                        {
                            CellBoarderList.Add(position);
                        }
                    }
                }
            }
            
            foreach (var cellBoarder in CellBoarderList)
            {
                if (!tilemap.HasTile(cellBoarder))
                {
                    boarderTilemap.SetTile(cellBoarder, plant_ground_red);    
                }
            }

            tilemap.RefreshAllTiles();
            boarderTilemap.RefreshAllTiles();
            Log.Error($"创建 TileMap 数量 : {titleMapCount}");
        }

        public override void Destroy(UnitEntity unitEntity)
        {
            unitEntity.ClientWorld().UnitEntityMap = null;
        }
    }
}