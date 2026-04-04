using System.Collections.Generic;

namespace ET
{
    public class SkillRuntimeData : IUnitEntityLogicElemData
    {
        public uint BuffIdGen;
        public long GlobalSkillCD; // 全局CD
        
        public List<SkillData> RuntimeSkillDatas = new();
        public List<BuffData> RunningBuffDatas = new(); // 持续执行的Buff
    }

    public class SkillData
    {
        public SkillData ParentSkillData; // 如果有 可能为Null
        public long UnitInsId; // 释放的Unit
        public SkillConfig SkillConfig;
      
        public long SkillAddTime;
        public long SkillStartTime;

        // 是否释放了buff
        public bool IsRunBuff = false;
        public List<BuffData> AllBuffDatas = new(); // 所有的 BuffData
        
        // 是否使用动画
        public bool IsRunAnimation = false;
        
        public int ExecutedBuffCount;               // 已经执行完成Buff数量
    }

    public class BuffData
    {
        public uint BuffId;
        public BuffConfig BuffConfig;
        public SkillData SkillData;     // 释放的技能
        public int OffExecuteTime;      // buff相对技能释放时间
        
        
        public bool IsRun = false;
        public long BuffStartTime;       // Buff 执行时间
        public long BuffEndTime;
        public bool IsExit = false;
    }
}