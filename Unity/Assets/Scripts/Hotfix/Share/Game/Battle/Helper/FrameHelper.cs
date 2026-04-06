namespace ET
{
    public static class FrameHelper
    {
        public static uint CalFrameNum(long time)
        {
            return (uint)(time + GameConstant.LogicInterval - 1) / GameConstant.LogicInterval;
        }
    }
}

