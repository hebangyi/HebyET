using System;

namespace ET
{
	[Flags]
	public enum SceneType: long
	{
		// 通用
		None = 0,
		Main = 1, // 主纤程,一个进程一个, 初始化从这里开始
		NetInner = 1 << 2, // 负责进程间消息通信
		
		
		// 子服务
		Entry = 1 << 3,	// 入口服务
		Account = 1 << 4, // 账号服务
		Lobby = 1 << 5,
		Http = 1 << 6,
		Location = 1 << 7,
		RouterGate = 1 << 9,
		RouterServer = 1 << 10,
		
		
		Map = 1 << 8,
		
		
		
		// 测试服务器
		Robot = 1 << 20,
		BenchmarkClient = 1 << 21,
		BenchmarkServer = 1 << 22,
		Match = 1 << 24,
		Room = 1 << 25,
		LockStepClient = 1 << 26,
		LockStepServer = 1 << 27,
		RoomRoot = 1 << 28,
		Watcher = 1 << 29,

		// 客户端
		Demo = 1 << 30,
		Current = 1L << 31,
		LockStep = 1L << 32,
		LockStepView = 1L << 33,
		DemoView = 1L << 34,
		NetClient = 1L << 35,

		All = long.MaxValue,
	}

	public static class SceneTypeHelper
	{
		public static bool HasSameFlag(this SceneType a, SceneType b)
		{
			if (((ulong) a & (ulong) b) == 0)
			{
				return false;
			}
			return true;
		}
	}
}