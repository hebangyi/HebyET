using Spine.Unity;
using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(UnitEntity))]
    public class UnitEntityGameObjectComponent : Entity, IAwake<GameObject>, IDestroy
    {
        
        private GameObject gameObject;

        public GameObject GameObject 
        {
            get
            {
                return this.gameObject;
            }
            set
            {
                this.gameObject = value;
                this.Transform = value.transform;
            }
        }

        public Transform Transform { get; private set; }
    }
}
