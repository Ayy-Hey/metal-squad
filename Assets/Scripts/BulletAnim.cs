using System;
using System.Collections;
using UnityEngine;

public class BulletAnim : CachingMonoBehaviour
{
	public float Damage { get; set; }

	private void OnDisable()
	{
		this.isInit = false;
		this.ColliderAttack.SetActive(false);
		this.TypeBullet = ETypeBullet.NONE;
		for (int i = 0; i < this.ArrCollider.Length; i++)
		{
			this.ArrCollider[i].SetActive(false);
		}
		GameManager.Instance.bulletManager.BulletAnimPool.Store(this);
	}

	public override void InitObject()
	{
		base.InitObject();
	}

	public override void UpdateObject()
	{
		if (this.TypeBullet == ETypeBullet.NONE || !this.isInit)
		{
			return;
		}
		if (!CameraController.Instance.IsInView(this.transform))
		{
			base.gameObject.SetActive(false);
			return;
		}
		switch (this.TypeBullet)
		{
		case ETypeBullet.BOSS_1_1:
		case ETypeBullet.BOSS_1_2:
		case ETypeBullet.BOSS_3_3:
			this.rigidbody2D.MovePosition(this.rigidbody2D.position + this.Speed * Time.deltaTime * this.direction);
			break;
		case ETypeBullet.BOSS_1_3:
		{
			Vector3 a = new Vector3(this.direction.x, this.direction.y);
			this.pos += a * Time.deltaTime * this.MoveSpeed;
			this.rigidbody2D.position = this.pos + this.transform.up * Mathf.Sin(Time.time * this.frequency) * this.magnitude;
			break;
		}
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Ground") || (other.CompareTag("Rambo") && this.isInit))
		{
			this.isInit = false;
			this.rigidbody2D.isKinematic = true;
			this.rigidbody2D.velocity = Vector2.zero;
			this.mSprite.enabled = false;
			Vector3 vector = new Vector3(0f, 0f, 0f);
			ISkill component = other.GetComponent<ISkill>();
			switch (this.TypeBullet)
			{
			case ETypeBullet.BOSS_1_1:
			{
				IHealth component2 = other.GetComponent<IHealth>();
				if (component2 != null)
				{
					if (component != null && component.IsInVisible())
					{
						return;
					}
					component2.AddHealthPoint(-this.Damage, EWeapon.BOMB);
				}
				GameManager.Instance.fxManager.ShowEffect((!other.CompareTag("Ground")) ? 5 : 0, this.transform.position, Vector3.one, true, true);
				this.ColliderAttack.SetActive(true);
				base.StartCoroutine(this.Hide());
				GameManager.Instance.audioManager.PlayBoom();
				break;
			}
			case ETypeBullet.BOSS_1_2:
			{
				IHealth component2 = other.GetComponent<IHealth>();
				if (component2 != null)
				{
					if (component != null && component.IsInVisible())
					{
						return;
					}
					component2.AddHealthPoint(-this.Damage, EWeapon.BOMB);
				}
				GameManager.Instance.fxManager.ShowEffect((!other.CompareTag("Ground")) ? 4 : 0, this.transform.position, Vector3.one, true, true);
				base.gameObject.SetActive(false);
				GameManager.Instance.audioManager.PlayBoom();
				break;
			}
			case ETypeBullet.BOSS_1_3:
			case ETypeBullet.BOSS_3_3:
				if (other.CompareTag("Rambo"))
				{
					if (component != null && component.IsInVisible())
					{
						return;
					}
					BaseRambo component3 = other.GetComponent<BaseRambo>();
					if (component3 != null)
					{
						component3.TrungDoc();
					}
				}
				base.gameObject.SetActive(false);
				break;
			}
		}
	}

	private IEnumerator Hide()
	{
		yield return new WaitForSeconds(0.5f);
		base.gameObject.SetActive(false);
		this.ColliderAttack.SetActive(false);
		yield break;
	}

	public void Setup(ETypeBullet type)
	{
		this.TypeBullet = type;
		this.mSprite.enabled = true;
		for (int i = 0; i < this.ArrCollider.Length; i++)
		{
			if (type == (ETypeBullet)i)
			{
				this.ArrCollider[i].SetActive(true);
			}
			else
			{
				this.ArrCollider[i].SetActive(false);
			}
		}
		switch (type)
		{
		case ETypeBullet.BOSS_1_1:
			this.mAnimator.SetTrigger("boss_1_1");
			break;
		case ETypeBullet.BOSS_1_2:
			this.mAnimator.SetTrigger("boss_1_2");
			break;
		case ETypeBullet.BOSS_1_3:
		case ETypeBullet.BOSS_3_3:
			this.ArrCollider[2].SetActive(true);
			this.mAnimator.SetTrigger("boss_1_3");
			break;
		}
	}

	public void SetValue(Vector3 pos, Vector2 direction, float Damage, float Speed)
	{
		this.rigidbody2D.isKinematic = false;
		this.transform.position = pos;
		this.pos = pos;
		this.direction = direction;
		this.Damage = Damage;
		this.Speed = Speed;
		this.isInit = true;
	}

	private const string Boss1_1 = "boss_1_1";

	private const string Boss1_2 = "boss_1_2";

	private const string Boss1_3 = "boss_1_3";

	private float MoveSpeed = 4f;

	private float frequency = 4f;

	private float magnitude = 1f;

	private Vector3 pos;

	private Vector2 direction;

	private float Speed;

	private bool isInit;

	[Header("Unity")]
	public Animator mAnimator;

	public GameObject[] ArrCollider;

	public GameObject ColliderAttack;

	public SpriteRenderer mSprite;

	public ETypeBullet TypeBullet = ETypeBullet.NONE;
}
