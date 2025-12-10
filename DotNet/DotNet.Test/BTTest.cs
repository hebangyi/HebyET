using behaviac;
using ET;

namespace DotNet.Test;

public static class BTTest
{
    public static void Test2()
    {
        // behaviac.Workspace.Instance.FileFormat = behaviac.Workspace.EFileFormat.EFF_cs;
        MonsterAIAgent aiAgent = new MonsterAIAgent();
        aiAgent.btsetcurrent("MonsterAITree");

        for (int i = 0; i < 200; i++)
        {
            var status = aiAgent.btexec();
            Console.WriteLine($"{aiAgent.GetClassTypeName()} status : {status}");
        }
    }
    
}