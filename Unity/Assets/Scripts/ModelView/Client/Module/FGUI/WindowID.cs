namespace ET.Client
{
    public enum WindowID
    {
        None = 0,
        CommonBG = 1,       // 测试背景
        LoginMainView = 2,  // 登录界面
        LoadingUIView = 3,  // 加载界面
        
        
        LobbyMainView = 4,  // 大厅
    }
    
    
    public enum UIWindowType
    {
        Normal,    // 普通主界面
        Fixed,     // 固定窗口
        PopUp,     // 弹出窗口
        Other,      //其他窗口
    }
}
