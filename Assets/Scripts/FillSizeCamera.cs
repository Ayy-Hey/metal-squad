using System;
using UnityEngine;

public class FillSizeCamera : MonoBehaviour
{
	private void Start()
	{
		Vector3 localScale = base.transform.localScale;
		localScale.x = Mathf.Max(1f, (float)Screen.width / 1280f);
		localScale.y = Mathf.Max(1f, (float)Screen.height / 720f);
		base.transform.localScale = localScale;
	}
}
