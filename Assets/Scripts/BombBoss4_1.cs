using System;
using UnityEngine;

public class BombBoss4_1 : CachingMonoBehaviour
{
	private void OnDisable()
	{
		this.isInit = false;
		GameManager.Instance.fxManager.ShowEffect(6, this.transform.position, Vector3.one, true, true);
		this.TimeAutoBump = float.MaxValue;
		GameManager.Instance.bombManager.BombBoss4_1Pool.Store(this);
		Collider2D collider2D = Physics2D.OverlapCircle(this.transform.position, 2f, this.layerMask);
		try
		{
			if (collider2D != null)
			{
				ISkill component = collider2D.GetComponent<ISkill>();
				if (component == null || !component.IsInVisible())
				{
					collider2D.GetComponent<IHealth>().AddHealthPoint(-this.Damage, EWeapon.NONE);
				}
			}
		}
		catch
		{
		}
	}

	public void OnInit()
	{
		this.TimeAutoBump = Time.timeSinceLevelLoad;
		this.isInit = true;
	}

	public void OnUpdate()
	{
		if (!this.isInit)
		{
			return;
		}
		if (Time.timeSinceLevelLoad - this.TimeAutoBump >= 5f)
		{
			base.gameObject.SetActive(false);
		}
	}

	private void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.CompareTag("Rambo"))
		{
			base.gameObject.SetActive(false);
		}
		if (coll.gameObject.CompareTag("Ground"))
		{
			this.TimeAutoBump = Time.timeSinceLevelLoad;
			if (ProfileManager.settingProfile.IsSound)
			{
				this.audioGround.Play();
			}
		}
	}

	private float Damage = 120f;

	private float TimeAutoBump = float.MaxValue;

	public AudioSource audioGround;

	private bool isInit;

	[SerializeField]
	private LayerMask layerMask;
}
