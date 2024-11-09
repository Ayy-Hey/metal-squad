using System;
using UnityEngine;

[AddComponentMenu("EasyTouch Controls/Set Direct Action Transform ")]
public class ETCSetDirectActionTransform : MonoBehaviour
{
	private void Start()
	{
		if (!string.IsNullOrEmpty(this.axisName1))
		{
			ETCInput.SetAxisDirecTransform(this.axisName1, base.transform);
		}
		if (!string.IsNullOrEmpty(this.axisName2))
		{
			ETCInput.SetAxisDirecTransform(this.axisName2, base.transform);
		}
	}

	public string axisName1;

	public string axisName2;
}
