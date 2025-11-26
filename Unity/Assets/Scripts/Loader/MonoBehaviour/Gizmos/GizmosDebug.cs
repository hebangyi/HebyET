using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class GizmosDebug: MonoBehaviour
    {
        public static GizmosDebug Instance { get; private set; }
        
        
        public List<GizmosLine> Lines = new List<GizmosLine>();
        public List<GizmosWireSphere> Spheres = new List<GizmosWireSphere>();
        public List<Vector3> Points = new List<Vector3>();
        

        public int AreaSize;
        
        
        private void Awake()
        {
            Instance = this;
            
            GizmosWireSphere wireSphere = new();
            wireSphere.center = new Vector3(0, 0, 0f);
            wireSphere.radius = GameConstant.AOIWatchRadius;
            
            Spheres.Add(wireSphere);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            foreach (var line in Lines)
            {
                Gizmos.DrawLine(line.StartPoint, line.EndPoint);
            }

            Gizmos.color = Color.blue;
            foreach (var sphere in Spheres)
            {
                Gizmos.DrawWireSphere(sphere.center, sphere.radius);    
            }
            
            Gizmos.color = Color.green;
            for (int i = 0; i <= this.AreaSize; i += GameConstant.AOICellSize)
            {
                Gizmos.DrawLine(new Vector3(i, 0, 0), new Vector3(i, this.AreaSize, 0));
            }
            
            for (int i = 0; i <= this.AreaSize; i += GameConstant.AOICellSize)
            {
                Gizmos.DrawLine(new Vector3(0, i, 0), new Vector3(this.AreaSize, i, 0));
            }
        }
    }
}