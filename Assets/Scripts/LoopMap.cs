using System;
using System.Collections.Generic;
using UnityEngine;

public class LoopMap : SingletonGame<LoopMap>
{
	private void Start()
	{
		foreach (Renderer renderer in this.listBoxMap)
		{
			LoopMap.allMapSize += renderer.bounds.size.x;
		}
	}

	private void Update()
	{
		base.transform.Translate(Time.deltaTime * 3f, 0f, 0f);
	}

	public List<Renderer> listBoxMap;

	public static float allMapSize;

	private float p;
}
