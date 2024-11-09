using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DataEVL", menuName = "Assets/DataEVL", order = 1)]
public class DataEVL : ScriptableObject
{
	[SerializeField]
	internal EVL[] datas;
}
