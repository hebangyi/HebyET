using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ET.Client
{
    [BattleEvent(WorldMode.View)]
    public class CreateUnitEntityEvent1_CreateGameObject: ABattleEvent<CreateUnitEntityEvent1>
    {
        protected override void Run(World world, CreateUnitEntityEvent1 args)
        {
            var unitEntity = args.UnitEntity;

            var unitEntityCommonData = unitEntity.GetUnitEntityElemData<UnitEntityCommonData>();

            if (unitEntityCommonData == null)
            {
                Log.Error("创建 GameObject 错误, 找不到 UnitEntity UnitEntityCommonData");
                return;
            }
            
            switch (unitEntityCommonData.UnitEntityType)
            {
                case UnitEntityTypeEnum.Player:
                {
                    CreatePlayer(unitEntity).Coroutine();
                    break;
                }
                case UnitEntityTypeEnum.PlantMessage:
                {
                    this.CreatePlantMessage(unitEntity).Coroutine();
                    break;
                }
                case UnitEntityTypeEnum.GizmosDebug:
                {
                    this.CreateGizmosDebug(unitEntity).Coroutine();
                    break;
                }
            }
        }
        
        
        public async ETTask CreatePlayer(UnitEntity unitEntity)
        {
            string assetsName = $"Assets/Bundles/Unit/Unit.prefab";
            GameObject bundleGameObject = await unitEntity.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(assetsName);
            GameObject playerGameObject = bundleGameObject.Get<GameObject>("Skeleton");

            GameObject go = UnityEngine.Object.Instantiate(playerGameObject, GlobalComponent.Instance.Unit, true);
            var unitEntityPosition = unitEntity.GetUnitEntityElemData<UnitEntityPosition>();

            var unitEntityGameObjectComponent = unitEntity.TryAddComponent<UnitEntityGameObjectComponent>();
            unitEntityGameObjectComponent.GameObject = go;
            go.name = $"Player_{unitEntity.InsId}";
            go.transform.position = new Vector3(unitEntityPosition.Position.x, 0, unitEntityPosition.Position.y);
        }


        public async ETTask CreatePlane(UnitEntity unitEntity)
        {/*
            var plantCellInfo = unitEntity.GetUnitEntityElemData<PlaneCellInfo>();
            if (plantCellInfo == null)
            {
                return;
            }
            
            string assetsName = $"Assets/Bundles/Unit/Unit.prefab";
            GameObject bundleGameObject = await unitEntity.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(assetsName);
            GameObject planeGameObject = bundleGameObject.Get<GameObject>("Plane");
            GameObject go = UnityEngine.Object.Instantiate(planeGameObject, GlobalComponent.Instance.Unit, true);
            var meshFilter = go.GetComponent<MeshFilter>();
            if (meshFilter == null)
            {
                Log.Error($"{assetsName} 没有找到 GameObject");
                return;
            }
            
            Mesh mesh = new Mesh();
            int[] triangles = new int[plantCellInfo.PlantEdges.Count * 3];
            // 点
            Vector3[] vertices = new Vector3[2 * plantCellInfo.PlantEdges.Count + 1] ;
            vertices[2 * plantCellInfo.PlantEdges.Count] = new Vector3(plantCellInfo.Center.x, 0, plantCellInfo.Center.y);
            for (int i = 0; i < plantCellInfo.PlantEdges.Count; i++)
            {
                var edge = plantCellInfo.PlantEdges[i];
                vertices[2 * i] = new Vector3(edge.x, 0, edge.y);
                vertices[2 * i + 1] = new Vector3(edge.z, 0, edge.w);
                
                
                triangles[i * 3] = vertices.Length - 1;
                triangles[i * 3 + 1] = 2 * i;
                triangles[i * 3 + 2] = 2 * i + 1;
            }
            

            // 3. 设置网格数据
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            meshFilter.mesh = mesh;
            
            var unitEntityGameObjectComponent = unitEntity.TryAddComponent<UnitEntityGameObjectComponent>();
            unitEntityGameObjectComponent.GameObject = go;
            
            // go.transform.position = new Vector3(plantCellInfo.Center.x, 0, plantCellInfo.Center.y);
            go.name = $"Plant_{unitEntity.InsId}";
            go.SetActive(false);
            */
        }


        public async ETTask CreatePlantMessage(UnitEntity unitEntity)
        {
            Log.Error("CreatePlantMessage!!!");
            var plantCellInfo = unitEntity.GetUnitEntityElemData<UnitEntityMapMessage>();
            if (plantCellInfo == null)
            {
                return;
            }
            
            string assetsName = $"Assets/Bundles/Unit/Unit.prefab";
            GameObject bundleGameObject = await unitEntity.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(assetsName);
            GameObject planeGameObject = bundleGameObject.Get<GameObject>("PlaneMessage");
            GameObject go = UnityEngine.Object.Instantiate(planeGameObject, GlobalComponent.Instance.Unit, true);
            var unitEntityGameObjectComponent = unitEntity.TryAddComponent<UnitEntityGameObjectComponent>();
            unitEntityGameObjectComponent.GameObject = go;
            var groundTile = go.Get<TileBase>("Ground");
            var tilemap = go.Get<GameObject>("TileMap").GetComponent<Tilemap>();

            for (int x = 0; x < plantCellInfo.MapWidth; x++)
            {
                for (int y = 0; y < plantCellInfo.MapHeight; y++)
                {
                    var target = x * plantCellInfo.MapWidth + y;
                    if (plantCellInfo.MapData[target])
                    {
                        Vector3Int position = new Vector3Int(x, y, 0);
                        tilemap.SetTile(position, groundTile);    
                    }
                }
            }
            tilemap.RefreshAllTiles();
            // go.transform.position = new Vector3(plantCellInfo.Center.x, 0, plantCellInfo.Center.y);
            go.name = $"PlantMessage_{unitEntity.InsId}";
        }

        public async ETTask CreateGizmosDebug(UnitEntity unitEntity)
        {
            Log.Error("CreateGizmosDebug!!!");
            
            string assetsName = $"Assets/Bundles/Unit/Unit.prefab";
            GameObject bundleGameObject = await unitEntity.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(assetsName);
            GameObject gizmosGameObject = bundleGameObject.Get<GameObject>("GizmosDebug");
            GameObject go = UnityEngine.Object.Instantiate(gizmosGameObject, GlobalComponent.Instance.Unit, true);

            var gizmosDebug = go.GetComponent<GizmosDebug>();
            
            var gizmosDebugInfo = unitEntity.GetUnitEntityElemData<GizmosDebugInfo>();
            if (gizmosDebugInfo == null)
            {
                return;
            }

            foreach (var border in gizmosDebugInfo.Borders)
            {
                Vector3 startPoint = new Vector3();
                Vector3 endPoint = new Vector3();
                
                
                startPoint.x = (float)border.x;
                startPoint.z = (float)border.y;

                endPoint.x = (float)border.z;
                endPoint.z = (float)border.w;
                
                GizmosLine gizmosLine = new GizmosLine(startPoint, endPoint);
                gizmosDebug.Lines.Add(gizmosLine);
            }
            
            foreach (var centerPoint in gizmosDebugInfo.CenterPoints)
            {
                gizmosDebug.Points.Add(new Vector3(){x = (float)centerPoint.x, z = (float)centerPoint.y});
            }

            gizmosDebug.AreaSize = gizmosDebugInfo.AreaSize;
        }
    }    
}
