//"--This is an automatically generated class by FairyGUIPlugin. Please do not modify it."
using ETModel;
using FairyGUI;

namespace ET.UICommon
{
    [ObjectSystem]
    public class FGUITestMainViewAwakeSystem : AwakeSystem<FGUITestMainView, GObject>
    {
        public override void Awake(FGUITestMainView self, GObject go)
        {
            self.Awake(go);
        }
    }

    public sealed class FGUITestMainView : FUI
    {
        public const string UIPackageName = "UICommon";
        public const string UIResourceName = "TestMainView";
        public const string UIResURL = "ui://UICommon/TestMainView";
        public const string FUIName = "UICommon_TestMainView";
        public static System.Action<FGUITestMainView> OnPreDisposeEvent;

        /// <summary>
        /// TestMainView的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GComponent self;


		public GButton fgui_Test


        static FGUITestMainView()
        {
            _subTypeUINameDic[typeof(FGUITestMainView)] = (UIPackageName, UIResourceName);
        }

        private static GObject CreateGObject()
        {
            return UIPackage.CreateObject(UIPackageName, UIResourceName);
        }

        private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
        {
            UIPackage.CreateObjectAsync(UIPackageName, UIResourceName, result);
        }

        public static string ResName()
        {
            return FUIName;
        }

        public static FGUITestMainView CreateInstanceWithResName()
        {
            FGUITestMainView inst = ComponentFactory.Create<FGUITestMainView, GObject>(CreateGObject());
            inst.Name = ResName();
            return inst;
        }

        public static FGUITestMainView CreateInstance()
        {
            return ComponentFactory.Create<FGUITestMainView, GObject>(CreateGObject());
        }

        public static ETTask<FGUITestMainView> CreateInstanceAsync()
        {
            ETTaskCompletionSource<FGUITestMainView> tcs = new ETTaskCompletionSource<FGUITestMainView>();

            CreateGObjectAsync((go) =>
            {
                tcs.SetResult(ComponentFactory.Create<FGUITestMainView, GObject>(go));
            });

            return tcs.Task;
        }

        public static FGUITestMainView Create(GObject go)
        {
            return ComponentFactory.Create<FGUITestMainView, GObject>(go);
        }

        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static FGUITestMainView GetFormPool(GObject go)
        {
            var fui = go.Get<FGUITestMainView>();

            if (fui == null)
            {
                fui = Create(go);
            }
            fui.isFromFGUIPool = true;

            return fui;
        }

        public void Awake(GObject go)
        {
            if (go == null)
            {
                return;
            }

            SelfGObject = go;

            if (string.IsNullOrWhiteSpace(Name))
            {
                Name = Id.ToString();
            }

            self = (GComponent)go;

            self.Add(this);

            var com = go.asCom;

            if (com != null)
            {
/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

fgui_Test = (GButton)GetChild("Test");

            }
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            OnPreDisposeEvent?.Invoke(this);

            self.Remove();
            self = null;
			Test?.Dispose();
			Test = null;

            base.Dispose();
        }
    }
}