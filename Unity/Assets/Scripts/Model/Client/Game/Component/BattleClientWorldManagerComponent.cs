namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class BattleClientWorldManagerComponent: Entity, IAwake
    {
        public static BattleClientWorldManagerComponent Instance;
        
        public World CurrentWorld { get; set; }
    }
}

