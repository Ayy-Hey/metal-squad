using System;
using Com.LuisPedroFonseca.ProCamera2D;
using UnityEngine;

public class TriggerCamera : BaseTrigger, IPositionOverrider
{
	public int POOrder { get; set; }

	public Vector3 OverridePosition(float deltaTime, Vector3 originalPosition)
	{
		throw new NotImplementedException();
	}
}
