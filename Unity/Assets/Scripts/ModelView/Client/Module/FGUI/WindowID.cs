namespace ET.Client
{
    public enum WindowID
    {
        None = 0,
        CommonBG = 1,       // 测试背景
        FGUILoginMainView = 2,  // 登录界面
        FGUILoadingUIView = 3,  // 加载界面
        FGUILobbyMainView = 4,  // 大厅
        FGUIBattleOperationMainView = 5,    // 战斗服操作界面
        FGUIHealthBarMainView = 6,    // 血量条
    }
    
    
    public enum UIWindowType
    {
        Normal,    // 普通主界面
        Fixed,     // 固定窗口
        PopUp,     // 弹出窗口
        Other,      //其他窗口
    }
}
