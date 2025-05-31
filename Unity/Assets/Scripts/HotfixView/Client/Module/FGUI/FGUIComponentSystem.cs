using System;
using System.Collections.Generic;
using FairyGUI;

namespace ET.Client
{
    [EntitySystemOf(typeof(FGUIComponent))]
    [FriendOf(typeof(FGUIComponent))]
    public static partial class FGUIComponentSystem
    {
        [EntitySystem]
        private static void Awake(this FGUIComponent self)
        {
            FGUIComponent.Instance = self;
            self._createAllLayer();
         }

        private static void _createAllLayer(this FGUIComponent self)
        {
            self.AllWindowTypes.Clear();

            int layerIndex = 0;
            foreach (UIWindowType windowType in Enum.GetValues(typeof(UIWindowType)))
            {
                GComponent component = new();
                component.displayObject.gameObject.name = windowType.ToString();
                GRoot.inst.AddChildAt(component, layerIndex++);
                FGUILayer fguiLayer = self.AddChild<FGUILayer, GObject>(component);
                self.AllWindowTypes[windowType] = fguiLayer;
            }
        }

        public static async ETTask ShowWindowAsync(this FGUIComponent self, WindowID windowId, ShowWindowData showWindowData = null)
        {
            UIBaseWindow baseWindow = await self.LoadWindowAsync(windowId);
            if (baseWindow != null)
            {
                self.RealShowWindow(baseWindow, windowId, showWindowData);
            }
        }

        public static void HideWindow(this FGUIComponent self, WindowID windowId)
        {
            UIBaseWindow baseWindow = self.GetUIBaseWindow(windowId);
            if (baseWindow != null)
            {
                self.RealHideWindow(baseWindow, windowId);
            }
        }

        public static void CloseWindow(this FGUIComponent self, WindowID windowId)
        {
            UIBaseWindow baseWindow = self.GetUIBaseWindow(windowId);
            if (baseWindow != null)
            {
                self.RealCloseWindow(baseWindow, windowId);
            }
        }
        
        # region 私有方法

        private static void RealHideWindow(this FGUIComponent self,UIBaseWindow baseWindow, WindowID id)
        {
            baseWindow.GObject.visible = false;
            self.InVisibleWindowsDict[(int)id] = baseWindow;
            var eventHandler = FGUIEventComponent.Instance.GetEventHandlerByWindowID(id);
            if (eventHandler == null)
            {
                Log.Error($"Window Id : {id} Not Found EventHandler");
                return;
            }
            eventHandler.OnHideWindow(baseWindow);
        }
        
        private static void RealShowWindow(this FGUIComponent self, UIBaseWindow baseWindow, WindowID id, ShowWindowData showWindowData)
        {
            baseWindow.GObject.visible = true;
            self.VisibleWindowsDict[(int)id] = baseWindow;
            var eventHandler = FGUIEventComponent.Instance.GetEventHandlerByWindowID(id);
            if (eventHandler == null)
            {
                Log.Error($"Window Id : {id} Not Found EventHandler");
                return;
            }
            eventHandler.OnShowWindow(baseWindow, showWindowData);
        }

        private static void RealCloseWindow(this FGUIComponent self, UIBaseWindow baseWindow, WindowID id)
        {
            baseWindow.GObject.visible = false;
            var eventHandler = FGUIEventComponent.Instance.GetEventHandlerByWindowID(id);
            if (eventHandler == null)
            {
                Log.Error($"Window Id : {id} Not Found EventHandler");
                return;
            }

            eventHandler.BeforeUnload(baseWindow);
            self.AllWindowsDict.Remove((int)id);
            self.VisibleWindowsDict.Remove((int)id);
            self.InVisibleWindowsDict.Remove((int)id);
            baseWindow.Dispose();
        }

        private static async ETTask<UIBaseWindow> LoadWindowAsync(this FGUIComponent self, WindowID id)
        {
            CoroutineLock coroutineLock = null;
            var coroutineLockComponent = self.Root().GetComponent<CoroutineLockComponent>();
            try
            {
                coroutineLock = await coroutineLockComponent.Wait(CoroutineLockType.LoadUIBaseWindows, (int)id);
                UIBaseWindow baseWindow = self.GetUIBaseWindow(id);

                if (baseWindow == null)
                {
                    baseWindow = self.AddChild<UIBaseWindow>();
                    baseWindow.WindowId = id;

                    var res = FGUIEventComponent.Instance.GetWindowPackageAndRes(id);
                    if (res.Item1 == null || res.Item2 == null)
                    {
                        Log.Error($"Window Id : {id} Not Found Package And Resource.");
                        return baseWindow;
                    }

                    string packageName = res.Item1;
                    string resourceName = res.Item2;
                    var gobject = await self.CreateGObject(packageName, resourceName);
                    if (gobject == null)
                    {
                        Log.Error($"Create GObject failed: {packageName} {resourceName}");
                        return baseWindow;
                    }

                    baseWindow.GObject = gobject;

                    var eventHandler = FGUIEventComponent.Instance.GetEventHandlerByWindowID(id);
                    if (eventHandler == null)
                    {
                        Log.Error($"Window Id : {id} Not Found EventHandler");
                        return baseWindow;
                    }

                    eventHandler.OnInitWindowCoreData(baseWindow);

                    var fguiLayer = self.AllWindowTypes.GetValueOrDefault(baseWindow.WindowType);
                    fguiLayer?.AddWindow(baseWindow.GObject);
                    
                    // baseWindow?.SetRoot(EUIRootHelper.GetTargetRoot(baseWindow.WindowData.windowType));
                    // baseWindow.uiTransform.SetAsLastSibling();

                    eventHandler.OnInitComponent(baseWindow);
                    eventHandler.OnRegisterUIEvent(baseWindow);
                    self.AllWindowsDict[(int)id] = baseWindow;
                }

                return baseWindow;
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            finally
            {
                coroutineLock?.Dispose();
            }

            return null;
        }

        /// <summary>
        /// 异步加载UI窗口实例
        /// </summary>
        private static async ETTask<GObject> CreateGObject(this FGUIComponent self, string packageName, string resourceName)
        {
            await FGUIPackageComponent.Instance.TryAddPackageAsync(packageName);
            ETTask<GObject> tcs = ETTask<GObject>.Create();
            UIPackage.CreateObjectAsync(packageName, resourceName, (GObject go) => { tcs.SetResult(go); });
            var gobject = await tcs;
            return gobject;
        }

        private static UIBaseWindow GetUIBaseWindow(this FGUIComponent self, WindowID id)
        {
            if (self.AllWindowsDict.ContainsKey((int)id))
            {
                return self.AllWindowsDict[(int)id];
            }

            return null;
        }
        # endregion
        
    }
}