using System;
using UnityEngine;

public class BulletWithTail : Bullet
{
	public void TryAwake()
	{
		this.transform = base.GetComponent<Transform>();
		this.rigidbody2D = base.GetComponent<Rigidbody2D>();
	}

	private void OnDisable()
	{
		GameManager.Instance.fxManager.ShowEffect(UnityEngine.Random.Range(4, 6), this.transform.position, 0.8f * Vector3.one, UnityEngine.Random.Range(0f, 1f) > 5f, true);
		Collider2D[] array = Physics2D.OverlapCircleAll(this.transform.position, 1f, this.layerMask);
		for (int i = 0; i < array.Length; i++)
		{
			BaseEnemy component = array[i].GetComponent<BaseEnemy>();
			if (component != null && component.isInCamera)
			{
				component.AddHealthPoint(-this.Damage, EWeapon.M4A1);
			}
		}
		try
		{
			GameManager.Instance.bulletManager.PoolBulletTail[this.typeBullet].Store(this);
		}
		catch
		{
		}
	}

	public override void InitObject()
	{
		base.InitObject();
	}

	public void OnUpdate(float deltaTime)
	{
		if (!this.isReady)
		{
			return;
		}
		Vector3 a = new Vector3(this.Direction.x, this.Direction.y);
		a.Normalize();
		this.transform.Translate(a * deltaTime * this.Speed);
		if (!CameraController.Instance.IsInView(this.transform))
		{
			base.gameObject.SetActive(false);
		}
	}

	public void SetValue(Vector3 pos, Vector2 Direction, float Damage, float Speed)
	{
		base.SetValue(EWeapon.NONE, -1, pos, Direction, Damage, Speed, 0f);
		this.tfChild.rotation = Quaternion.identity;
		this.Damage = Damage;
		this.transform.position = pos;
		Quaternion rotation = Quaternion.LookRotation(Direction.normalized, Vector3.forward);
		rotation.x = 0f;
		rotation.y = 0f;
		rotation.eulerAngles = new Vector3(rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z + 90f);
		this.tfChild.rotation = rotation;
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

	[SerializeField]
	private LayerMask layerMask;

	public int typeBullet;

	public Transform tfChild;
}
