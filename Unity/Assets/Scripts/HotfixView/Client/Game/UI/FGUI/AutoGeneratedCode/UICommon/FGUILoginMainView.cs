//"--This is an automatically generated class by FairyGUIPlugin. Please do not modify it."
using ETModel;
using FairyGUI;

namespace ET.ET.Client
{
    [ObjectSystem]
    public class FGUILoginMainViewAwakeSystem : AwakeSystem<FGUILoginMainView, GObject>
    {
        public override void Awake(FGUILoginMainView self, GObject go)
        {
            self.Awake(go);
        }
    }

    public sealed class FGUILoginMainView : FUI
    {
        public const string UIPackageName = "UICommon";
        public const string UIResourceName = "LoginMainView";
        public const string UIResURL = "ui://UICommon/LoginMainView";
        public const string FUIName = "UICommon_LoginMainView";
        public static System.Action<FGUILoginMainView> OnPreDisposeEvent;

        /// <summary>
        /// LoginMainView的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GComponent self;

		public GButton fgui_loginBtn;l


        static FGUILoginMainView()
        {
            _subTypeUINameDic[typeof(FGUILoginMainView)] = (UIPackageName, UIResourceName);
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

        public static FGUILoginMainView CreateInstanceWithResName()
        {
            FGUILoginMainView inst = ComponentFactory.Create<FGUILoginMainView, GObject>(CreateGObject());
            inst.Name = ResName();
            return inst;
        }

        public static FGUILoginMainView CreateInstance()
        {
            return ComponentFactory.Create<FGUILoginMainView, GObject>(CreateGObject());
        }

        public static ETTask<FGUILoginMainView> CreateInstanceAsync()
        {
            ETTaskCompletionSource<FGUILoginMainView> tcs = new ETTaskCompletionSource<FGUILoginMainView>();

            CreateGObjectAsync((go) =>
            {
                tcs.SetResult(ComponentFactory.Create<FGUILoginMainView, GObject>(go));
            });

            return tcs.Task;
        }

        public static FGUILoginMainView Create(GObject go)
        {
            return ComponentFactory.Create<FGUILoginMainView, GObject>(go);
        }

        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static FGUILoginMainView GetFormPool(GObject go)
        {
            var fui = go.Get<FGUILoginMainView>();

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
				fgui_loginBtn = (GButton)com.GetChild("loginBtn");

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
			fgui_loginBtn?.Dispose();
			fgui_loginBtn = null;

            base.Dispose();
        }
    }
}