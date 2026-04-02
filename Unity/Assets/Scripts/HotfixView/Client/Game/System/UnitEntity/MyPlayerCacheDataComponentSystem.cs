using System;
using Unity.Mathematics;
using UnityEngine;

namespace ET.Client
{
    [ClientWorldEventHandler]
    public class MyPlayerTriggerEnterEvent_StopPlayerMove : AClientWorldEvent<MyPlayerTriggerEnterEvent>
    {
        protected override async ETTask Run(ClientWorld world, MyPlayerTriggerEnterEvent args)
        {
            var mainPlayer = world.MainPlayer;
            if (mainPlayer == null)
            {
                return;
            }

            MainPlayerHelper.SetForceStop(mainPlayer, true);
        }
    }

    /*
    [ClientWorldEventHandler]
    public class MyPlayerTriggerStayEvent_StopPlayerMove : AClientWorldEvent<MyPlayerTriggerStayEvent>
    {
        protected override async ETTask Run(ClientWorld world, MyPlayerTriggerStayEvent args)
        {
            var mainPlayer = world.MainPlayer;
            if (mainPlayer == null)
            {
                return;
            }
            

            MainPlayerHelper.SetForceStop(mainPlayer, true);
        }
    }*/

    [ClientWorldEventHandler]
    public class MyPlayerTriggerExitEvent_StopPlayerMove : AClientWorldEvent<MyPlayerTriggerExitEvent>
    {
        protected override async ETTask Run(ClientWorld world, MyPlayerTriggerExitEvent args)
        {
            var mainPlayer = world.MainPlayer;
            if (mainPlayer == null)
            {
                return;
            }

            MainPlayerHelper.SetForceStop(mainPlayer, false);
        }
    }

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

            var unitEntity = self.GetParent<ClientUnitEntity>();
            if (self.IsDragging)
            {
                
                int towardAngle = (self.OperaAngel + self.CameraAngleOffSet) % 360;
                self.TowardAngle = towardAngle;

                // TODO
                var speed = 30;
                var atan2 = towardAngle / GameConstant.Rad2Deg;
                var deltaX = Math.Cos(atan2) * 10000;
                var deltaY = Math.Sin(atan2) * 10000;
                var gameObject = unitEntity.GetComponent<UnitEntityGameObjectComponent>().GameObject;
                var r = gameObject.GetComponent<Rigidbody2D>();
                var v = new Vector2((int)deltaX, (int)deltaY).normalized * speed;

                var checkDistance = 0.5f;

                var hit2D = Physics2D.Raycast(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y),
                    new Vector2((float)deltaX, (float)deltaY).normalized, checkDistance);
                

                if (hit2D.collider != null)
                {
                    Log.Info(hit2D.collider.gameObject.name);
                    Log.Info("Raycast");
                    r.velocity = Vector2.zero;
                    return;
                }
                
                
                Log.Info("Move...");
                r.velocity = v;
                
                self.Position = new float2(gameObject.transform.position.x, gameObject.transform.position.y);
                self.IsMoving = true;
            }
            else
            {
                var gameObject = unitEntity.GetComponent<UnitEntityGameObjectComponent>().GameObject;
                var r = gameObject.GetComponent<Rigidbody2D>();
                r.velocity = Vector2.zero;
            }
        }

        private static void UpdateView(this MyPlayerCacheDataComponent self)
        {
            var unitEntity = self.GetParent<ClientUnitEntity>();

            var unitEntityGameObjectComponent = unitEntity.GetComponent<UnitEntityGameObjectComponent>();
            if (unitEntityGameObjectComponent == null)
            {
                return;
            }

            var gameObject = unitEntityGameObjectComponent.GameObject;
        }

        private static async ETTask SyncData(this MyPlayerCacheDataComponent self)
        {
            try
            {
                BattleUnitEntity battleUnitEntity = BattleUnitEntity.Create(true);

                var unitEntity = self.GetParent<ClientUnitEntity>();
                battleUnitEntity.InsId = unitEntity.InsId;

                var unitEntityPosition = unitEntity.GetUnitEntityElemData<UnitEntityPosition>();
                var unitEntityCameraData = unitEntity.GetUnitEntityElemData<UnitEntityCameraData>();
                var unitEntityTowardAngle = unitEntity.GetUnitEntityElemData<UnitEntityTowardAngle>();
                // var unitEntityPlayerAnimateStatus = unitEntity.GetUnitEntityElemData<UnitEntityPlayerAnimateStatus>();

                if (self.IsDragging && !unitEntityPosition.Position.Equals(self.Position))
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

                if (unitEntityTowardAngle.TowardAngle != self.TowardAngle)
                {
                    unitEntityTowardAngle.TowardAngle = (short)self.TowardAngle;
                    ushort compId = OpcodeType.Instance.GetOpcode(typeof(UnitEntityTowardAngle));
                    var unitEntityElemData = UnitEntityElemData.Create();
                    unitEntityElemData.CompId = compId;
                    unitEntityElemData.ElemDatas = MemoryPackHelper.Serialize(unitEntityTowardAngle);
                    battleUnitEntity.EleDatas.Add(unitEntityElemData);
                }

                if (battleUnitEntity.EleDatas.Count > 0)
                {
                    var clientBattleSenderComponent = ClientBattleSenderComponent.Instance;
                    C2B_PlayerUploadDirtyElemData request = C2B_PlayerUploadDirtyElemData.Create();
                    request.BattleUnitEntity = battleUnitEntity;
                    B2C_PlayerUploadDirtyElemData response = (B2C_PlayerUploadDirtyElemData)await clientBattleSenderComponent.Call(request);
                }

                if (!self.IsDragging && self.IsMoving)
                {
                    self.IsMoving = false;
                    C2B_PlayerMoveStop request = C2B_PlayerMoveStop.Create();
                    B2C_PlayerMoveStop response = (B2C_PlayerMoveStop)await ClientBattleSenderComponent.Instance.Call(request);
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