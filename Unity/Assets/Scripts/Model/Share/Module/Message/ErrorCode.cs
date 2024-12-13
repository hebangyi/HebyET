namespace ET
{
    public enum ErrorCode
    {
        ERR_Success = 0,
        
        // 1-11004 是SocketError请看SocketError定义
        //-----------------------------------
        // 100000-109999是Core层的错误
        
        
        // 110000以下的错误请看ErrorCore.cs
        
        // 这里配置逻辑层的错误码
        // 110000 - 200000是抛异常的错误
        ServerInternalErr = 110000,                 // 服务器内部错误 
        ParamErr = 110001,                          // 参数错误
        
        ///////////////////////////// 登录
        ServerIsStarting = 110100,                  // 参数错误
        AccountLoginErr = 110101                    // 用户名密码错误
        
        
        // 200001以上不抛异常
    }
}