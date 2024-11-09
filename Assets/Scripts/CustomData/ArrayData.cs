using System;
using UnityEngine;

namespace CustomData
{
	[CreateAssetMenu(fileName = "ArrayData", menuName = "Assets/ArrayData", order = 1)]
	public class ArrayData : ScriptableObject
	{
		[SerializeField]
		internal ArrayInt[] intDatas;

		[SerializeField]
		internal ArrayFloat[] floatDatas;
	}
}
