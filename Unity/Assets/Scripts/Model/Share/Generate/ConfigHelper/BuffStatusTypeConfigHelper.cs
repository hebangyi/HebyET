using System.Collections.Generic;

namespace ET
{
    public partial class BuffStatusTypeConfigCategory
    {
        public Dictionary<BuffStatus, BuffStatusTypeConfig> BuffStatus2Config = new ();
        
        public override void AfterLoadData()
        {
            foreach (var config in this.dict.Values)
            {
                this.BuffStatus2Config[config.BuffStatus] = config;
            }
        }
    }
    
    public partial class BuffStatusTypeConfig
    {
        
    }
}