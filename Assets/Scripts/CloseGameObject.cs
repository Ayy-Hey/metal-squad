using System;
using System.Collections;
using UnityEngine;

public class CloseGameObject : MonoBehaviour
{
	private void OnEnable()
	{
		base.StartCoroutine(this.Hide());
	}

	private IEnumerator Hide()
	{
		yield return new WaitForSeconds(this.TIME_HIDE);
		base.gameObject.SetActive(false);
		yield break;
	}

	public float TIME_HIDE = 3f;

	public bool isPanelGun;
}
