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
            var clientUpdateLogicComponent = ClientUpdateLogicComponent.Instance;
        }

        [EntitySystem]
        private static void Update(this MyPlayerCacheDataComponent self)
        {
            self.UpdateLogic();
            self.UpdateView();
        }

        private static void UpdateLogic(this MyPlayerCacheDataComponent self)
        {
            long nowTime = TimeInfo.Instance.NowMillTime();
            if (self.LastUpdateTime == nowTime)
            {
                return;
            }

            long subTime = nowTime - self.LastUpdateTime;
            self.LastUpdateTime = nowTime;
            
            if (self.IsMoving)
            {
                var unitEntity = self.GetParent<UnitEntity>();
                
                float towardAngle = self.OperaAngel - self.CameraAngleOffSet % 360;
                self.TowardAngle = towardAngle;

                var unitEntityInfo = unitEntity.GetUnitEntityElemData<UnitEntityInfo>();
                var speed = unitEntityInfo.Speed;

                var atan2 = towardAngle / GameConstant.Rad2Deg;
                var deltaX = Math.Cos(atan2) * speed;
                var deltaY = Math.Sin(atan2) * speed;
                
                deltaX = deltaX * subTime / 1000;
                deltaY = deltaY * subTime / 1000;
                
                self.Position += new float2((float)deltaX, (float)deltaY);
            }
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