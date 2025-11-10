using System;
using Unity.Mathematics;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(MyPlayerCacheDataComponent))]
    [FriendOf(typeof(MyPlayerCacheDataComponent))]
    public static partial class MyPlayerCacheDataComponentSystem
    {
        [EntitySystem]
        private static void Awake(this MyPlayerCacheDataComponent self)
        {
            // self.GetParent<UnitEntity>()
        }
        
        [EntitySystem]
        private static void Update(this MyPlayerCacheDataComponent self)
        {
            var time = TimeInfo.Instance.NowMillTime();
            var unitEntity = self.GetParent<UnitEntity>();

            if (self.LastExecuteClientTime == 0)
            {
                self.LastExecuteClientTime = time;
            }

            if (time == self.LastExecuteClientTime)
            {
                return;
            }
            
            if (self.IsMoving)
            {
                int towardAngle = self.OperaAngel - self.CameraAngleOffSet;
                self.TowardAngle = towardAngle;

                var unitEntityInfo = unitEntity.GetUnitEntityElemData<UnitEntityInfo>();
                var speed = unitEntityInfo.Speed;

                var atan2 = towardAngle / GameConstant.Rad2Deg;
                var deltaX = Math.Cos(atan2) * speed;
                var deltaY = Math.Sin(atan2) * speed;
                
                var subTime = time - self.LastExecuteClientTime;
                deltaX = deltaX * subTime / 1000;
                deltaY = deltaY * subTime / 1000;
                
                self.Position += new float2((float)deltaX, (float)deltaY);
            }
            
            
            self.LastExecuteClientTime = time;
            self.UpdateView();
        }

        private static void FixedUpdate(this MyPlayerCacheDataComponent self, long deltaTime)
        {
            
            
            
        }
        

        private static void UpdateView(this MyPlayerCacheDataComponent self)
        {
            var unitEntity = self.GetParent<UnitEntity>();

            var unitEntityGameObjectComponent = unitEntity.GetComponent<UnitEntityGameObjectComponent>();
            if (unitEntityGameObjectComponent == null)
            {
                return;
            }

            var gameObject = unitEntityGameObjectComponent.GameObject;
            gameObject.transform.position =
                    new Vector3(self.Position.x, 0, self.Position.y);
            
        }
    }
}