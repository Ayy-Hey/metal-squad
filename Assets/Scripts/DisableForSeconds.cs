using System;
using System.Collections;
using UnityEngine;

public class DisableForSeconds : MonoBehaviour
{
	public void StartCountdown()
	{
		this.timeCountdown = 0f;
		base.gameObject.SetActive(true);
		base.StartCoroutine(this.DisableObj());
	}

	private IEnumerator DisableObj()
	{
		yield return new WaitForSeconds(0.5f);
		this.timeCountdown += 0.5f;
		if (this.timeCountdown >= this.timeDisable)
		{
			base.gameObject.SetActive(false);
		}
		else
		{
			base.StartCoroutine(this.DisableObj());
		}
		yield break;
	}

	public float timeDisable;

	private float timeCountdown;
}
