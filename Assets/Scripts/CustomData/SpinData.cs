using System;
using UnityEngine;

namespace CustomData
{
	[CreateAssetMenu(fileName = "SpinData", menuName = "Assets/SpinData", order = 1)]
	public class SpinData : ScriptableObject
	{
		public GiftPage[] giftPages;
	}
}
