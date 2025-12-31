using behaviac;
using ET;

namespace DotNet.Test;

public static class BTTest
{
    public static void Test()
    {
        behaviac.Workspace.Instance.FileFormat = behaviac.Workspace.EFileFormat.EFF_cs;
        
        TestAgent aiAgent = new TestAgent();
        aiAgent.btload("TestAgentTree", false);
        aiAgent.btsetcurrent("TestAgentTree");
        
        behaviac.Workspace.Instance.DoubleValueSinceStartup = (double)DateTime.UtcNow.Ticks / 10000;

        for (int i = 0; i < 1000; i++)
        {
            Thread.Sleep(100);
            behaviac.Workspace.Instance.DoubleValueSinceStartup = (double)DateTime.UtcNow.Ticks / 10000;
            Console.WriteLine(behaviac.Workspace.Instance.DoubleValueSinceStartup);
            
            
            var status = aiAgent.btexec();
            Console.WriteLine($"{aiAgent.GetClassTypeName()} status : {status}");
        }

        
    }
    
    
    public static void Test2()
    {
        // behaviac.Workspace.Instance.FileFormat = behaviac.Workspace.EFileFormat.EFF_cs;
        // MonsterAIAgent aiAgent = new MonsterAIAgent();
        // aiAgent.btsetcurrent("MonsterAITree");

        /*for (int i = 0; i < 200; i++)
        {
            var status = aiAgent.btexec();
            Console.WriteLine($"{aiAgent.GetClassTypeName()} status : {status}");
        }*/
    }


    public static void Test3()
    {
        Console.WriteLine((int)(-0.6));
    }
    
    
    public static void Test4()
    {
        Console.WriteLine(Math.Atan2(1, 0) * 57.29578f);
    }
    
    public static void Test5()
    {
        Console.WriteLine(-359 % 360);
    }
}