using System;
using UnityEngine;
using UnityEngine.UI;

public class EffUIButton : MonoBehaviour
{
	public void OnButtonDown(bool isDown)
	{
		if (!this.enableEffect)
		{
			return;
		}
		base.transform.localScale = ((!isDown) ? Vector3.one : this.size);
	}

	public Button button;

	public bool enableEffect = true;

	public Vector3 size;
}
