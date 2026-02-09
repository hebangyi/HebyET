using FairyGUI;

namespace ET.Client
{
    [ComponentOf(typeof(UnitEntity))]
    public class UnitEntityHealthBarComponent: Entity, IAwake<GObject>
    {
        public GObject GObject;
        public FGUIHealthBar FGUIHealthBar;
    }
}
