using System;
using System.Collections;
using UnityEngine;

public class PreviewEffect : MonoBehaviour
{
	public void OnShow(string name)
	{
		this.anim.Play(name);
		base.StartCoroutine(this.AutoHide());
	}

	private IEnumerator AutoHide()
	{
		yield return new WaitForSeconds(0.5f);
		base.gameObject.SetActive(false);
		yield break;
	}

	private void OnDisable()
	{
		base.StopAllCoroutines();
		PreviewWeapon.Instance.PoolPreviewEffect.Store(this);
	}

	[SerializeField]
	private Animator anim;
}
