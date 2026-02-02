using UnityEngine;

namespace ET.Client
{
    [ClientUnitEntityContext(UETypeEnum.GizmosDebug)]
    public class GizmosDebugClientContext : BaseClientUnitEntityContext
    {
        public override void CreateView(UnitEntity unitEntity)
        {
            base.CreateView(unitEntity);
            
            var unitEntityGameObjectComponent = unitEntity.GetComponent<UnitEntityGameObjectComponent>();
            var go = unitEntityGameObjectComponent.GameObject;

            var gizmosDebug = go.GetComponent<GizmosDebug>();
            var gizmosDebugInfo = unitEntity.GetUnitEntityElemData<GizmosPlantInfo>();
            if (gizmosDebugInfo == null)
            {
                return;
            }

            foreach (var border in gizmosDebugInfo.Borders)
            {
                Vector3 startPoint = new Vector3();
                Vector3 endPoint = new Vector3();

                startPoint.x = (float)border.x;
                startPoint.y = (float)border.y;

                endPoint.x = (float)border.z;
                endPoint.y = (float)border.w;

                GizmosLine gizmosLine = new GizmosLine(startPoint, endPoint);
                gizmosDebug.Lines.Add(gizmosLine);
            }

            foreach (var centerPoint in gizmosDebugInfo.CenterPoints)
            {
                gizmosDebug.Points.Add(new Vector3() { x = (float)centerPoint.x, z = (float)centerPoint.y });
            }

            gizmosDebug.AreaSize = gizmosDebugInfo.AreaSize;
        }
    }
}
