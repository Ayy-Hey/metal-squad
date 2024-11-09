using System;
using UnityEngine;

public class BulletBoss4_1 : CachingMonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Rambo"))
		{
			ISkill component = other.GetComponent<ISkill>();
			if (component != null && component.IsInVisible())
			{
				return;
			}
			IHealth component2 = other.GetComponent<IHealth>();
			if (!object.ReferenceEquals(component2, null))
			{
				component2.AddHealthPoint(-this.DAMAGE * GameMode.Instance.GetMode(), EWeapon.ROCKET);
			}
			base.gameObject.SetActive(false);
		}
	}

	private void OnDisable()
	{
		this.isShoot = false;
		try
		{
			this.callback(this);
			if (!this.noBump)
			{
				GameManager.Instance.fxManager.ShowEffect(4, this.transform.position, Vector3.one, true, true);
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(base.name + " Message:  " + ex.Message);
		}
		this.noBump = false;
	}

	public override void UpdateObject()
	{
		if (!this.isShoot)
		{
			return;
		}
		this.rigidbody2D.MovePosition(this.rigidbody2D.position + this.direction * Time.deltaTime * this.SPEED);
		if (Time.timeSinceLevelLoad - this.TimeDestroy >= 5f)
		{
			this.noBump = true;
			base.gameObject.SetActive(false);
		}
	}

	public void Shoot(Vector2 pos, Vector2 direction, float Damage, float Speed_Bullet, Action<BulletBoss4_1> callback)
	{
		this.callback = callback;
		this.DAMAGE = Damage;
		this.SPEED = Speed_Bullet;
		this.isShoot = true;
		this.rigidbody2D.position = pos;
		this.direction = direction;
		this.direction.Normalize();
		this.TimeDestroy = Time.timeSinceLevelLoad;
		if (ProfileManager.settingProfile.IsSound)
		{
			this.audioBullet.Play();
		}
	}

	private float SPEED = 2.5f;

	private float DAMAGE = 50f;

	private Vector2 direction;

	private bool isShoot;

	public LayerMask layerMask;

	private bool noBump;

	public AudioSource audioBullet;

	private float TimeDestroy;

	private Action<BulletBoss4_1> callback;
}
