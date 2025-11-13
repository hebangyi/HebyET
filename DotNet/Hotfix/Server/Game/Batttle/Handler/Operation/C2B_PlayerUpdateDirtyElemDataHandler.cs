using System.Collections.Generic;

namespace ET.Server;

[MessageClientHandler(SceneType.Battle)]
public class C2B_PlayerUpdateDirtyElemDataHandler: MessageClientHandler<BattleRole, C2B_PlayerUpdateDirtyElemData, B2C_PlayerUpdateDirtyElemData>
{
    protected override void Run(BattleRole battleRole, C2B_PlayerUpdateDirtyElemData request, B2C_PlayerUpdateDirtyElemData response)
    {
        var world = battleRole.World();

        if (world == null)
        {
            return;
        }
        
        var unitPlayerEntity = UnitPlayerHelper.GetPlayerUnitEntityByPlayerId(world, battleRole.RoleId);
        if (unitPlayerEntity == null)
        {
            return;
        }

        if (request.BattleUnitEntity == null)
        {
            return;
        }
        
        if (unitPlayerEntity.InsId != request.BattleUnitEntity.InsId)
        {
            return;
        }
        
        
        var elementList = request.BattleUnitEntity.EleDatas;
        foreach (var element in elementList)
        {
            var compId = element.CompId;
            var unitElemType = OpcodeType.Instance.GetType(compId);
            
            if (unitElemType == null)
            {
                Log.Error($"没有找到UnitEntity 组件 {compId} 对应的数据类型");
                continue;
            }
            
            var newUnitEntityElemData =
                    MemoryPackHelper.Deserialize(unitElemType, element.ElemDatas, 0, element.ElemDatas.Length) as IUnitEntityElemData;

            var oleUnitEntityElemData = unitPlayerEntity.UnitEntityData.GetValueOrDefault(compId);
            
            
            world.PublishEvent();
            
        }
        
    }
}