using System;
using UnityEngine;

public class BulletLightning : CachingMonoBehaviour
{
	public void Init(float damage, float speed, Vector3 pos, Vector3 direction, Action<BulletLightning> hideAction)
	{
		this._damage = damage;
		this._speed = speed;
		this._direction = direction.normalized;
		this._hideAction = hideAction;
		base.gameObject.SetActive(true);
		this.transform.position = pos;
		this._coolDownHide = 5f;
		this.isInit = true;
	}

	public void OnUpdate(float deltaTime)
	{
		if (!this.isInit)
		{
			return;
		}
		this.transform.Translate(this._speed * deltaTime * this._direction);
		if (!this.renderer.isVisible)
		{
			this._coolDownHide -= deltaTime;
			if (this._coolDownHide <= 0f)
			{
				this.Hide();
			}
		}
	}

	public void Hide()
	{
		base.gameObject.SetActive(false);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Rambo"))
		{
			try
			{
				ISkill component = collision.GetComponent<ISkill>();
				if (component != null && component.IsInVisible())
				{
					return;
				}
				collision.GetComponent<IHealth>().AddHealthPoint(-this._damage, EWeapon.NONE);
			}
			catch
			{
			}
		}
		GameManager.Instance.fxManager.ShowEffNoBulletLightning(this.transform.position, Vector3.one * 2f);
		this.Hide();
	}

	private void OnDisable()
	{
		this.isInit = false;
		try
		{
			this._hideAction(this);
		}
		catch
		{
		}
	}

	[HideInInspector]
	public bool isInit;

	public Renderer renderer;

	private Action<BulletLightning> _hideAction;

	private float _damage;

	private float _speed;

	private Vector3 _direction;

	private float _coolDownHide;
}
