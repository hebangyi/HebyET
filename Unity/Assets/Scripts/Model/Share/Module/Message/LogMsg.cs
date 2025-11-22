using System.Collections.Generic;
using System.Diagnostics;

namespace ET
{
    public class LogMsg: Singleton<LogMsg>, ISingletonAwake
    {
        private readonly HashSet<ushort> ignore = new()
        {
            ClientMessage.C2G_Ping, 
            ClientMessage.G2C_Ping, 
            ClientMessage.C2G_Benchmark, 
            ClientMessage.G2C_Benchmark,
            ClientMessage.L2C_PlayerAOIWorldDirtyPush,
            ClientMessage.C2B_PlayerUploadDirtyElemData,
            ClientMessage.B2C_PlayerUploadDirtyElemData,
        };

        public void Awake()
        {
        }

        [Conditional("DEBUG")]
        public void Debug(Fiber fiber, object msg)
        {
            ushort opcode = OpcodeType.Instance.GetOpcode(msg.GetType());
            if (this.ignore.Contains(opcode))
            {
                return;
            }
            fiber.Log.Debug(msg.ToString());
        }
    }
}