using System;

namespace ET
{
    public partial class SkillConfigCategory
    {
        public override void AfterLoadData()
        {
            foreach (var skillConfig in this.dict.Values)
            {
                Log.Info($"skillConfig , Id :{skillConfig.Id}, SkillAniTag : {skillConfig.SkillAniTag}, CD : {skillConfig.CDTime}");
                // BuffStack 排序
                Array.Sort(skillConfig.BuffStacks, (a, b) => a.StartTime.CompareTo(b.StartTime));
            }
        }
    }
    

}