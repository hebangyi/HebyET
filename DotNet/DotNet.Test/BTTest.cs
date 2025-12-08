namespace DotNet.Test;

public static class BTTest
{
    public static void Test()
    {
        behaviac.Workspace.Instance.FileFormat = behaviac.Workspace.EFileFormat.EFF_cs;
        AIAgent aiAgent = new AIAgent();
        aiAgent.btload("AIAgent", false);
        aiAgent.btsetcurrent("AIAgent");

        for (int i = 0; i < 1000; i++)
        {
            var status = aiAgent.btexec();
            Console.WriteLine($"status : {status} , aiAgent : {aiAgent.status}");
        }
    }
}