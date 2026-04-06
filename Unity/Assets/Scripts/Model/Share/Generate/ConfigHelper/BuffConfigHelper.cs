namespace ET
{
    public partial class BuffConfig
    {
    
        public class RigidityBuffParam
        {
            // 持续时间
            public long DurationTime;
        }

        public RigidityBuffParam _rigidityBuffParam;

        public RigidityBuffParam RigidityBuffParamConfig
        {
            get
            {
                if (this._rigidityBuffParam == null)
                {
                    this._rigidityBuffParam = JsonHelper.FromJson<RigidityBuffParam>(this.BuffParam);
                }

                return this._rigidityBuffParam;
            }
        }
    }
}

