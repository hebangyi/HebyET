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
            clientUpdateLogicComponent.AddFixedUpdateHandler(self.DoFixedUpdate);
        }

        [EntitySystem]
        private static void Update(this MyPlayerCacheDataComponent self)
        {
            self.UpdateView();
        }

        private static void DoFixedUpdate(this MyPlayerCacheDataComponent self, long deltaTime)
        {
            var unitEntity = self.GetParent<UnitEntity>();
            
            Log.Info($"DoFixedUpdate : {deltaTime}");
            if (self.IsMoving)
            {
                int towardAngle = self.OperaAngel - self.CameraAngleOffSet;
                self.TowardAngle = towardAngle;

                var unitEntityInfo = unitEntity.GetUnitEntityElemData<UnitEntityInfo>();
                var speed = unitEntityInfo.Speed;

                var atan2 = towardAngle / GameConstant.Rad2Deg;
                var deltaX = Math.Cos(atan2) * speed;
                var deltaY = Math.Sin(atan2) * speed;
                
                deltaX = deltaX * deltaTime / 1000;
                deltaY = deltaY * deltaTime / 1000;
                
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