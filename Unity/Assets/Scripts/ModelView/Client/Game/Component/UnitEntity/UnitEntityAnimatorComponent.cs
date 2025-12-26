using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(UnitEntity))]
    public class UnitEntityAnimatorComponent : Entity, IAwake
    {
        private GameObject gameObject;
    }
}