using System;
using UnityEngine;

public class BombCicle1 : CachingMonoBehaviour
{
	public void Init(float damage, float delay, Vector3 pos, float size, Action<BombCicle1> callback)
	{
		this.callback = callback;
		this._damage = damage;
		this._delay = delay;
		this._speed = 0f;
		this._size = size;
		this._obstacleContact = 0;
		this._firstCollision = (this._secondCollision = false);
		base.gameObject.SetActive(true);
		this.transform.position = pos;
		this.transform.localScale = Vector3.one * size;
		this.animation["Bomb_Cicle_1"].speed = 1f;
		this.isInit = true;
	}

	public void OnUpdate(float deltaTime)
	{
		if (!this.isInit)
		{
			return;
		}
		if (this._secondCollision)
		{
			this._delay -= deltaTime;
			if (this._delay <= 0f)
			{
				for (int i = 0; i < GameManager.Instance.ListRambo.Count; i++)
				{
					bool flag = Vector3.Distance(this.transform.position, GameManager.Instance.ListRambo[i].transform.position) < this._size * 2f;
					if (flag)
					{
						try
						{
							GameManager.Instance.ListRambo[i].GetComponent<IHealth>().AddHealthPoint(-this._damage, EWeapon.NONE);
						}
						catch
						{
						}
					}
					GameManager.Instance.fxManager.ShowFxNo01(this.transform.position, this._size);
					base.gameObject.SetActive(false);
				}
			}
			return;
		}
		this.transform.Rotate(0f, 0f, -1f);
		this.transform.Translate(0f, this._speed * deltaTime, 0f, Space.World);
		this._speed += this.g * deltaTime;
		if (this._speed < -15f)
		{
			base.gameObject.SetActive(false);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (this.obstacleMask.Contains(collision.gameObject.layer))
		{
			this._obstacleContact++;
			bool flag = this._obstacleContact == 1 && UnityEngine.Random.Range(0, 3) == 1;
			if (flag)
			{
				return;
			}
		}
		bool flag2 = collision.CompareTag("Ground") || collision.CompareTag("Obstacle");
		if (flag2)
		{
			if (!this._firstCollision)
			{
				this._firstCollision = true;
				this._speed = UnityEngine.Random.Range(2f, 3f);
			}
			else if (!this._secondCollision && this._speed < 0f)
			{
				this._secondCollision = true;
				this._speed = 0f;
				this.animation["Bomb_Cicle_1"].speed = 2f;
			}
		}
	}

	private void OnDisable()
	{
		try
		{
			this.isInit = false;
			this.callback(this);
		}
		catch
		{
		}
	}

	[HideInInspector]
	public bool isInit;

	[SerializeField]
	[Tooltip("gia tốc:")]
	private float g = -5f;

	[SerializeField]
	private Animation animation;

	[SerializeField]
	private LayerMask obstacleMask;

	private float _damage;

	private Action<BombCicle1> callback;

	private float _speed;

	private float _delay;

	private bool _firstCollision;

	private bool _secondCollision;

	private float _size;

	private int _obstacleContact;
}
