using System;

namespace ET
{
    public class ResponseTypeAttribute: BaseAttribute
    {
        public string Type { get; }

        public ResponseTypeAttribute(string type)
        {
            this.Type = type;
        }
        
        public ResponseTypeAttribute(string type, string scene)
        {
            this.Type = type;
        }
        
        
        public ResponseTypeAttribute(string type, string scene, string entity )
        {
            this.Type = type;
        }
    }
}