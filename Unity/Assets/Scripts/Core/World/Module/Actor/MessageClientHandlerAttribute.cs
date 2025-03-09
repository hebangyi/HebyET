using System;

namespace ET
{
    public class MessageClientHandlerAttribute: MessageHandlerAttribute
    {
        public MessageClientHandlerAttribute(SceneType sceneType) : base(sceneType)
        {
        }
    }
}

