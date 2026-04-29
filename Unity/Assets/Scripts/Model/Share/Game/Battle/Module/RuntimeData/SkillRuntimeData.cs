using System.Collections.Generic;

namespace ET
{
    public class SkillRuntimeData : IUnitEntityLogicElemData
    {
        public long GlobalSkillCD; // 全局CD
        public List<SkillData> RuntimeSkillDatas = new();
    }

    public class SkillData
    {
        public SkillData ParentSkillData; // 如果有 可能为Null
        public long UnitInsId; // 释放的Unit
        public SkillConfig SkillConfig;
      
        // 技能 开始帧
        public uint SkillStartFrame;
        // buff Stack 开始帧
        public uint BuffStackStartFrame;
        
        // buffStack 执行索引
        public int BuffStackIndex;
        
        // 是否使用动画
        public bool IsRunAnimation = false;
    }

    public class BuffData
    {
        public long BuffId;
        public BuffConfig BuffConfig;
        public SkillData SkillData;     // 释放的技能
        public uint BuffStartFrame;       // Buff 执行时间
    }
}