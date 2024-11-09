using System;
using UnityEngine;

namespace CustomData
{
	[CreateAssetMenu(fileName = "GiftArray", menuName = "Assets/GiftArray", order = 1)]
	public class GiftArray : ScriptableObject
	{
		[SerializeField]
		internal Gift[] gifts;
	}
}
