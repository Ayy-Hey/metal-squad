using System;
using UnityEngine;

namespace CustomData
{
	[Serializable]
	public class Gift
	{
		[SerializeField]
		internal Item gift;

		[SerializeField]
		internal int giftAmount;
	}
}
