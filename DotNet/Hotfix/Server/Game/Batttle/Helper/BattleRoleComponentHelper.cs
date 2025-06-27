using NativeCollection;

namespace ET.Server;


// 在线玩家秒级监听处理器
[Event(SceneType.Battle)]
public class GlobalTimeTenSecond_CheckBattlePlayerClockTime : AEvent<Scene, GlobalTimeTenSecond>
{
    protected override async ETTask Run(Scene scene, GlobalTimeTenSecond args)
    {
        var battleRoleComponent = scene.GetComponent<BattleRoleComponent>();
        battleRoleComponent.CheckAndRemoveOfflinePlayer();
        await ETTask.CompletedTask;
    }
}


public static class BattleRoleComponentHelper
{
    // 检查并移除下线的玩家
    public static void CheckAndRemoveOfflinePlayer(this BattleRoleComponent self)
    {
        List<long> unloadedIds = new List<long>();
        foreach (var roleKv in self.BattleRoles)
        {
            BattleRole role = roleKv.Value;
            if (role == null || role.RoleStatus == BattleRoleStatus.Offline)
            {
                unloadedIds.Add(role.RoleId);
            }
        }


        foreach (var unloadedId in unloadedIds)
        {
            if(self.BattleRoles.Remove(unloadedId, out var role))
            {
                BattleRole r = role;
                r?.Dispose();
            }
            
            Log.Info($"玩家已经卸载 : {unloadedId}");
        }
    }
}