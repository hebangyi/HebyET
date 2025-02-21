using System;
using System.Net;
using ET.Client;

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

			// TODO Lobby 分配节点的信息
			var lobbyNodeInfo = EtcdHelper.GetRandomNode(SceneType.Lobby);
			if (lobbyNodeInfo == null)
			{
				response.Error = (int)ErrorCode.ServerNotStartFinished;
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
			
			response.Address = lobbyNodeInfo.OuterIpAndOuterPortAddress;
			response.Key = testAccount.roleItem.RoleId;
			await ETTask.CompletedTask;
		}

		private async ETTask CloseSession(Session session)
		{
			await session.Root().GetComponent<TimerComponent>().WaitAsync(1000);
			session.Dispose();
		}
	}
}
