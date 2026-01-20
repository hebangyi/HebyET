using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(UnitEntity))]
    public class UnitEntitySpineAnimationComponent : Entity, IAwake<GameObject>
    {
        public GameObject GameObject { get; set; }

        public SkeletonAnimation SkeletonAnimation { get; set; }
        
        // 动画对应的播放时间
        public Dictionary<string, float> AnimationName2DurationSec = new ();
    }
}