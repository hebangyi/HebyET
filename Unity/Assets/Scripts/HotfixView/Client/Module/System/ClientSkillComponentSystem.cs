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

        public static void UseSkill(this ClientSkillComponent self, long skillId)
        {
            var skillConfig = SkillConfigCategory.Instance.GetById(skillId);
            if (skillConfig == null)
            {
                return;
            }

            var cdTime = FrameHelper.CalFrameNum(skillConfig.CD) * GameConstant.LogicInterval;
            var nowTime = TimeInfo.Instance.NowMillTime();

            self.SkillCDS[skillId] = nowTime + cdTime;
            Log.Info($"Skill In Cd :{self.SkillCDS[skillId]}");
        }

        public static bool SkillInCD(this ClientSkillComponent self, long skillId)
        {
            if (!self.SkillCDS.ContainsKey(skillId))
            {
                Log.Info($"Skill not In Cd");
                return false;
            }

            var nowTime = TimeInfo.Instance.NowMillTime();
            var cdTime = self.SkillCDS.GetValueOrDefault(skillId);
            if (nowTime >= cdTime)
            {
                Log.Info($"Skill In Cd :{nowTime} {cdTime}");
                return false;
            }

            Log.Info($"Skill not In Cd True");
            return true;
        }
    }
}