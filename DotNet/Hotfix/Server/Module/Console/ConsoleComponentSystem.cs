using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ET.Server;

namespace ET
{
    [EntitySystemOf(typeof(ConsoleComponent))]
    [FriendOf(typeof(ConsoleComponent))]
    [FriendOf(typeof(ModeContex))]
    public static partial class ConsoleComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ConsoleComponent self)
        {
            self.Start().Coroutine();
        }

        
        private static async ETTask Start(this ConsoleComponent self)
        {
            self.CancellationTokenSource = new CancellationTokenSource();

            while (true)
            {
                try
                {
                    ModeContex modeContex = self.GetComponent<ModeContex>();
                    string line = await Task.Factory.StartNew(() =>
                    {
                        Console.Write($"{modeContex?.Mode ?? ""}> ");
                        return Console.In.ReadLine();
                    }, self.CancellationTokenSource.Token);
                    var lines = line.Trim().Split(" ");
                    if (lines.Length <= 0)
                    {
                        continue;
                    }
                    
                    var commandStr = lines[0];
                    if (Enum.TryParse(commandStr, true, out CommandEnum e))
                    {
                        string[] args = lines.Skip(1).ToArray();
                        CommandHelper.Execute(e, args);
                    }
                    else
                    {
                        Log.Error($"命令解析 CommandEnum 异常 : 命令 {commandStr}");
                    }
                }
                catch (Exception e)
                {
                    Log.Console(e.ToString());
                }
            }
        }
    }
}