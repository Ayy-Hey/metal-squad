using System;
using UnityEngine;

public class RainBombSkill : CachingMonoBehaviour
{
	private void OnDisable()
	{
		try
		{
			GameManager.Instance.fxManager.ShowFxNo01(this.transform.position, 1f);
			this.isInit = false;
			Collider2D[] array = Physics2D.OverlapCircleAll(this.transform.position, this.radius, this.layerMask);
			for (int i = 0; i < array.Length; i++)
			{
				BaseEnemy component = array[i].GetComponent<BaseEnemy>();
				if (component != null && component.isInCamera)
				{
					component.AddHealthPoint(-this.damage, EWeapon.NONE);
				}
			}
			GameManager.Instance.skillManager.PoolRainBombSkill.Store(this);
		}
		catch (Exception ex)
		{
		}
	}

	public void Init(Vector2 pos, bool hasDamage = true)
	{
		this.countOverlap = 0;
		this.isInit = true;
		this.mSprite.enabled = true;
		this.transform.position = pos;
		this.radius = 3f;
		if (hasDamage)
		{
			this.damage = DataLoader.characterData[2].skills[0].Damage[ProfileManager.rambos[2].LevelUpgrade];
		}
		else
		{
			this.damage = 0f;
		}
		if (GameMode.Instance.MODE == GameMode.Mode.TUTORIAL)
		{
			this.damage = 300f;
		}
		this.rigidbody2D.isKinematic = false;
		this.mCollider.enabled = true;
	}

	public override void UpdateObject()
	{
		if (!this.isInit)
		{
			return;
		}
		base.UpdateObject();
		if (!this.rigidbody2D.isKinematic && this.transform.position.y <= CameraController.Instance.BottomCamera())
		{
			this.rigidbody2D.isKinematic = true;
			this.mCollider.enabled = false;
			base.gameObject.SetActive(false);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Enemy") || other.CompareTag("Tank") || other.CompareTag("Boss"))
		{
			this.rigidbody2D.isKinematic = true;
			this.mCollider.enabled = false;
			CameraController.Instance.Shake(CameraController.ShakeType.BulletPlayer);
			this.mSprite.enabled = false;
			base.gameObject.SetActive(false);
			return;
		}
		bool flag = other.CompareTag("Ground") || (other.CompareTag("Obstacle") && UnityEngine.Random.Range(0f, 1f) > 0.5f && this.countOverlap < 1);
		if (flag)
		{
			this.countOverlap++;
			CameraController.Instance.Shake(CameraController.ShakeType.BulletPlayer);
			this.rigidbody2D.isKinematic = true;
			this.mCollider.enabled = false;
			base.gameObject.SetActive(false);
		}
	}

	public void SetPause()
	{
		this.rigidbody2D.isKinematic = true;
	}

	public void SetResume()
	{
		this.rigidbody2D.isKinematic = false;
	}

	private float radius;

	private float damage;

	public SpriteRenderer mSprite;

	public BoxCollider2D mCollider;

	public LayerMask layerMask;

	private bool isInit;

	private int countOverlap;
}
