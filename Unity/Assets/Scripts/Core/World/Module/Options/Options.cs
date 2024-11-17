using CommandLine;
using System;
using System.Collections.Generic;

namespace ET
{
    public enum AppType
    {
        Server,
        Watcher, // 每台物理机一个守护进程，用来启动该物理机上的所有进程
        GameTool,
        ExcelExporter,
        Proto2CS,
        BenchmarkClient,
        BenchmarkServer,

        Demo,
        LockStep,
    }

    public enum LocationType
    {
        CN,
    }

    public class Options : Singleton<Options>

    {
        # region 通用
        [Option("Process", Required = false, Default = 1)]
        public int Process { get; set; }
        [Option("LogLevel", Required = false, Default = 0)]
        public int LogLevel { get; set; }
        #endregion
        
        # region 服务器
        [Option("BigZone", Required = false, Default = 1)]
        public int BigZone { get; set; }
        [Option("ProcessConfig", Required = false, Default = "default_config.json", HelpText = "ProcessConfig File Name")]
        public string ProcessConfig { get; set; }
        [Option("StartConfig", Required = false, Default = "StartConfig/Localhost")]
        public string StartConfig { get; set; }
        [Option("Develop", Required = false, Default = 1, HelpText = "develop mode, 0正式 1开发 2压测")]
        public int Develop { get; set; }
        [Option("Console", Required = false, Default = 0)]
        public int Console { get; set; }
        # endregion

        # region 客户端
        // 地址 
        [Option("Location", Required = false, Default = LocationType.CN, HelpText = "location")]
        public LocationType LocationType { get; set; }

        [Option("AppType", Required = false, Default = AppType.Server, HelpText = "AppType enum")]
        public AppType AppType { get; set; }
        # endregion
    }
}