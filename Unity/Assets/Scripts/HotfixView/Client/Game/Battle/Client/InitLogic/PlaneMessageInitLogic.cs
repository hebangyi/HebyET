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
            
            var plant_ground_green = go.Get<TileBase>("plant_ground_green");
            var plant_ground_yellow = go.Get<TileBase>("plant_ground_yellow");
            var tilemap = go.Get<GameObject>("TileMap").GetComponent<Tilemap>();

            var unitEntityMapMessage = unitEntity.GetUnitEntityElemData<UnitEntityMapMessage>();
            var areaSize = unitEntityMapMessage.AreaSize;
            int unitSize = 10;
            int unitRadius  = unitSize / 2;
            
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
                
                var formX = (int)(plantMinX / unitSize);
                var toX = (int)(plantMaxX / unitSize) + 1;
                var formY = (int)(plantMinY / unitSize);
                var toY = (int)(plantMaxY / unitSize) + 1;
                
                for (var x = formX; x < toX; x++)
                {
                    for (var y = formY; y < toY; y++)
                    {
                        var centerX = x * unitSize + unitRadius;
                        var centerY = y * unitSize + unitRadius;
                        
                        if (BattleMapHelper.IsPointInPolygon(new float2(centerX, centerY), cellInfo.Borders))
                        {
                            Vector3Int position = new Vector3Int(x, y, 0);
                            tilemap.SetTile(position, current_plant);
                            titleMapCount++;
                        }
                    }
                }
            }
            tilemap.RefreshAllTiles();
            
            Log.Error($"创建 TileMap 数量 : {titleMapCount}");
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
