using System;
using System.Linq;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace ET.Client
{
   [FriendOf(typeof(UnitEntitySpineAnimationComponent))]
   [EntitySystemOf(typeof(UnitEntitySpineAnimationComponent))]
   public static partial class UnitEntitySpineAnimationComponentSystem
   {
       
       [EntitySystem]
       private static void Awake(this UnitEntitySpineAnimationComponent self, GameObject gameObject)
       {
           GameObject spineAnimationObj = gameObject.Get<GameObject>("SpineAnimation");
           if (spineAnimationObj == null)
           {
               return;
           }
           
           
           self.GameObject = gameObject;
           self.SkeletonAnimation = spineAnimationObj.GetComponent<SkeletonAnimation>();
           
           foreach (var animation in self.SkeletonAnimation.AnimationState.Data.SkeletonData.Animations)
           {
               var name = animation.Name;
               var duration = animation.Duration;

               self.AnimationName2DurationSec[name] = duration;
           }
       }

       public static bool HasAnimation(this UnitEntitySpineAnimationComponent self, string name)
       {
           return self.SkeletonAnimation.AnimationState.Data.SkeletonData.Animations.Any(x => x.Name == name);
       }

       public static Spine.Animation GetAnimationByName(this UnitEntitySpineAnimationComponent self, string name)
       {
           return self.SkeletonAnimation.AnimationState.Data.SkeletonData.Animations.FirstOrDefault(x => x.Name == name);
       }

       public static void SetAnimationAtTime(this UnitEntitySpineAnimationComponent self, string name, float time, bool isLoop = true)
       {
           var animation = self.GetAnimationByName(name);
           if (animation == null)
           {
               Log.Error($"设置Spine 动画错误! {self.GetParent<UnitEntity>().InsId} 找不到动画 {name}");
               return;
           }

           var startTime = Math.Min(time, animation.Duration);
           self.SkeletonAnimation.state.SetAnimation(0, name, isLoop);
           // 设置轨道
           TrackEntry currentTrack = self.SkeletonAnimation.AnimationState.GetCurrent(0);
           if (currentTrack != null)
           {
               currentTrack.TrackTime = startTime;
           }
       }

       public static void SetAnimationTimeScla(this UnitEntitySpineAnimationComponent self, float timeScale)
       {
           if (self.SkeletonAnimation == null)
           {
               return;
           }

           self.SkeletonAnimation.timeScale = timeScale;
       }
   }
}