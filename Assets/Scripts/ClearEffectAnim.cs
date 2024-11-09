using System;
using System.Collections;
using UnityEngine;

public class ClearEffectAnim : MonoBehaviour
{
	private void OnEnable()
	{
		base.StopAllCoroutines();
		base.StartCoroutine(this.Hide());
	}

	private IEnumerator Hide()
	{
		yield return new WaitForSeconds(this.time);
		base.gameObject.SetActive(false);
		yield break;
	}

	public float time;
}
