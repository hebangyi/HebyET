using System;
using System.Collections.Generic;

namespace ET
{
    
    /// <summary>
    /// 战斗事件管理器
    /// </summary>
    [ComponentOf(typeof(Scene))]
    public class BattleEventManagerComponent: Entity, IAwake
    {
        public static BattleEventManagerComponent Instance;
        
        public readonly Dictionary<Type, List<IBattleEvent>> AllEvents = new();
    }
}

