using FairyGUI;

namespace ET.Client
{
    [ComponentOf(typeof(UnitEntity))]
    public class UnitEntityHealthBarComponent: Entity, IAwake<GObject>, IUpdate
    {
        // UI 上的血量条 GObject
        public GObject GObject;
        public FGUIHealthBar FGUIHealthBar;
    }
}
