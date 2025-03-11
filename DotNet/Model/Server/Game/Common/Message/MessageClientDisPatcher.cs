using System;
using System.Collections.Generic;

namespace ET
{
    public class MessageClientDisPatcherInfo
    {
        public SceneType SceneType { get; }
        public IMessageClientHandler Handler { get; }

        public MessageClientDisPatcherInfo(SceneType sceneType, IMessageClientHandler handler)
        {
            this.SceneType = sceneType;
            this.Handler = handler;
        }
    }

    [Code]
    public class MessageClientDisPatcher : Singleton<MessageClientDisPatcher>, ISingletonAwake
    {
        private readonly Dictionary<ushort, List<MessageClientDisPatcherInfo>> handlers = new();
        
        
        public void Awake()
        {
            HashSet<Type> types = CodeTypes.Instance.GetAttributeTypes(typeof (MessageClientHandlerAttribute));

            foreach (Type type in types)
            {
                IMessageClientHandler handler = Activator.CreateInstance(type) as IMessageClientHandler;
                if (handler == null)
                {
                    Log.Error($"message handle {type.Name} 需要继承 IMHandler");
                    continue;
                }
                
                object[] attrs = type.GetCustomAttributes(typeof(MessageClientHandlerAttribute), true);

                foreach (object attr in attrs)
                {
                    MessageClientHandlerAttribute messageClientHandlerAttribute = attr as MessageClientHandlerAttribute;

                    var requestType = handler.GetRequestType();
                    
                    ushort opcode = OpcodeType.Instance.GetOpcode(requestType);
                    if (opcode == 0)
                    {
                        Log.Error($"消息opcode为0: {requestType.Name}");
                        continue;
                    }
                    MessageClientDisPatcherInfo messageClientDisPatcherInfo = new (messageClientHandlerAttribute.SceneType, handler);
                    this.RegisterHandler(opcode, messageClientDisPatcherInfo);
                }
            }
        }
        
        private void RegisterHandler(ushort opcode, MessageClientDisPatcherInfo handler)
        {
            if (!this.handlers.ContainsKey(opcode))
            {
                this.handlers.Add(opcode, new List<MessageClientDisPatcherInfo>());
            }

            this.handlers[opcode].Add(handler);
        }
        
        public void Handle(Entity entity, object message)
        {
            List<MessageClientDisPatcherInfo> actions;
            ushort opcode = OpcodeType.Instance.GetOpcode(message.GetType());
            if (!this.handlers.TryGetValue(opcode, out actions))
            {
                Log.Error($"消息没有处理: {opcode} {message}");
                return;
            }

            SceneType sceneType = entity.IScene.SceneType;
            foreach (MessageClientDisPatcherInfo ev in actions)
            {
                if (!ev.SceneType.HasSameFlag(sceneType))
                {
                    continue;
                }
                
                try
                {
                    ev.Handler.Handle(entity, message);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }
        
    }
}




