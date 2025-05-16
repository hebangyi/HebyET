using System;
using System.Collections.Generic;
using System.Reflection;

namespace ET.Client
{
    // 
    public class PageNode
    {
        public ViewInfo normalUI; //基础界面(一定是全屏带背景)
        public ViewInfo BubbleViewInfo; //打开中的气泡界面，若无则为null
        public Queue<ViewInfo> BubbleQueses = new Queue<ViewInfo>(); //堆内是所有待打开 气泡弹出界面(同时只打开一个)
        public Stack<ViewInfo> PopStacks = new Stack<ViewInfo>(); //栈内的是所有 (一定是半透)弹窗界面
        public Queue<ViewInfo> PopBubbleQueses = new Queue<ViewInfo>(); //所有待打开 Pop气泡弹出界面(第一個Pop關閉之後彈出第二個)

        public PageNode(ViewType viewType, params object[] param)
        {
            normalUI = new ViewInfo(viewType, param);
        }

        public PageNode(ViewInfo viewInfo)
        {
            normalUI = viewInfo;
        }
    }

    public class ViewInfo
    {
        public ViewType ViewType;
        public object[] ViewPrams;

        public ViewInfo(ViewType ViewType, object[] viewPrams)
        {
            this.ViewType = ViewType;
            this.ViewPrams = viewPrams;
        }
    }

    public class FGUIManagerComponent
    {
        private Stack<PageNode> _uiTree = new (10);
        private Dictionary<ViewType, PageNode> _allNodes = new ();

        private Dictionary<ViewType, Type> _viewType2ControllerTypes = new Dictionary<ViewType, Type>();
        private Dictionary<ViewType, BasePageController> _allViewType2Controllers = new Dictionary<ViewType, BasePageController>();

        
        public void Awake()
        {
            HashSet<Type> types = CodeTypes.Instance.GetAttributeTypes(typeof(FGUIViewControllerAttribute));
            foreach (var type in types)
            {
                var viewAttr = (FGUIViewControllerAttribute)type.GetCustomAttribute(typeof(FGUIViewControllerAttribute), false);
                if (viewAttr == null)
                {
                    continue;
                }

                this._viewType2ControllerTypes[viewAttr.ViewType] = type;
            }
        }
        
        
        public BasePageController GetControllerByViewType(ViewType viewType)
        {
            if (viewType == ViewType.None)
            {
                return null;
            }

            var pageController = _allViewType2Controllers.GetValueOrDefault(viewType);
            if (pageController != null)
            {
                return pageController;
            }

            var controllerType = _viewType2ControllerTypes.GetValueOrDefault(viewType);
            if (controllerType == null)
            {
                Log.Error($"View Type {viewType} 2 Controller Type NotFound!");
                return null;
            }
            
            var basePageController = Activator.CreateInstance(controllerType) as BasePageController;
            _allViewType2Controllers[viewType] = basePageController;
            return basePageController;
        }
    }
}