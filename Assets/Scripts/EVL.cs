using System;
using UnityEngine;

[Serializable]
public struct EVL
{
	[SerializeField]
	internal float hp;

	[Range(1f, 2000f)]
	[SerializeField]
	internal float damage;

	[Range(1f, 20f)]
	[SerializeField]
	internal float speed;

	[SerializeField]
	[Range(0f, 10f)]
	internal float timeReload;

	[Range(3f, 20f)]
	[SerializeField]
	internal float maxVision;
}
