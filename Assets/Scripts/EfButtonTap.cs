using System;
using UnityEngine;

public class EfButtonTap : MonoBehaviour
{
	public void OnButtonDown()
	{
		base.transform.localScale *= 1.2f;
	}

	public void OnButtonUp()
	{
		base.transform.localScale = Vector3.one;
	}
}
