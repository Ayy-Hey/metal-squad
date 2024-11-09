using System;
using UnityEngine;

public class BulletEnemy : Bullet
{
	private void OnDisable()
	{
		this.SkipGround = false;
		if (this.boxCurrent != null)
		{
			this.boxCurrent.isTrigger = false;
		}
		for (int i = 0; i < this.ArrCollider.Length; i++)
		{
			this.ArrCollider[i].SetActive(false);
		}
		this.isInit = false;
		this.transform.localScale = Vector3.one;
		GameManager.Instance.bulletManager.BulletEnemyPool.Store(this);
	}

	protected void OnCollisionEnter2D(Collision2D coll)
	{
		if (this.SkipGround)
		{
			return;
		}
		IHealth component = coll.gameObject.GetComponent<IHealth>();
		if (component != null)
		{
			component.AddHealthPoint(-this.Damage, EWeapon.NONE);
		}
		switch (this.type)
		{
		case 6:
			GameManager.Instance.fxManager.ShowEffect(5, this.transform.position, Vector3.one, true, true);
			break;
		case 7:
		case 8:
			GameManager.Instance.fxManager.ShowEffect(6, this.transform.position, Vector3.one, true, true);
			break;
		default:
			GameManager.Instance.fxManager.ShowEffect(3, this.transform.position, Vector3.one, true, true);
			break;
		}
		base.gameObject.SetActive(false);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Ground") || other.CompareTag("platform"))
		{
			return;
		}
		ISkill component = other.GetComponent<ISkill>();
		if (component != null && component.IsInVisible())
		{
			return;
		}
		IHealth component2 = other.gameObject.GetComponent<IHealth>();
		if (component2 != null)
		{
			component2.AddHealthPoint(-this.Damage, EWeapon.NONE);
		}
		switch (this.type)
		{
		case 6:
			GameManager.Instance.fxManager.ShowEffect(5, this.transform.position, Vector3.one, true, true);
			break;
		case 7:
		case 8:
			GameManager.Instance.fxManager.ShowEffect(6, this.transform.position, Vector3.one, true, true);
			break;
		default:
			GameManager.Instance.fxManager.ShowEffect(3, this.transform.position, Vector3.one, true, true);
			break;
		}
		base.gameObject.SetActive(false);
	}

	public void SetSkipGround()
	{
		if (this.boxCurrent != null)
		{
			this.boxCurrent.isTrigger = true;
		}
		this.SkipGround = true;
	}

	public override void InitObject()
	{
		base.InitObject();
		this.SkipGround = false;
		this.boxCurrent = null;
		if (this._AudioM4A1 != null)
		{
			SingletonGame<AudioController>.Instance.PlaySound(this._AudioM4A1, 0.2f);
		}
	}

	public override void UpdateObject()
	{
		base.UpdateObject();
		this._coolDownHide -= Time.deltaTime;
		if (this._coolDownHide <= 0f)
		{
			base.gameObject.SetActive(false);
		}
	}

	public override void SetValue(EWeapon weapon, int type, Vector3 pos, Vector2 Direction, float Damage, float Speed, float angle = 0f)
	{
		base.SetValue(weapon, type, pos, Direction, Damage, Speed, 0f);
		this.type = type;
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
		this.ArrCollider[type].SetActive(true);
		try
		{
			this.boxCurrent = this.ArrCollider[type].GetComponent<Collider2D>();
		}
		catch
		{
		}
		this.transform.rotation = Quaternion.identity;
		this.Damage = Damage;
		this.transform.position = pos;
		Quaternion rotation = Quaternion.LookRotation(Direction.normalized, -Vector3.forward);
		rotation.x = 0f;
		rotation.y = 0f;
		rotation.eulerAngles = new Vector3(rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z + angle);
		this.transform.rotation = rotation;
		this._coolDownHide = 10f / Speed;
		this.isReady = true;
	}

	public override void Pause()
	{
		base.Pause();
	}

	public override void Resume()
	{
		base.Resume();
	}

	public GameObject[] ArrCollider;

	private int type;

	private bool SkipGround;

	private Collider2D boxCurrent;

	public AudioClip _AudioM4A1;

	private float _coolDownHide;
}
