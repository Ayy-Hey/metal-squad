using System;
using UnityEngine;

public class ThisPlatform
{
	public static bool IsIphoneX
	{
		get
		{
			float num = (float)Screen.width / (float)Screen.height;
			return (num <= 1.8f || num >= 2.1f) && num >= 2.1f;
		}
	}
}
