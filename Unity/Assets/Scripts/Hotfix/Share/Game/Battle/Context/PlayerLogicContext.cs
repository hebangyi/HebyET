using System.Linq;
using Unity.Mathematics;

namespace ET
{
    [LogicUnitEntityContext(UELayerTypeEnum.Player, UETypeEnum.Player)]
    public class PlayerLogicContext : BaseLogicUnitEntityContext
    {
        public override void InitCommonData(UnitEntity unitEntity)
        {
            base.InitCommonData(unitEntity);
            
            var unitEntityCommonData = unitEntity.GetUnitEntityElemData<UnitEntityCommonData>();
            var battlePlayerConfig = BattlePlayerConfigCategory.Instance.GetOne();
            unitEntityCommonData.ConfigId = battlePlayerConfig.Id;
        }

        public override void InitCustomData(UnitEntity unitEntity)
        {
            var unitEntityInitContext = unitEntity.GetComponent<UnitEntityInitContext>();
            unitEntity.CreateUnitEntityElemData<GizmosPlayerAOICell>();
            unitEntity.CreateUnitEntityElemData<UnitEntityCameraData>();
            
            
            // 设置技能
            var unitEntitySkillData = unitEntity.CreateUnitEntityElemData<UnitEntitySkillData>();
            
            // 设置Buff
            unitEntity.CreateUnitEntityElemData<UnitEntityBuffData>();
            
            // 技能
            // 装填普攻技能
            var battlePlayerConfig = BattlePlayerConfigCategory.Instance.GetOne();
            var attackSkillConfig = SkillConfigCategory.Instance.GetById(battlePlayerConfig.AttackSkill);
            if (attackSkillConfig != null)
            {
                UnitEntitySkillDataItem unitEntitySkillDataItem = UnitEntitySkillDataItem.Create();
                unitEntitySkillDataItem.SkillId = battlePlayerConfig.AttackSkill;
                unitEntitySkillDataItem.SkillStatusEnum = SkillStatusEnum.Ready;
                unitEntitySkillData.SkillDataItems.Add(unitEntitySkillDataItem);
            }
            
            var unitEntityPosition = unitEntity.GetUnitEntityElemData<UnitEntityPosition>();
            unitEntityPosition.Position = new float2(0f, 0f);
            
            var unitEntityPlayerInfo = unitEntity.CreateUnitEntityElemData<UnitEntityPlayerInfo>();
            var playerId = unitEntityInitContext.Params as long?;
            unitEntityPlayerInfo.PlayerId = playerId.GetValueOrDefault();

            // 技能信息
            unitEntity.CreateUnitEntityLogicElemData<SkillRuntimeData>();
        }
        

        public override void InitComponent(UnitEntity unitEntity)
        {
            var playerInfo = unitEntity.GetUnitEntityElemData<UnitEntityPlayerInfo>();
            var gizmosPlayerAOICell = unitEntity.GetUnitEntityElemData<GizmosPlayerAOICell>();
            var unitEntityPosition = unitEntity.GetUnitEntityElemData<UnitEntityPosition>();
            var unitEntityPlayerSkill = unitEntity.GetUnitEntityElemData<UnitEntitySkillData>();
            
            unitEntity.LogicWorld().PlayerId2Players[playerInfo.PlayerId] = unitEntity;
            
            // 随机选择一个地块
            var logicWorld = unitEntity.LogicWorld();
            var unitEntityMap = logicWorld.PlantMessageUnitEntity;
            
            var unitEntityMapMessage = unitEntityMap.GetUnitEntityElemData<UnitEntityMapMessage>();
            var cellInfo = unitEntityMapMessage.PlantInfo.CellInfos.FirstOrDefault();
            if (cellInfo != null)
            {
                unitEntityPosition.Position = cellInfo.CenterPoint;
                unitEntityPosition.Position += new float2(10, 10);
            }
            

            unitEntity.AddComponent<PlayerAOISeeUnitEntity>();
            // 技能系统
            unitEntity.AddComponent<SkillComponent>();
            // buff系统
            unitEntity.AddComponent<BuffComponent>();
            unitEntity.AddComponent<AOIUnitEntity, float2, UETypeEnum>(unitEntityPosition.Position, UETypeEnum.Player);
            
            var cellIds = AOIHelper.GetAOICellIds(unitEntityPosition.Position);
            gizmosPlayerAOICell.CellIds.AddRange(cellIds);
        }

        public override  void Destroy(UnitEntity unitEntity)
        {
            var playerInfo = unitEntity.GetUnitEntityElemData<UnitEntityPlayerInfo>();
            unitEntity.LogicWorld().PlayerId2Players.Remove(playerInfo.PlayerId);
        }
    }
}