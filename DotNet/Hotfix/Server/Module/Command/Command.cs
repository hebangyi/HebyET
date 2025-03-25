namespace ET.Server;

public static class CommandHelper
{
    public static void Execute(CommandEnum commandEnum, params string[] args)
    {
        switch (commandEnum)
        {
            case CommandEnum.ServerExit:
            {
                FiberManager.Instance.AllFiberExit().Coroutine();
                break;
            }
            default:
            {
                Log.Error($"命令枚举 {commandEnum} 没有找到处理方法!");
                break;
            }
        }
    }
}