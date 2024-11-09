using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class UltimateJoystickScreenSizeUpdater : UIBehaviour
{
	protected override void OnRectTransformDimensionsChange()
	{
		base.StartCoroutine("YieldPositioning");
	}

	private IEnumerator YieldPositioning()
	{
		yield return new WaitForEndOfFrame();
		UltimateJoystick[] allJoysticks = UnityEngine.Object.FindObjectsOfType(typeof(UltimateJoystick)) as UltimateJoystick[];
		for (int i = 0; i < allJoysticks.Length; i++)
		{
			allJoysticks[i].UpdatePositioning();
		}
		yield break;
	}
}
