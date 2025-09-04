using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace ET
{
    public class GizmosLine
    {
        public Vector3 StartPoint;
        public Vector3 EndPoint;

        public GizmosLine(Vector3 startPoint, Vector3 endPoint)
        {
            this.StartPoint = startPoint;
            this.EndPoint = endPoint;
        }
    }
}
