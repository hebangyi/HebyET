using System;
using System.Net;


namespace ET.Server
{
	[MessageSessionHandler(SceneType.Account)]
	public class C2A_LoginHandler : MessageSessionHandler<C2A_Login, A2C_Login>
	{
		protected override async ETTask Run(Session session, C2A_Login request, A2C_Login response)
		{
			if (!AccountHelper.CheckNormalAccountValidate(request.Account))
			{
				response.Error = (int)ErrorCode.AccountLoginErr;
				return;
			}
			
			var ret = LoginHelper.CheckLoginCommon(session.Root());
			if (ret != ErrorCode.ERR_Success)
			{
				response.Error = (int)ret;
				return;
			}

			// TODO 后期进入队列 并且可以横向扩展
			var mongoDbComponent = session.Fiber().Root.GetComponent<MongoDBComponent>();
			var testAccount = await mongoDbComponent.QueryOne<TestAccount>(x => x.Account == request.Account);
			if (testAccount == null)
			{
				testAccount = new TestAccount();
				testAccount.Account = request.Account;
				testAccount.roleItem = new RoleItem();
				
				testAccount.roleItem.RoleId = IdGenerater.Instance.GenerateId();
				testAccount.roleItem.NickName = testAccount.roleItem.RoleId.ToString();
				
				await mongoDbComponent.Save(testAccount);
			}
			
			// 1.获取一个可用的大厅服SceneNode
			SceneNodeInfo sceneNodeInfo = new ();
			// 2.根据请求查询账号DB
			
			// TODO
			// 3.如果没有账号 发送账号 注册到Lobby注册账号
			ActorId actorId = new ActorId(sceneNodeInfo.ProcessId, sceneNodeInfo.SceneId);
			// TODO
			
			// 4.发送登录请求
			R2G_GetLoginKey r2GGetLoginKey = R2G_GetLoginKey.Create();
			r2GGetLoginKey.Account = request.Account;
			G2R_GetLoginKey g2RGetLoginKey = (G2R_GetLoginKey) await session.Fiber().Root.GetComponent<MessageSender>().Call(actorId, r2GGetLoginKey);
			
			
			// 分配的 Lobby服务器
			
			
			
			/*
			

			response.Address = config.InnerIPPort.ToString();
			response.Key = g2RGetLoginKey.Key;
			response.GateId = g2RGetLoginKey.GateId;

			CloseSession(session).Coroutine();
			response.Address = "127.0.0.1";
			response.Key = 0;
			response.GateId = 0; */
			
			await ETTask.CompletedTask;
		}

		private async ETTask CloseSession(Session session)
		{
			await session.Root().GetComponent<TimerComponent>().WaitAsync(1000);
			session.Dispose();
		}
	}
}
