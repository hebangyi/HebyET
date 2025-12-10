using behaviac;
using ET;

namespace DotNet.Test;

public static class BTTest
{
    public static void Test2()
    {
        behaviac.Workspace.Instance.FileFormat = behaviac.Workspace.EFileFormat.EFF_cs;
        MonsterAIAgent aiAgent = new MonsterAIAgent();
        aiAgent.btload("MonsterAITree", false);
        aiAgent.btsetcurrent("MonsterAITree");

        for (int i = 0; i < 200; i++)
        {
            Thread.Sleep(100);
            behaviac.Workspace.Instance.DoubleValueSinceStartup = (double)DateTime.UtcNow.Ticks / 1000000;
            var status = aiAgent.btexec();
            Console.WriteLine($"{aiAgent.GetClassTypeName()} status : {status}");
        }
    }
    
}