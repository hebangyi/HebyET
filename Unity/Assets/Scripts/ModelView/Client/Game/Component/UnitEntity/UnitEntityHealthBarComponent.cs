using FairyGUI;

namespace ET.Client
{
    [ComponentOf(typeof(UnitEntity))]
    public class UnitEntityHealthBarComponent: Entity, IAwake
    {
        public GObject GObject;
    }
}
