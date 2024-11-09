using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class UltimateButtonScreenSizeUpdater : UIBehaviour
{
	protected override void OnRectTransformDimensionsChange()
	{
		base.StartCoroutine("YieldPositioning");
	}

	private IEnumerator YieldPositioning()
	{
		yield return new WaitForEndOfFrame();
		UltimateButton[] allButtons = UnityEngine.Object.FindObjectsOfType(typeof(UltimateButton)) as UltimateButton[];
		for (int i = 0; i < allButtons.Length; i++)
		{
			allButtons[i].UpdatePositioning();
		}
		yield break;
	}
}
