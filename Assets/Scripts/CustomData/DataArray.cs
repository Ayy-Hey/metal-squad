using System;
using UnityEngine;

namespace CustomData
{
	[CreateAssetMenu(fileName = "DataArray", menuName = "Assets/DataArray", order = 1)]
	public class DataArray : ScriptableObject
	{
		[SerializeField]
		internal int[] intDatas;

		[SerializeField]
		internal float[] floatDatas;
	}
}
