using System.Collections.Generic;

namespace ET.Client
{
    [FriendOf(typeof(ClientSkillComponent))]
    [EntitySystemOf(typeof(ClientSkillComponent))]
    public static partial class ClientSkillComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ClientSkillComponent self)
        {
        }

        public static void AddSkillCD(this ClientSkillComponent self, long skillId)
        {
            var skillConfig = SkillConfigCategory.Instance.GetById(skillId);
            if (skillConfig == null)
            {
                return;
            }

            
            var cdTime = FrameHelper.CalFrameNum(skillConfig.CDTime) * GameConstant.LogicInterval;
            var nowTime = TimeInfo.Instance.NowMillTime();

            self.SkillCDS[skillId] = nowTime + cdTime;
        }

        public static bool CheckSkillInCD(this ClientSkillComponent self, long skillId)
        {
            if (!self.SkillCDS.ContainsKey(skillId))
            {
                return false;
            }

            var nowTime = TimeInfo.Instance.NowMillTime();
            var cdTime = self.SkillCDS.GetValueOrDefault(skillId);
            if (nowTime >= cdTime)
            {
                return false;
            }

            return true;
        }
    }
}