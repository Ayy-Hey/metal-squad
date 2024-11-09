using System;
using UnityEngine;

public class BulletBossMini : Bullet
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		GameManager.Instance.fxManager.ShowEffect(3, this.transform.position, Vector3.one, true, true);
		if (!collision.gameObject.CompareTag("Rambo"))
		{
			return;
		}
		IHealth component = collision.gameObject.GetComponent<IHealth>();
		if (component != null)
		{
			component.AddHealthPoint(-this.Damage, EWeapon.NONE);
		}
		base.gameObject.SetActive(false);
	}

	private void Update()
	{
		if (GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
		if (!this.isReady)
		{
			return;
		}
		this.rigidbody2D.velocity = -this.transform.right * this.Speed;
		if (!CameraController.Instance.IsInView(this.transform))
		{
			base.gameObject.SetActive(false);
		}
	}

	public override void SetValue(EWeapon weapon, int type, Vector3 pos, Vector2 Direction, float Damage, float Speed, float angle = 0f)
	{
		base.SetValue(weapon, type, pos, Direction, Damage, Speed, angle);
		this.isReady = true;
	}

	private void OnDisable()
	{
		try
		{
			if (GameManager.Instance.StateManager.EState == EGamePlay.RUNNING)
			{
				this.isInit = false;
				this.Hide();
			}
		}
		catch
		{
		}
	}

	private void Hide()
	{
		this.rigidbody2D.velocity = Vector2.zero;
		this.transform.localPosition = Vector3.zero;
	}

	public Action<BulletBossMini> Off;
}
