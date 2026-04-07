namespace ET
{
    public static class ErrorCode
    {
        public const int ERR_Success = 0;
        
        // 1-11004 是SocketError请看SocketError定义
        //-----------------------------------
        // 100000-109999是Core层的错误
        
        
        // 110000以下的错误请看ErrorCore.cs
        
        // 这里配置逻辑层的错误码
        // 110000 - 200000是抛异常的错误
        public const int ServerInternalErr = 110000;                 // 服务器内部错误 
        public const int ParamErr = 110001;                          // 参数错误
        public const int ServerNotStartFinished = 11000;            // 服务器没有启动完成
        
        public const int ClientInternalErr = 120000;                 // 服务器内部错误
        
        ////////////////////////////////////////////////////////// Start Lobby
        ///////////////////////////// 登录
        public const int ServerIsStarting = 210100;                  // 参数错误
        public const int OtherPersonLogin = 210101;                  // 其他玩家登录
        
        ////////////////////////////////////////////////////////// End Lobby
        
        
        ////////////////////////////////////////////////////////// Start Battle
        ///////////////////////////// 战斗匹配
        public const int NotFoundBattleNode = 220101;                  // 其他玩家登录
        public const int PlayerIsMatching = 220102;                    // 玩家正在登录
        
        ///////////////////////////// 战斗服登录
        public const int NotFoundBattleWorld = 220201;                 // 没有找到战斗世界
        public const int NotFoundWorldPlayer = 220202;                 // 战斗世界没有找到玩家

        public const int SkillInCD = 220301;                           // 技能在CD中
        
        
        ////////////////////////////////////////////////////////// End Battle
        
        
        //// 账号服 
        public const int AccountLoginErr = 310101;                   // 用户名密码错误
        
        //// 逻辑大厅服
        // 登录
        public const int LoginTokenErr = 411101;                     // 用户名密码错误
        
        // 200001以上不抛异常
    }
}