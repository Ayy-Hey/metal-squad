using System;
using UnityEngine;

public class BulletBeSung : CachingMonoBehaviour
{
	public void Init(float speed, float damage, float scale, Vector3 pos, Vector3 direction, Action hideAction = null)
	{
		this.hideAction = hideAction;
		this.speed = speed;
		this.damage = damage;
		base.gameObject.SetActive(true);
		this.transform.position = pos;
		this.transform.localScale = Vector3.one * scale;
		this.transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
		this.coolDownHide = 3f;
		if (this.clip)
		{
			SingletonGame<AudioController>.Instance.PlaySound(this.clip, 0.5f);
		}
		this.isInit = true;
	}

	public void OnUpdate(float deltaTime)
	{
		if (!this.isInit)
		{
			return;
		}
		if (this.coolDownHide > 0f)
		{
			this.coolDownHide -= deltaTime;
		}
		else
		{
			base.gameObject.SetActive(false);
		}
		this.transform.Translate(this.speed * deltaTime * this.transform.up, Space.World);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		base.gameObject.SetActive(false);
		GameManager.Instance.fxManager.ShowEffect(3, this.transform.position, Vector3.one, true, true);
		if (collision.CompareTag("Rambo"))
		{
			try
			{
				collision.GetComponent<IHealth>().AddHealthPoint(-this.damage, EWeapon.NONE);
			}
			catch
			{
			}
		}
	}

	private void OnDisable()
	{
		try
		{
			this.isInit = false;
			this.hideAction();
		}
		catch
		{
		}
	}

	public bool isInit;

	[SerializeField]
	private AudioClip clip;

	private Action hideAction;

	private float speed;

	private float damage;

	private float coolDownHide;
}
