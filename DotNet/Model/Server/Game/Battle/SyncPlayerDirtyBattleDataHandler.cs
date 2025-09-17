using System;
using System.Collections.Generic;

namespace ET.Server;

public class SyncPlayerDirtyBattleDataHandler : ISyncHandler
{
    private LogicWorld mLogicWorld;
    public Action<LogicWorld> m_DoFunc;
    
    public SyncPlayerDirtyBattleDataHandler(LogicWorld logicWorld, Action<LogicWorld> doFunc)
    {
        this.mLogicWorld = logicWorld;
        this.m_DoFunc = doFunc;
    }

    public void Sync()
    {
        this.m_DoFunc?.Invoke(this.mLogicWorld);
    }
}