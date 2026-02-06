using UnityEngine;

namespace ET.Client
{
    [UnitEntityViewLogic]
    public class UnitEntityPositionUpdate : BaseClientEleLogic<UnitEntityPosition>
    {
        public override void OnInitT(UnitEntity unitEntity, UnitEntityPosition elemData)
        {
        }

        public override void OnDestroyT(UnitEntity unitEntity, UnitEntityPosition elemData)
        {
        }

        public override void OnUpdateT(UnitEntity unitEntity, UnitEntityPosition oldData, UnitEntityPosition newData)
        {
            // 自己的玩家ID 不更新
            var clientWorld = unitEntity.ClientWorld();
            if (clientWorld.MainPlayerId == unitEntity.Id)
            {
                // TODO 如果我的缓存数据 和 UnitPosition 差距过大 则强制平移
                return;
            }

            var unitEntityGameObjectComponent = unitEntity.GetComponent<UnitEntityGameObjectComponent>();
            if (unitEntityGameObjectComponent != null && unitEntityGameObjectComponent.GameObject)
            {
                // TODO 平移更新
                unitEntityGameObjectComponent.GameObject.transform.position =
                        new Vector3(newData.Position.x, newData.Position.y, 0);
            }
            
            unitEntity.GetComponent<UnitEntityHealthBarComponent>()?.UpdateHPBarPosition();
        }
    }
}