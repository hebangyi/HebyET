using System.Reflection;

namespace ET
{
    public abstract class BaseLogicUnitEntityContext : ILogicUnitEntityContext
    {
        public virtual void InitCommonData(UnitEntity unitEntity)
        {
            var attribute = this.GetType().GetCustomAttribute<LogicUnitEntityContext>();
            var layerType = attribute.LayerType;
            var ueTypeEnum = attribute.UeTypeEnum;
            
            var unitEntityCommonData = unitEntity.CreateUnitEntityElemData<UnitEntityCommonData>();
            unitEntityCommonData.UnitEntityType = ueTypeEnum;
            unitEntityCommonData.UELayerTypeEnum = layerType;


            var unitEntityAnimation = unitEntity.CreateUnitEntityElemData<UnitEntityAnimation>();
            unitEntityAnimation.AnimateState = AnimateStateEnum.Idle;
        }

        public virtual void InitCustomData(UnitEntity unitEntity)
        {
        }

        public virtual void Init(UnitEntity unitEntity)
        {
        }

        public virtual void Destroy(UnitEntity unitEntity)
        {
        }
    }
}

