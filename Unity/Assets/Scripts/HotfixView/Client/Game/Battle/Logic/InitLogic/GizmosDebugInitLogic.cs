/*
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ET.Client
{
    [UnitEntityViewLogic]
    public class GizmosDebugEleInit: IClientEleInit
    {
        public void OnInit(UnitEntity unitEntity)
        {
            var unitEntityGameObjectComponent = unitEntity.GetComponent<UnitEntityGameObjectComponent>();
            var go = unitEntityGameObjectComponent.GameObject;
            
            
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

        public void OnDestroy(UnitEntity unitEntity)
        {
        }

        public ushort WatchComponentId()
        {
            return OpcodeType.Instance.GetOpcode(typeof(GizmosDebugInfo));
        }
    }    
}
*/
