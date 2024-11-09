using System;
using System.Collections;
using UnityEngine;

public class DemoBullet1 : MonoBehaviour
{
	private void OnEnable()
	{
		this.hide = false;
		this.speed = (float)UnityEngine.Random.Range(8, 15);
		base.StartCoroutine(this.AutoHide());
	}

	private IEnumerator AutoHide()
	{
		yield return new WaitForSeconds(1f);
		this.OnHide(false);
		yield break;
	}

	private void Update()
	{
		base.transform.Translate(-Time.deltaTime * this.speed, 0f, 0f);
	}

	public void OnHide(bool attack = false)
	{
		if (!this.hide)
		{
			this.hide = true;
			if (attack)
			{
				this.ef = DemoSpecialSkillManager.instance.efBombPool.New();
				if (this.ef != null)
				{
					this.ef.transform.position = base.transform.position + new Vector3(0f, 0.5f, 0f);
					this.ef.isBombEf = false;
					this.ef.gameObject.SetActive(true);
				}
				else
				{
					MonoBehaviour.print("thieu ef");
				}
			}
			base.gameObject.SetActive(false);
			base.transform.localPosition = Vector3.zero;
			DemoSpecialSkillManager.instance.bullet1Pool.Store(this);
		}
	}

	private bool hide;

	public float speed;

	private DemoEfBomb ef;
}
