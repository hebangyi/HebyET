﻿using System;
using System.Net;


namespace ET.Server
{
	[MessageSessionHandler(SceneType.Account)]
	public class C2R_LoginHandler : MessageSessionHandler<C2R_Login, R2C_Login>
	{
		protected override async ETTask Run(Session session, C2R_Login request, R2C_Login response)
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
			
			// TODO 后期进入队列  
			
			


			/*// 随机分配一个Ga
			 te
			StartSceneConfig config = RealmGateAddressHelper.GetGate(session.Zone(), request.Account);
			Log.Debug($"gate address: {config}");

			// 向gate请求一个key,客户端可以拿着这个key连接gate
			R2G_GetLoginKey r2GGetLoginKey = R2G_GetLoginKey.Create();
			r2GGetLoginKey.Account = request.Account;
			G2R_GetLoginKey g2RGetLoginKey = (G2R_GetLoginKey) await session.Fiber().Root.GetComponent<MessageSender>().Call(
				config.ActorId, r2GGetLoginKey);

			response.Address = config.InnerIPPort.ToString();
			response.Key = g2RGetLoginKey.Key;
			response.GateId = g2RGetLoginKey.GateId;

			CloseSession(session).Coroutine();*/
			response.Address = "127.0.0.1";
			response.Key = 0;
			response.GateId = 0; 
			
			await ETTask.CompletedTask;
		}

		private async ETTask CloseSession(Session session)
		{
			await session.Root().GetComponent<TimerComponent>().WaitAsync(1000);
			session.Dispose();
		}
	}
}
