using System;
using System.Threading;
using CommandLine;
using ET.Server;

namespace ET
{
	public class Init
	{
		public void Start()
		{
			try
			{
				AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
				{
					Log.Error(e.ExceptionObject.ToString());
				};
				
				// 服务器命令行参数
				Parser.Default.ParseArguments<Options>(System.Environment.GetCommandLineArgs())
						.WithNotParsed(error => throw new Exception($"命令行格式错误! {error}"))
						.WithParsed((o)=>ApplicationContext.Instance.AddSingleton(o));

				ApplicationContext.Instance.AddSingleton<Logger>().Log = new NLogger(Options.Instance.AppType.ToString(), Options.Instance.Process, 0);
				// 加载服务器配置
				ApplicationContext.Instance.AddSingleton<ProcessConfig>();
				
				ETTask.ExceptionHandler += Log.Error;
				
				ApplicationContext.Instance.AddSingleton<TimeInfo>();
				ApplicationContext.Instance.AddSingleton<FiberManager>();
				ApplicationContext.Instance.AddSingleton<CodeLoader>();
				ApplicationContext.Instance.AddSingleton<ServerManager>();
			}
			catch (Exception e)
			{
				Log.Error(e);
				throw;
			}
		}

		public void Update()
		{
			TimeInfo.Instance.Update();
			FiberManager.Instance.Update();
		}

		public void LateUpdate()
		{
			FiberManager.Instance.LateUpdate();
		}
	}
}
