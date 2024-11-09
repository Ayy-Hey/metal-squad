using System;
using System.Collections;
using UnityEngine;

public class BombAirplane : CachingMonoBehaviour
{
	private void OnDisable()
	{
		GameManager.Instance.bombManager.BombAirplanePool.Store(this);
	}

	public void Init(float radius, float damage, bool isAirBomb)
	{
		this.isAirBomb = isAirBomb;
		this.mSprite.enabled = true;
		this.radius = radius;
		this.damage = damage;
		this.rigidbody2D.isKinematic = false;
		this.rigidbody2D.gravityScale = 0f;
		this.mCollider.enabled = true;
		this.speed = 0f;
		if (isAirBomb)
		{
			this.transform.eulerAngles = (this.euler = new Vector3(0f, 0f, -90f));
		}
	}

	public override void UpdateObject()
	{
		if (this.isAirBomb)
		{
			this.euler.z = Mathf.SmoothDampAngle(this.euler.z, 0f, ref this.veloSmooth, 0.5f);
			this.transform.eulerAngles = this.euler;
		}
		this.speed += Time.deltaTime * Physics2D.gravity.y;
		Vector3 v = this.transform.up * this.speed;
		this.rigidbody2D.velocity = v;
		if (!this.rigidbody2D.isKinematic && this.transform.position.y <= CameraController.Instance.BottomCamera())
		{
			base.StopAllCoroutines();
			base.StartCoroutine(this.Active(1f));
			this.rigidbody2D.isKinematic = true;
			this.mCollider.enabled = false;
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.CompareTag("Rambo"))
		{
			if (other.CompareTag("Ground") || other.CompareTag("Tank"))
			{
				base.StopAllCoroutines();
				base.StartCoroutine(this.Active(0f));
				this.rigidbody2D.isKinematic = true;
				this.mCollider.enabled = false;
			}
			return;
		}
		ISkill component = other.GetComponent<ISkill>();
		if (component != null && component.IsInVisible())
		{
			return;
		}
		this.rigidbody2D.isKinematic = true;
		this.mCollider.enabled = false;
		CameraController.Instance.Shake(CameraController.ShakeType.GrenadePlayer);
		GameManager.Instance.fxManager.ShowFxNo01(this.transform.position, 1f);
		this.mSprite.enabled = false;
		IHealth component2 = other.GetComponent<IHealth>();
		if (component2 != null)
		{
			component2.AddHealthPoint(-this.damage, EWeapon.BOMB);
		}
		base.gameObject.SetActive(false);
	}

	private IEnumerator Active(float time)
	{
		CameraController.Instance.Shake(CameraController.ShakeType.BulletPlayer);
		GameManager.Instance.fxManager.ShowFxNo01(this.transform.position, 1f);
		float dts = 0f;
		for (int i = 0; i < GameManager.Instance.ListRambo.Count; i++)
		{
			dts = Vector2.Distance(new Vector2(this.transform.position.x, this.transform.position.y), GameManager.Instance.ListRambo[i].GetPosition());
			if (dts <= 1.5f)
			{
				IHealth component = GameManager.Instance.ListRambo[i].gameObject.GetComponent<IHealth>();
				if (component != null)
				{
					component.AddHealthPoint(-this.damage, EWeapon.BOMB);
				}
			}
		}
		this.mSprite.enabled = false;
		yield return new WaitForSeconds(time);
		base.gameObject.SetActive(false);
		GameManager.Instance.hudManager.ClearEffectTarget();
		yield break;
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

	private bool isAirBomb;

	private Vector3 euler;

	private float veloSmooth;

	private float speed;
}
