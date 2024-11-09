using System;
using UnityEngine;

public class DemoBaseObject : MonoBehaviour
{
	protected void Init()
	{
		this.hide = false;
		this.startPos = base.transform.position;
	}

	public void OnHide()
	{
		if (!this.hide)
		{
			this.hide = true;
			base.gameObject.SetActive(false);
			base.transform.position = this.startPos;
		}
	}

	protected bool hide = true;

	protected Vector3 startPos;
}
