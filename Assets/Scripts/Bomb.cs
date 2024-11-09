using System;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : CachingMonoBehaviour
{
	public float Damage { get; set; }

	private void OnDisable()
	{
		this.isReady = false;
		this.isInit = false;
		this.rigidbody2D.isKinematic = false;
		this.Body.gameObject.SetActive(true);
		if (this.audioClip)
		{
			SingletonGame<AudioController>.Instance.PlaySound(this.audioClip, 0.2f);
		}
		for (int i = 0; i < this.ArrCollider.Length; i++)
		{
			this.ArrCollider[i].SetActive(false);
		}
		try
		{
			CameraController.Instance.Shake(CameraController.ShakeType.ShakeEnemy);
			GameManager.Instance.bombManager.bombPool.Store(this);
		}
		catch
		{
		}
	}

	private void ClearRambo()
	{
		List<BaseCharactor> listRambo = GameManager.Instance.ListRambo;
		for (int i = 0; i < listRambo.Count; i++)
		{
			BaseCharactor baseCharactor = listRambo[i];
			if (!(baseCharactor == null) && baseCharactor.gameObject.activeSelf)
			{
				float num = Vector2.Distance(this.transform.position, baseCharactor.Origin());
				if (num <= this.radius_damage)
				{
					IHealth component = baseCharactor.GetComponent<IHealth>();
					if (component != null)
					{
						component.AddHealthPoint(-this.Damage, EWeapon.GRENADE_M61);
					}
				}
			}
		}
	}

	private void ClearEnemies()
	{
		List<BaseEnemy> listEnemy = GameManager.Instance.ListEnemy;
		for (int i = 0; i < listEnemy.Count; i++)
		{
			BaseEnemy baseEnemy = listEnemy[i];
			if (!(baseEnemy == null) && baseEnemy.gameObject.activeSelf)
			{
				float num = Vector2.Distance(this.transform.position, baseEnemy.Origin()) - baseEnemy.Radius;
				if (num <= this.radius_damage)
				{
					IHealth component = baseEnemy.GetComponent<IHealth>();
					if (component != null)
					{
						component.AddHealthPoint(-this.Damage, EWeapon.GRENADE_M61);
					}
				}
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D coll)
	{
		if (!this.isReady)
		{
			return;
		}
		string tag = this.transform.tag;
		if (coll.CompareTag("Explosive"))
		{
			IHealth component = coll.GetComponent<IHealth>();
			if (component != null)
			{
				component.AddHealthPoint(-this.Damage, EWeapon.BOMB);
			}
		}
		int num = 0;
		ELevel level = GameManager.Instance.Level;
		switch (level)
		{
		case ELevel.LEVEL_27:
		case ELevel.LEVEL_28:
		case ELevel.LEVEL_30:
			break;
		default:
			if (level != ELevel.LEVEL_21)
			{
				goto IL_7D;
			}
			break;
		}
		num = 1;
		IL_7D:
		if (GameManager.Instance.isJetpackMode)
		{
			num = 1;
		}
		this.isReady = false;
		if (coll.CompareTag("Ground") || (CameraController.Instance.orientaltion == CameraController.Orientation.VERTICAL && coll.CompareTag("Obstacle") && this.rigidbody2D.velocity.y < 0f))
		{
			this.objAttack.SetActive(true);
			this.ClearRambo();
			this.rigidbody2D.isKinematic = true;
			this.Body.gameObject.SetActive(false);
			if (num != 0)
			{
				if (num == 1)
				{
					GameManager.Instance.fxManager.ShowExplosionSpine(this.transform.position, num);
				}
			}
			else
			{
				GameManager.Instance.fxManager.CreateGrenadeBasic(this.transform.position).transform.localScale = new Vector3(2f, 2f, 2f);
			}
			this.objAttack.SetActive(false);
			base.gameObject.SetActive(false);
		}
	}

	public override void InitObject()
	{
		this.Body.gameObject.SetActive(true);
		this.objAttack.SetActive(false);
		this.isInit = true;
		this.isReady = false;
	}

	public override void UpdateObject()
	{
		if (!this.isReady && this.rigidbody2D.velocity.y < 0f)
		{
			this.isReady = true;
		}
		this.Body.Rotate(0f, 0f, 5f);
	}

	public void Throw(bool isRight, int type, float force, float damage, float radius_damage)
	{
		base.tag = "BombEnemy";
		this.ArrCollider[type].SetActive(true);
		this.mSprite.sprite = this.ArrSprite[type];
		this.radius_damage = radius_damage;
		this.Damage = damage;
		float d = force * this.rigidbody2D.mass;
		this.rigidbody2D.AddForce(new Vector2((!isRight) ? 0.7f : -0.7f, 0.8f) * d);
		this.isReady = true;
	}

	public void SetPause()
	{
		this.savedVelocity = this.rigidbody2D.velocity;
		this.savedAngularVelocity = this.rigidbody2D.angularVelocity;
		this.rigidbody2D.velocity = Vector2.zero;
		this.rigidbody2D.isKinematic = true;
	}

	public void SetResume()
	{
		this.rigidbody2D.isKinematic = false;
		this.rigidbody2D.AddForce(this.savedVelocity, ForceMode2D.Impulse);
		this.rigidbody2D.AddTorque(this.savedAngularVelocity, ForceMode2D.Impulse);
	}

	private Vector3 savedVelocity;

	private float savedAngularVelocity;

	private bool isReady;

	private float radius_damage;

	public Transform Body;

	public GameObject objAttack;

	public SpriteRenderer mSprite;

	public Sprite[] ArrSprite;

	public GameObject[] ArrCollider;

	public AudioClip audioClip;

	[HideInInspector]
	public bool isInit;
}
