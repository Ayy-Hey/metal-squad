using System;
using UnityEngine;

public class MapOfLoop : MonoBehaviour
{
	private void OnBecameInvisible()
	{
		MonoBehaviour.print(base.name);
		if (base.transform != null)
		{
			base.transform.position += new Vector3(LoopMap.allMapSize, 0f, 0f);
		}
	}
}
