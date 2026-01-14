using System;

namespace ET
{
    public class ResponseTypeAttribute: BaseAttribute
    {
        public string Type { get; }

        public ResponseTypeAttribute(string type, string scene = null, string entity = null)
        {
            this.Type = type;
        }
    }
}