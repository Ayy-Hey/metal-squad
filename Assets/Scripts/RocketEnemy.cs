using System;
using UnityEngine;

public class RocketEnemy : Bullet
{
	public new float Damage { get; set; }

	private void OnDisable()
	{
		for (int i = 0; i < this.arrCollider.Length; i++)
		{
			this.arrCollider[i].gameObject.SetActive(false);
		}
		this.isInit = false;
		this.isFire = false;
		GameManager.Instance.bulletManager.RocketEnemyPool.Store(this);
	}

	public override void InitObject()
	{
		base.InitObject();
		this.last_turn = 0f;
		this.turn = 2.5f;
		this.rigidbody2D.isKinematic = false;
		this.Body.gameObject.SetActive(true);
		this.boxCollider2D.enabled = true;
		if (this._AudioRocket != null)
		{
			SingletonGame<AudioController>.Instance.PlaySound(this._AudioRocket, 0.2f);
		}
	}

	private void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.layer == 19 || (coll.gameObject.layer == 10 && this.rigidbody2D.velocity.y > 0f))
		{
			return;
		}
		if (coll.CompareTag("Rambo") || coll.CompareTag("Ground"))
		{
			this.rigidbody2D.isKinematic = true;
			this.Body.gameObject.SetActive(false);
			this.boxCollider2D.enabled = false;
			GameManager.Instance.fxManager.ShowEffect((!coll.CompareTag("Rambo")) ? 0 : 6, this.transform.position, Vector3.one, true, true);
			this.isFire = false;
			base.gameObject.SetActive(false);
		}
		ISkill component = coll.GetComponent<ISkill>();
		if (component != null && component.IsInVisible())
		{
			return;
		}
		if (coll.CompareTag("Rambo"))
		{
			IHealth component2 = coll.GetComponent<IHealth>();
			if (component2 != null)
			{
				component2.AddHealthPoint(-this.Damage, EWeapon.ROCKET);
			}
			return;
		}
		for (int i = 0; i < GameManager.Instance.ListRambo.Count; i++)
		{
			float num = Vector2.Distance(new Vector2(this.transform.position.x, this.transform.position.y), GameManager.Instance.ListRambo[i].Origin());
			if (num < 1.5f)
			{
				IHealth component3 = GameManager.Instance.ListRambo[i].gameObject.GetComponent<IHealth>();
				if (component3 != null)
				{
					component3.AddHealthPoint(-this.Damage, EWeapon.ROCKET);
				}
			}
		}
	}

	public override void UpdateObject()
	{
	}

	public override void FixedUpdateObject()
	{
		if (!this.isFire)
		{
			return;
		}
		if (this.transform.position.y <= CameraController.Instance.transform.position.y - 7f || this.transform.position.y >= CameraController.Instance.transform.position.y + 7f)
		{
			base.gameObject.SetActive(false);
		}
		if (Vector3.Distance(this.transform.position, this.target) > 0.5f && this.transform.position.y >= this.target.y)
		{
			Quaternion b = Quaternion.LookRotation(this.transform.position - this.target, Vector3.forward);
			b.x = 0f;
			b.y = 0f;
			this.transform.rotation = Quaternion.Slerp(this.transform.rotation, b, Time.deltaTime * this.turn);
			this.last_turn += Time.deltaTime * Time.deltaTime;
			this.turn += this.last_turn;
		}
		this.rigidbody2D.velocity = this.transform.up * this.speed;
	}

	public void SetFire(int type, Vector3 target, float damage, int angle = 0)
	{
		this.isFire = true;
		this.target = target;
		this.Damage = damage;
		this.transform.eulerAngles = new Vector3(0f, 0f, (float)angle);
		this.arrCollider[type].gameObject.SetActive(true);
		GameMode.Mode emode = GameMode.Instance.EMode;
		if (emode != GameMode.Mode.NORMAL)
		{
			if (emode != GameMode.Mode.HARD)
			{
				if (emode == GameMode.Mode.SUPER_HARD)
				{
					this.spriteRenderer.sprite = this.sprites3[type];
				}
			}
			else
			{
				this.spriteRenderer.sprite = this.sprites2[type];
			}
		}
		else
		{
			this.spriteRenderer.sprite = this.sprites[type];
		}
	}

	public override void Resume()
	{
		base.Resume();
	}

	public override void Pause()
	{
		base.Pause();
	}

	private bool isFire;

	private Vector3 target;

	private float speed = 5f;

	private float turn = 2.5f;

	private float last_turn;

	public Collider2D[] arrCollider;

	public AudioClip _AudioRocket;
}
