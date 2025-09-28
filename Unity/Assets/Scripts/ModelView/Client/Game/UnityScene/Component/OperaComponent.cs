using System;

using UnityEngine;

namespace ET.Client
{
	[ComponentOf(typeof(UnityScene))]
	public class OperaComponent: Entity, IAwake, IUpdate
	{
		public int lastAngel = -1000;
		
		public int operaAngel = -1000;

		// 镜头角度的便宜量
		public int CameraAngelOffset;
	}
}
