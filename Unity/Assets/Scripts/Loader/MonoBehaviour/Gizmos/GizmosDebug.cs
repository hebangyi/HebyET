using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class GizmosDebug: MonoBehaviour
    {
        public static GizmosDebug Instance { get; private set; }
        
        
        public List<GizmosLine> Lines = new List<GizmosLine>();
        public List<Vector3> Points = new List<Vector3>();
        public List<Vector3> AOICells = new List<Vector3>();

        public int AreaSize;
        
        
        private void Awake()
        {
            Instance = this;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            foreach (var line in Lines)
            {
                Gizmos.DrawLine(line.StartPoint, line.EndPoint);
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
            
            
            Gizmos.color = Color.magenta;
            foreach (var aoiCell in AOICells)
            {
                Gizmos.DrawWireCube(aoiCell, new Vector3(GameConstant.AOICellSize, GameConstant.AOICellSize, 0));    
            }
        }
    }
}