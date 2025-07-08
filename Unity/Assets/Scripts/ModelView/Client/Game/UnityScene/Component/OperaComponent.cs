using System;

using UnityEngine;

namespace ET.Client
{
	[ComponentOf(typeof(UnityScene))]
	public class OperaComponent: Entity, IAwake, IUpdate
	{
		public int lastAngel = -1000;
		public int targetAngel = -1000;
	}
}
