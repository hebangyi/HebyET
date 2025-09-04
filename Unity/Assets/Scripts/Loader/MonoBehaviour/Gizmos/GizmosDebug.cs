using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class GizmosDebug: MonoBehaviour
    {
        public static GizmosDebug Instance { get; private set; }
        
        public Color lineColor = Color.red;
        
        public List<GizmosLine> Lines = new List<GizmosLine>();

        private void Awake()
        {
            Instance = this;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = lineColor;
            foreach (var line in Lines)
            {
                Gizmos.DrawLine(line.StartPoint, line.EndPoint);
            }
        }
    }
}