using System;
using MyDataLoader;
using UnityEngine;

public class SungTreoTuong : BaseEnemy
{
	private void OnValidate()
	{
		if (!this.data)
		{
			this.data = Resources.Load<DataEVL>("Charactor/Enemies/" + base.GetType().ToString());
		}
	}

	private void OnDisable()
	{
		try
		{
			this.isInit = false;
			GameManager.Instance.ListEnemy.Remove(this);
		}
		catch
		{
			UnityEngine.Debug.Log("don't remove this");
		}
	}

	private void Update()
	{
		if (GameManager.Instance.StateManager.EState == EGamePlay.RUNNING)
		{
			if (!this.isInit)
			{
				this.Init();
			}
			else
			{
				this.OnUpdate(Time.deltaTime);
			}
		}
	}

	public void Init()
	{
		if (!this.data)
		{
			this.OnValidate();
		}
		this.cacheEnemy = new Enemy();
		this.cacheEnemy.HP = (this.HP = this.data.datas[this.level].hp);
		this._canAttack = (this.data.datas[this.level].maxVision <= 0f);
		this._coolDown = ((!this._canAttack) ? 0f : this.data.datas[this.level].timeReload);
		GameManager.Instance.ListEnemy.Add(this);
		this.isInit = true;
		this.rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
	}

	public void OnUpdate(float deltaTime)
	{
		if (!base.isInCamera)
		{
			return;
		}
		this.LookAtRambo();
		if (this.transform.position.x < CameraController.Instance.LeftCamera() || this.transform.position.y < CameraController.Instance.BottomCamera())
		{
			base.gameObject.SetActive(false);
			return;
		}
		if (!this._canAttack)
		{
			this._canAttack = (this.transform.position.x - GameManager.Instance.player.transform.position.x <= this.data.datas[this.level].maxVision);
			return;
		}
		if (this._coolDown > 0f)
		{
			this._coolDown -= deltaTime;
		}
		if (this._coolDown <= 0f)
		{
			this.ResetCoolDown();
			this.Attack();
		}
	}

	private void LookAtRambo()
	{
		this._rotate = Quaternion.LookRotation(GameManager.Instance.player.tfOrigin.position - this.transform.position, Vector3.back);
		this._rotate.x = (this._rotate.y = 0f);
		this.transform.rotation = this._rotate;
	}

	private void Attack()
	{
		Vector3 v = GameManager.Instance.player.tfOrigin.position - this.firePoint.position;
		GameManager.Instance.bulletManager.CreateBulletEnemy(this.bulletType, v, this.firePoint.position, this.data.datas[this.level].damage * GameMode.Instance.GetMode(), this.data.datas[this.level].speed, 0f).spriteRenderer.flipX = false;
	}

	private void ResetCoolDown()
	{
		this._coolDown = this.data.datas[this.level].timeReload;
	}

	public override void Hit(float damage)
	{
		base.Hit(damage);
		if (this.HP <= 0f)
		{
			GameManager.Instance.fxManager.ShowEffect(5, this.transform.position, Vector3.one, true, true);
			base.gameObject.SetActive(false);
		}
	}

	[SerializeField]
	private DataEVL data;

	[SerializeField]
	private int level;

	[SerializeField]
	private int bulletType;

	[SerializeField]
	private Transform firePoint;

	private float _coolDown;

	private bool _canAttack;

	private Quaternion _rotate;

	private Vector3 _euler;
}
