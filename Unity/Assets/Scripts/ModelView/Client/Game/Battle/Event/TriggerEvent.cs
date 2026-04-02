using UnityEngine;

namespace ET.Client
{
    public struct MyPlayerTriggerEnterEvent
    {
        public Collider2D col;
    }
    
    public struct MyPlayerTriggerStayEvent
    {
        public Collider2D col;
    }
    
    public struct MyPlayerTriggerExitEvent
    {
    public Collider2D col;
    }
}
