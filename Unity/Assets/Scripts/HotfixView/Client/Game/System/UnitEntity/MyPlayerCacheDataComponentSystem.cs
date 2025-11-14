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
            UpdateLogicManagerComponent.Instance.AddTaskUpdateFunc(self.SyncData);
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
        
        private static async ETTask SyncData(this MyPlayerCacheDataComponent self)
        {
            try
            {
                BattleUnitEntity battleUnitEntity = BattleUnitEntity.Create(true);

                var unitEntity = self.GetParent<UnitEntity>();
                battleUnitEntity.InsId = unitEntity.InsId;
                
                var unitEntityPosition = unitEntity.GetUnitEntityElemData<UnitEntityPosition>();
                var unitEntityCameraData = unitEntity.GetUnitEntityElemData<UnitEntityCameraData>();
                var unitEntityPlayerAnimateStatus = unitEntity.GetUnitEntityElemData<UnitEntityPlayerAnimateStatus>();

                if (!unitEntityPosition.Position.Equals(self.Position))
                {
                    // 将缓存坐标更新到ElemData
                    unitEntityPosition.Position = self.Position;
                    
                    ushort compId = OpcodeType.Instance.GetOpcode(typeof(UnitEntityPosition));
                    var unitEntityElemData = UnitEntityElemData.Create();
                    unitEntityElemData.CompId = compId;
                    unitEntityElemData.ElemDatas = MemoryPackHelper.Serialize(unitEntityPosition);
                    battleUnitEntity.EleDatas.Add(unitEntityElemData);
                }

                if (unitEntityCameraData.CameraAngleOffSet != (short)self.CameraAngleOffSet)
                {
                    // 将缓存坐标更新到ElemData
                    unitEntityCameraData.CameraAngleOffSet = (short)self.CameraAngleOffSet;
                    
                    ushort compId = OpcodeType.Instance.GetOpcode(typeof(UnitEntityCameraData));
                    var unitEntityElemData = UnitEntityElemData.Create();
                    unitEntityElemData.CompId = compId;
                    unitEntityElemData.ElemDatas = MemoryPackHelper.Serialize(unitEntityCameraData);
                    battleUnitEntity.EleDatas.Add(unitEntityElemData);
                }
                
                if (unitEntityPlayerAnimateStatus.Status != self.PlayerAnimateStatusEnum)
                {
                    unitEntityPlayerAnimateStatus.Status = self.PlayerAnimateStatusEnum;
                    
                    ushort compId = OpcodeType.Instance.GetOpcode(typeof(UnitEntityPlayerAnimateStatus));
                    var unitEntityElemData = UnitEntityElemData.Create();
                    unitEntityElemData.CompId = compId;
                    unitEntityElemData.ElemDatas = MemoryPackHelper.Serialize(unitEntityCameraData);
                    battleUnitEntity.EleDatas.Add(unitEntityElemData);
                }

                if (battleUnitEntity.EleDatas.Count > 0)
                {
                    var clientBattleSenderComponent = ClientBattleSenderComponent.Instance;
                    C2B_PlayerUpdateDirtyElemData request = C2B_PlayerUpdateDirtyElemData.Create();
                    request.BattleUnitEntity = battleUnitEntity;
                    B2C_PlayerUpdateDirtyElemData response = (B2C_PlayerUpdateDirtyElemData)await clientBattleSenderComponent.Call(request);
                }
                
                battleUnitEntity.Dispose();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}