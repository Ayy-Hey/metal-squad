using System;
using System.Collections;
using UnityEngine;

public class DemoEfBomb : MonoBehaviour
{
	private void OnEnable()
	{
		this.hide = false;
		if (this.anim == null)
		{
			this.anim = base.GetComponent<Animator>();
		}
		base.transform.localScale = Vector3.one;
		if (this.isBombEf)
		{
			this.anim.Play("explosion");
			this.timeDelay = 0.8f;
		}
		else
		{
			this.anim.Play("bomno4");
			this.timeDelay = 0.5f;
			base.transform.localScale /= 2f;
		}
		base.StartCoroutine(this.AutoHide());
	}

	private IEnumerator AutoHide()
	{
		yield return new WaitForSeconds(this.timeDelay);
		this.OnHide();
		yield break;
	}

	public void OnHide()
	{
		if (!this.hide)
		{
			this.hide = true;
			base.gameObject.SetActive(false);
			DemoSpecialSkillManager.instance.efBombPool.Store(this);
		}
	}

	public Animator anim;

	public bool isBombEf;

	private float timeDelay;

	private bool hide = true;
}
