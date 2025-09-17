using UnityEngine;
using UnityEngine.Tilemaps;

namespace ET.Client
{
    [UnitEntityViewLogic]
    public class PlaneMessageInitLogic  : IUnitEntityClientWorldInitLogic
    {
        public void OnInit(UnitEntity unitEntity)
        {
            unitEntity.ClientWorld.UnitPlant = unitEntity;

            var unitEntityGameObjectComponent = unitEntity.GetComponent<UnitEntityGameObjectComponent>();
            var go = unitEntityGameObjectComponent.GameObject;
            
            var groundTile = go.Get<TileBase>("Ground");
            var tilemap = go.Get<GameObject>("TileMap").GetComponent<Tilemap>();

            var unitEntityMapMessage = unitEntity.GetUnitEntityElemData<UnitEntityMapMessage>();
            PlantData plantInfo = BattleMapHelper.GenPlantInfo(unitEntityMapMessage.PlantInfo);
            for (int x = 0; x < plantInfo.xPlantCount; x++)
            {
                for (int y = 0; y < plantInfo.yPlantCount; y++)
                {
                    var target = x * plantInfo.xPlantCount + y;
                    /*if (plantInfo.mapData[target])
                    {
                        Vector3Int position = new Vector3Int(x, y, 0);
                        tilemap.SetTile(position, groundTile);
                    }*/
                }
            }
            tilemap.RefreshAllTiles();
        }

        public void OnDestroy(UnitEntity unitEntity)
        {
            unitEntity.ClientWorld.UnitPlant = null;
        }

        public ushort WatchComponentId()
        {
            return OpcodeType.Instance.GetOpcode(typeof(UnitEntityMapMessage));
        }
    }    
}
