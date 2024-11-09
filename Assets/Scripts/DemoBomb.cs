using System;
using System.Collections;
using UnityEngine;

public class DemoBomb : MonoBehaviour
{
	private void OnEnable()
	{
		this.isHide = false;
		base.StartCoroutine(this.AutoHide());
	}

	private IEnumerator AutoHide()
	{
		yield return new WaitForSeconds(2f);
		this.OnHide();
		yield break;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		this.ef = DemoSpecialSkillManager.instance.efBombPool.New();
		this.ef.transform.localPosition = base.transform.localPosition + Vector3.up * 1.2f;
		this.ef.isBombEf = true;
		this.ef.gameObject.SetActive(true);
		this.OnHide();
	}

	public void OnHide()
	{
		if (!this.isHide)
		{
			this.isHide = true;
			base.gameObject.SetActive(false);
			base.transform.localPosition = Vector3.zero;
			DemoSpecialSkillManager.instance.bombPool.Store(this);
		}
	}

	private bool isHide = true;

	private DemoEfBomb ef;
}
