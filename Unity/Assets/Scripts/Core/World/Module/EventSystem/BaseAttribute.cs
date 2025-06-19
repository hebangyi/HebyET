using System;

namespace ET
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	[EnableClass]
	public class BaseAttribute: Attribute
	{
	}
}