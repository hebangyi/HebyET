using Unity.Mathematics;
using UnityEngine;

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
                case UnitEntityTypeEnum.Plant:
                {
                    CreatePlant(unitEntity).Coroutine();
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
            go.transform.position = new Vector3(unitEntityPosition.Position.x, unitEntityPosition.Position.y, unitEntityPosition.Position.z);
        }


        public async ETTask CreatePlant(UnitEntity unitEntity)
        {
            var plantCellInfo = unitEntity.GetUnitEntityElemData<PlantCellInfo>();
            if (plantCellInfo == null)
            {
                return;
            }
            
            string assetsName = $"Assets/Bundles/Unit/Unit.prefab";
            GameObject bundleGameObject = await unitEntity.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(assetsName);
            GameObject plantGameObject = bundleGameObject.Get<GameObject>("Plant");
            GameObject go = UnityEngine.Object.Instantiate(plantGameObject, GlobalComponent.Instance.Unit, true);
            var meshFilter = go.GetComponent<MeshFilter>();
            if (meshFilter == null)
            {
                Log.Error($"{assetsName} 没有找到 GameObject");
                return;
            }
            
            Mesh mesh = new Mesh();
            int[] triangles = new int[plantCellInfo.plantEdges.Count * 3];
            // 点
            Vector3[] vertices = new Vector3[2 * plantCellInfo.plantEdges.Count + 1] ;
            vertices[2 * plantCellInfo.plantEdges.Count] = new Vector3(plantCellInfo.Center.x, 0, plantCellInfo.Center.y);
            for (int i = 0; i < plantCellInfo.plantEdges.Count; i++)
            {
                var edge = plantCellInfo.plantEdges[i];
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
        }
    }    
}
