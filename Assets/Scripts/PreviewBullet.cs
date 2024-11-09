using System;
using UnityEngine;

public class PreviewBullet : CachingMonoBehaviour
{
	public void OnShoot(ETypeWeapon EWeapon, int rank, int IDWeapon, Vector2 Direction, float offsetRotationZ)
	{
		this.transform.localScale = Vector3.one;
		this.CountEnemy = 0;
		this.isTouchEnemy = false;
		this.EWeapon = EWeapon;
		this.IDWeapon = IDWeapon;
		this.transform.rotation = Quaternion.identity;
		Quaternion rotation = Quaternion.LookRotation(Direction, -Vector3.forward);
		rotation.x = 0f;
		rotation.y = 0f;
		rotation.eulerAngles = new Vector3(rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z + offsetRotationZ);
		this.transform.rotation = rotation;
		if (EWeapon != ETypeWeapon.PRIMARY)
		{
			if (EWeapon == ETypeWeapon.SPECIAL)
			{
				this.spriteCurrent.sprite = this.spriteBulletSpecial[IDWeapon].spriteBullet[rank];
				if (IDWeapon == 4)
				{
					this.transform.localScale = Vector3.one * 0.8f;
					this.objParticleSmoke.SetActive(true);
				}
			}
		}
		else
		{
			this.spriteCurrent.sprite = this.spriteBulletPrimary[IDWeapon].spriteBullet[rank];
			if (IDWeapon == 4)
			{
				this.objParticleSmoke.SetActive(true);
			}
		}
	}

	public void SetTarget(Transform target)
	{
		this.last_turn = 0f;
		this.turn = 2.5f;
		this.speed = 5f;
		this.target = target;
	}

	public void OnUpdate(float deltaTime)
	{
		if (this.EWeapon == ETypeWeapon.SPECIAL && this.IDWeapon == 4)
		{
			try
			{
				Quaternion b = Quaternion.LookRotation(this.transform.position - this.target.position, Vector3.forward);
				b.x = 0f;
				b.y = 0f;
				this.transform.rotation = Quaternion.Slerp(this.transform.rotation, b, deltaTime * this.turn);
				this.last_turn += deltaTime * deltaTime * 50f;
				this.turn += this.last_turn;
				this.rigidbody2D.velocity = this.transform.up * this.speed;
			}
			catch
			{
			}
			return;
		}
		this.rigidbody2D.velocity = this.transform.up * deltaTime * 50f * 10f;
	}

	private void OnDisable()
	{
		switch (this.EWeapon)
		{
		case ETypeWeapon.PRIMARY:
			switch (this.IDWeapon)
			{
			case 0:
			case 1:
			case 3:
				PreviewWeapon.Instance.CreateFxBullet(this.transform.position);
				break;
			case 2:
				if (Time.timeSinceLevelLoad - PreviewWeapon.Instance.timeCounter > 0.3f)
				{
					PreviewWeapon.Instance.timeCounter = Time.timeSinceLevelLoad;
					PreviewWeapon.Instance.CreateEffect(this.transform.position, "ice");
				}
				break;
			case 4:
				PreviewWeapon.Instance.CreateEffect(this.transform.position, "bomno5");
				this.objParticleSmoke.SetActive(false);
				break;
			}
			break;
		case ETypeWeapon.SPECIAL:
		{
			int idweapon = this.IDWeapon;
			if (idweapon != 1)
			{
				if (idweapon == 4)
				{
					PreviewWeapon.Instance.CreateEffect(this.transform.position, "bomno5");
					this.objParticleSmoke.SetActive(false);
				}
			}
			else
			{
				PreviewWeapon.Instance.CreateFxBullet(this.transform.position);
			}
			break;
		}
		}
		PreviewWeapon.Instance.PoolPreviewBullet.Store(this);
	}

	protected void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.CompareTag("Enemy"))
		{
			this.isTouchEnemy = true;
			if (this.EWeapon == ETypeWeapon.SPECIAL && this.IDWeapon == 1)
			{
				Vector3 position = coll.transform.position;
				this.CountEnemy++;
				if (this.CountEnemy > 1)
				{
					base.gameObject.SetActive(false);
				}
			}
			else
			{
				base.gameObject.SetActive(false);
			}
			if (this.EWeapon == ETypeWeapon.PRIMARY && this.IDWeapon == 2)
			{
				PreviewEnemy component = coll.GetComponent<PreviewEnemy>();
				if (component != null)
				{
					component.ShowIce();
				}
			}
			return;
		}
		if (coll.CompareTag("Ground"))
		{
			if (this.EWeapon == ETypeWeapon.SPECIAL && this.IDWeapon == 4)
			{
				return;
			}
			base.gameObject.SetActive(false);
		}
	}

	public SpriteRenderer spriteCurrent;

	public PreviewBullet.SpriteBullet[] spriteBulletPrimary;

	public PreviewBullet.SpriteBullet[] spriteBulletSpecial;

	private ETypeWeapon EWeapon;

	private int IDWeapon;

	private bool isTouchEnemy;

	private int CountEnemy;

	public GameObject objParticleSmoke;

	private Transform target;

	private float turn = 2.5f;

	private float last_turn;

	private float speed;

	[Serializable]
	public class SoundBullet
	{
		public AudioClip _audio;

		public float volume;
	}

	[Serializable]
	public struct SpriteBullet
	{
		public Sprite[] spriteBullet;
	}
}
