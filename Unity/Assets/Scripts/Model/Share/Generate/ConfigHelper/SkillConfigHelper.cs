namespace ET
{
    public partial class SkillConfigCategory
    {
        public override void AfterLoadData()
        {
            foreach (var skillConfig in this.dict.Values)
            {
                Log.Info($"skillConfig , Id :{skillConfig.Id}, SkillAniTag : {skillConfig.SkillAniTag}, CD : {skillConfig.CD}");    
            }
        }
    }
}