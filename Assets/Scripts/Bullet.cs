using System;
using UnityEngine;

public class Bullet : CachingMonoBehaviour
{
	public override void InitObject()
	{
		this.isInit = true;
		this.isReady = false;
		this.useByBoss = false;
		this.Speed = 5f;
		this.counter_time_hide = 2f;
	}

	public override void UpdateObject()
	{
		if (!this.isReady)
		{
			return;
		}
		this.rigidbody2D.velocity = this.transform.up * this.Speed;
		if (!CameraController.Instance.IsInView(this.transform))
		{
			if (!this.useByBoss)
			{
				base.gameObject.SetActive(false);
			}
			this.counter_time_hide -= Time.fixedDeltaTime;
			if (this.counter_time_hide <= 0f)
			{
				base.gameObject.SetActive(false);
			}
		}
	}

	public virtual void FixedUpdateObject()
	{
	}

	public virtual void SetValue(EWeapon weapon, int type, Vector3 pos, Vector2 Direction, float Damage, float Speed, float angle = 0f)
	{
		this.transform.position = pos;
		this.Direction = Direction;
		this.Damage = Damage;
		this.Speed = Speed;
	}

	public virtual void Pause()
	{
		if (this.rigidbody2D != null)
		{
			this.rigidbody2D.isKinematic = true;
		}
	}

	public virtual void Resume()
	{
		if (this.rigidbody2D != null)
		{
			this.rigidbody2D.isKinematic = false;
		}
	}

	protected bool isReady;

	protected float Speed;

	protected float Damage;

	protected Vector2 Direction;

	public Transform Body;

	public SpriteRenderer spriteRenderer;

	public BoxCollider2D boxCollider2D;

	[HideInInspector]
	public bool isInit;

	public Sprite[] sprites;

	public Sprite[] sprites2;

	public Sprite[] sprites3;

	protected const float TIME_HIDE = 2f;

	protected float counter_time_hide;

	[HideInInspector]
	public bool useByBoss;
}
