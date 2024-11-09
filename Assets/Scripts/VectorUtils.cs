using System;
using UnityEngine;

public class VectorUtils
{
	public static Vector2 Rotate(Vector2 v, float degrees)
	{
		float f = degrees * 0.0174532924f;
		float num = Mathf.Sin(f);
		float num2 = Mathf.Cos(f);
		float x = v.x;
		float y = v.y;
		return new Vector2(num2 * x - num * y, num * x + num2 * y);
	}
}
