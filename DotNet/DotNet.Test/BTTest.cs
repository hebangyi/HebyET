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
        string currentDirectory = Directory.GetCurrentDirectory();

        // 获取当前目录下所有文件的完整路径（仅当前目录，不包含子文件夹）
        string[] allFiles = Directory.GetFiles(currentDirectory);

        // 遍历输出文件列表
        Console.WriteLine("\n当前目录下的所有文件：");
        foreach (string filePath in allFiles)
        {
            Console.WriteLine(filePath);
        }
        
    }
}