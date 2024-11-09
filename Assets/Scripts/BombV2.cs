using System;
using UnityEngine;

public class BombV2 : CachingMonoBehaviour
{
	private void OnDisable()
	{
		try
		{
			GameManager.Instance.bombManager.BombPool.Store(this);
		}
		catch
		{
		}
	}

	public void OnInit(float Damage)
	{
		this.anim.SetTrigger("Normal");
		this.anim.Play(0);
		this.bombV2_Toxic.gameObject.SetActive(false);
		this.isInit = false;
		this.Damage = Damage;
	}

	public void OnUpdate()
	{
		if (!this.isInit)
		{
			return;
		}
		this.TimeCountDown -= Time.deltaTime;
		if (this.TimeCountDown <= 0f && !this.isActiveBomb)
		{
			GameManager.Instance.fxManager.ShowExplosionSpine(this.transform.position, 1);
			Collider2D[] array = Physics2D.OverlapCircleAll(this.transform.position, 1.5f, this.layerMask);
			for (int i = 0; i < array.Length; i++)
			{
				IHealth component = array[i].GetComponent<IHealth>();
				if (component != null)
				{
					component.AddHealthPoint(-this.Damage, EWeapon.BOMB);
				}
			}
			this.isActiveBomb = true;
			base.gameObject.SetActive(false);
		}
	}

	private void OnCollisionEnter2D(Collision2D coll)
	{
		this.bombV2_Toxic.gameObject.SetActive(true);
		this.anim.SetTrigger("Active");
		this.anim.Play(0);
		this.TimeCountDown = 5f;
		this.isInit = true;
		this.isActiveBomb = false;
	}

	[SerializeField]
	private Animator anim;

	[SerializeField]
	private BombV2_Toxic bombV2_Toxic;

	private bool isInit;

	private bool isActiveBomb;

	private float TimeCountDown = 5f;

	[SerializeField]
	private LayerMask layerMask;

	private float Damage = 50f;
}
