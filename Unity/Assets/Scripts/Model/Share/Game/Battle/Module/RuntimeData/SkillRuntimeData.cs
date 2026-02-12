using System.Collections.Generic;

namespace ET
{
    public class SkillRuntimeData
    {
        public bool IsExecuting;
        public long GlobalCDTime;       // 全局CD时间


        public Dictionary<uint, BuffData> RuntimeBuffDatas = new ();    // 正在执行的Buff
    }


    public class SkillData
    {
        public uint SkillId;
        public SkillConfig BuffConfig;
        public long UnitInsId;                      // 释放的Unit
        
        
        public List<BuffData> BuffDatas = new ();   // 参数的Buff
    }

    public class BuffData
    {
        public uint BuffId;
        public SkillData SkillData;   // 释放的技能
        public bool IsExecuted = false;
     
        public BuffConfig BuffConfig;
        public long BuffStartTime;    // Buff 执行时间
        public long BuffEndTime;
    }
}