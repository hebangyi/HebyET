using System;
using Unity.Mathematics;
using UnityEngine;

namespace ET.Client
{
	[ComponentOf(typeof(UnityScene))]
	public class OperaComponent: Entity, IAwake, IUpdate
	{
		// 操作按钮的角度
		public int lastAngel = -1000;
		
		public int OperaAngel = -1000;
		// 镜头角度的便宜量
		public int CameraAngelOffset;
		
		// 上一次同步的位置
		public float2 LastSyncPosition;
		
	}
}
