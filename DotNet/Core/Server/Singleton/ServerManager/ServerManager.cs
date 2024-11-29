namespace ET.Server;

public class ServerManager:Singleton<ServerManager>, ISingletonAwake
{
    public CancellationTokenSource CancellationToken = new ();
    
    public void Awake()
    {
        
    }
}