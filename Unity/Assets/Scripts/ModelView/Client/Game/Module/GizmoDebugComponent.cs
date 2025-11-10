using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class GizmoDebugComponent: Entity, IAwake
    {
        public static GizmoDebugComponent Instance;
    }    
}
