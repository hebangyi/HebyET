using Unity.Mathematics;

namespace ET.Client
{
	[MessageHandler(SceneType.Game)]
	public class M2C_StopHandler : MessageHandler<Scene, M2C_Stop>
	{
		protected override async ETTask Run(Scene root, M2C_Stop message)
		{
			
			Log.Info("收到测试消息");
			await ETTask.CompletedTask;
			/*Unit unit = root.CurrentScene().GetComponent<UnitComponent>().Get(message.Id);
			if (unit == null)
			{
				return;
			}

			MoveComponent moveComponent = unit.GetComponent<MoveComponent>();
			moveComponent.Stop(message.Error == 0);
			unit.Position = message.Position;
			unit.Rotation = message.Rotation;
			unit.GetComponent<ObjectWait>()?.Notify(new Wait_UnitStop() {Error = message.Error});
			await ETTask.CompletedTask;*/
		}
	}
}
