namespace ET.Model;

[ComponentOf(typeof(Scene))]
public class UnitySceneManagerComponent: Entity, IAwake
{
    public static UnitySceneManagerComponent Instance;
    
    private EntityRef<UnityScene> unityScene;

    public UnityScene UnityScene
    {
        get
        {
            return this.unityScene;
        }
        set
        {
            this.unityScene = value;
        }
    }
    
}