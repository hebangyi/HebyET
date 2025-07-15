using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class DynamicLandGenerator : MonoBehaviour
    {
        public int sides = 5;        // 多边形边数
        public float radius = 5f;    // 地块半径
        public float noiseStrength = 1f; // 形状随机强度
        
        
        // Start is called before the first frame update
        void Start()
        {
            GenerateLand();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        
        public void GenerateLand()
        {
            Mesh mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;

            // 1. 生成顶点
            Vector3[] vertices = new Vector3[sides];
            for (int i = 0; i < sides; i++)
            {
                float angle = i * 2 * Mathf.PI / sides;
            
                // 添加随机扰动
                float noise = Random.Range(-noiseStrength, noiseStrength);
                float r = radius + noise;
            
                vertices[i] = new Vector3(
                    r * Mathf.Cos(angle),
                    0,
                    r * Mathf.Sin(angle)
                );
            }

            // 2. 三角剖分（扇形分割）
            int[] triangles = new int[(sides - 2) * 3];
            for (int i = 0; i < sides - 2; i++)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }

            // 3. 设置网格数据
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();

            // 4. 添加碰撞体
            gameObject.AddComponent<MeshCollider>();
        }
    }
}
