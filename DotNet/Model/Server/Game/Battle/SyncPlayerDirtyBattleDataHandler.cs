using System;
using System.Collections.Generic;

namespace ET.Server;

public class SyncPlayerDirtyBattleDataHandler : ISyncHandler
{
    private World m_World;
    public Action<World> m_DoFunc;
    
    public SyncPlayerDirtyBattleDataHandler(World world, Action<World> doFunc)
    {
        this.m_World = world;
        this.m_DoFunc = doFunc;
    }

    public void Sync()
    {
        this.m_DoFunc?.Invoke(this.m_World);
    }
}