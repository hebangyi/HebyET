using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using CommandLine;

namespace ET.Server
{
    internal static class Init
    {
        private static int Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                Log.Error(e.ExceptionObject.ToString());
            };
            
            try
            {
                // 命令行参数
                Parser.Default.ParseArguments<Options>(args)
                    .WithNotParsed(error => throw new Exception($"命令行格式错误! {error}"))
                    .WithParsed((o)=>ApplicationContext.Instance.AddSingleton(o));
                
                ApplicationContext.Instance.AddSingleton<Logger>().Log = new NLogger(Options.Instance.AppType.ToString(), Options.Instance.Process, 0);
                
                ApplicationContext.Instance.AddSingleton<CodeTypes, Assembly[]>(new[] { typeof (Init).Assembly });
                ApplicationContext.Instance.AddSingleton<EventSystem>();
                
                // 强制调用一下mongo，避免mongo库被裁剪
                MongoHelper.ToJson(1);
                
                ETTask.ExceptionHandler += Log.Error;
                
                Log.Info($"server start........................ ");
				
                switch (Options.Instance.AppType)
                {
                    case AppType.ExcelExporter:
                    {
                        Options.Instance.Console = 1;
                        ExcelExporter.Export();
                        break;
                    }
                    case AppType.Proto2CS:
                    {
                        Options.Instance.Console = 1;
                        Proto2CS.Export();
                        break;
                    }
                    case AppType.CodeGen:
                    {
                        Options.Instance.Console = 1;
                        ComponentCodeGenerator.ExportComponentSystem();
                        break;
                    }
                }
                Thread.Sleep(5000);
                return 0;
            }
            catch (Exception e)
            {
                Log.Console(e.ToString());
            }
            
            Thread.Sleep(30000);
            return 1;
        }
    }
}