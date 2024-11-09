using System;
using UnityEngine;

public class UISale43 : MonoBehaviour
{
	private void Start()
	{
		float num = ((float)Screen.width / (float)Screen.height - 1.333f) / 0.444f;
		if (num < 0.9f)
		{
			base.transform.localScale = this.scaleTarget;
		}
	}

	public Vector3 scaleTarget;
}
