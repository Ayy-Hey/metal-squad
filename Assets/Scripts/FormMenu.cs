using System;
using UnityEngine;

public class FormMenu : MonoBehaviour
{
	public void Show()
	{
		base.gameObject.SetActive(true);
	}

	public void OnClose()
	{
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"OnClosed: ",
			this.OnClosed,
			" Name: ",
			base.name
		}));
		if (this.OnClosed != null)
		{
			this.OnClosed();
		}
		base.gameObject.SetActive(false);
	}

	public Action OnClosed;
}
