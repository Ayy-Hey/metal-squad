using System;
using System.Collections;
using MyDataLoader;
using Spine;
using Spine.Unity;
using UnityEngine;

public class MiniBoss5_2 : BaseEnemySpine
{
	public void InitEnemy(bool isMiniBoss = true, Action<MiniBoss5_2> callback = null)
	{
		this.DeathAction = callback;
		this._isMiniBoss = isMiniBoss;
		if (this.cacheEnemyData != null)
		{
			this.ID = this.cacheEnemyData.type;
		}
		this.cacheEnemy = new Enemy();
		this.cacheEnemy.HP = this.data.datas[this.cacheEnemyData.level].hp;
		this.HP = this.data.datas[this.cacheEnemyData.level].hp * GameMode.Instance.GetMode();
		this.cacheEnemy.Damage = this.data.datas[this.cacheEnemyData.level].damage * GameMode.Instance.GetMode();
		this.cacheEnemy.Speed = this.data.datas[this.cacheEnemyData.level].speed;
		base.gameObject.SetActive(true);
		if (this.lineBloodEnemy)
		{
			this.lineBloodEnemy.Reset();
		}
		this.skeletonAnimation.skeleton.FlipX = false;
		this.skeletonAnimation.state.SetEmptyAnimations(0f);
		this.skeletonAnimation.state.SetAnimation(1, this.idle, true);
		if (this._GunTip1 == null)
		{
			this._GunTip1 = this.skeletonAnimation.skeleton.FindBone("Gun_tip01");
		}
		if (this._GunTip2 == null)
		{
			this._GunTip2 = this.skeletonAnimation.skeleton.FindBone("Gun_tip02");
		}
		this.meshRenderer.sortingOrder = ((!this._isMiniBoss) ? 7 : 4);
		this.transform.position = this.cacheEnemyData.Vt2;
		if (!this._isMiniBoss)
		{
			this.targetMove = this.cacheEnemyData.Vt2;
			float num = UnityEngine.Random.Range(1f, 3f);
			this.targetMove.x = ((this.targetMove.x <= CameraController.Instance.camPos.x) ? (CameraController.Instance.camPos.x - num) : (CameraController.Instance.camPos.x + num));
			this.targetMove.y = Mathf.Clamp(this.targetMove.y, CameraController.Instance.camPos.y - 1f, CameraController.Instance.camPos.y + 3f);
		}
		this.State = ECharactor.RUN;
		this._firstRun = !this._isMiniBoss;
		this._changeState = false;
		this.isInit = true;
	}

	public override void UpdateObject()
	{
		if (!this.isInit)
		{
			return;
		}
		base.UpdateObject();
		if (!this._changeState)
		{
			ECharactor state = this.State;
			if (state != ECharactor.RUN)
			{
				if (state != ECharactor.ATTACK)
				{
					if (state == ECharactor.IDLE)
					{
						base.StartCoroutine(this.OnIdle());
					}
				}
				else
				{
					base.StartCoroutine(this.OnAttack());
				}
			}
			else
			{
				this.skeletonAnimation.state.SetAnimation(1, this.idle, true);
			}
			this._changeState = true;
		}
		else if (this.State == ECharactor.RUN)
		{
			this.transform.position = Vector3.SmoothDamp(this.transform.position, this.targetMove, ref this._velocity, 0.5f);
			bool flag = Vector3.Distance(this.transform.position, this.targetMove) < 0.1f;
			if (flag)
			{
				this.State = ECharactor.IDLE;
				this._changeState = false;
				if (!this._firstRun)
				{
					this._firstRun = true;
					this.meshRenderer.sortingOrder = 7;
				}
				return;
			}
		}
	}

	private IEnumerator OnAttack()
	{
		for (int i = 0; i < 2; i++)
		{
			this.PlaySound(1);
			this.skeletonAnimation.skeleton.FlipX = (this.transform.position.x < GameManager.Instance.player.transform.position.x);
			this.skeletonAnimation.state.SetAnimation(1, this.attacks[i], false);
			yield return new WaitForSeconds(0.15f);
			if (i % 2 == 0)
			{
				this._bulletPos = this._GunTip2.GetWorldPosition(this.transform);
			}
			else
			{
				this._bulletPos = this._GunTip1.GetWorldPosition(this.transform);
			}
			this._bulletDirec = GameManager.Instance.player.tfOrigin.position - this._bulletPos;
			float speed = Mathf.Min(this.cacheEnemy.Speed * 2f, 10f);
			GameManager.Instance.bulletManager.CreateBulletFlash(this.cacheEnemy.Damage, speed, this._bulletPos, this._bulletDirec);
			yield return new WaitForSeconds(0.18f);
		}
		this.MakeNewTarget();
		this.State = ECharactor.RUN;
		this._changeState = false;
		yield break;
	}

	private IEnumerator OnIdle()
	{
		this.skeletonAnimation.state.SetAnimation(1, this.idle, true);
		yield return new WaitForSeconds(this.idleTime);
		this.State = ECharactor.ATTACK;
		this._changeState = false;
		yield break;
	}

	private void MakeNewTarget()
	{
		this.targetMove.x = UnityEngine.Random.Range(this.transform.position.x - 2f, this.transform.position.x + 2f);
		this.targetMove.y = UnityEngine.Random.Range(this.transform.position.y - 2f, this.transform.position.y + 2f);
		this.targetMove.x = Mathf.Clamp(this.targetMove.x, CameraController.Instance.camPos.x - 5f, CameraController.Instance.camPos.x + 5f);
		this.targetMove.y = Mathf.Clamp(this.targetMove.y, CameraController.Instance.camPos.y - 1f, CameraController.Instance.camPos.y + 3f);
	}

	private void PlaySound(int id)
	{
		if (ProfileManager.settingProfile.IsSound)
		{
			this.audioSources[id].Play();
		}
	}

	public override void Hit(float damage)
	{
		if (this.State == ECharactor.DIE || GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
		base.Hit(damage);
		this.Hit();
		if (this.HP <= 0f && this.State != ECharactor.DIE)
		{
			GameManager.Instance.fxManager.ShowEffect(6, this.transform.position, Vector3.one, true, true);
			this.State = ECharactor.DIE;
			base.StartCoroutine(this.Die());
			if (!this._isMiniBoss)
			{
				base.CalculatorToDie(true, false);
			}
		}
	}

	private void Hit()
	{
		this.skeletonAnimation.state.SetAnimation(2, this.hit, false);
	}

	private IEnumerator Die()
	{
		this.PlaySound(2);
		this.skeletonAnimation.state.SetAnimation(1, this.death, false);
		yield return new WaitForSeconds(0.8f);
		base.gameObject.SetActive(false);
		yield break;
	}

	private void OnDisable()
	{
		try
		{
			this.isInit = false;
			GameManager.Instance.ListEnemy.Remove(this);
			if (this.OnEnemyDeaded != null)
			{
				this.OnEnemyDeaded();
				this.OnEnemyDeaded = null;
			}
			this.DeathAction(this);
		}
		catch
		{
		}
	}

	[SerializeField]
	public DataEVL data;

	[SpineAnimation("", "", true, false)]
	public string[] attacks;

	[SpineAnimation("", "", true, false)]
	public string idle;

	[SpineAnimation("", "", true, false)]
	public string hit;

	[SpineAnimation("", "", true, false)]
	public string death;

	public Action<MiniBoss5_2> DeathAction;

	public float speed;

	public Vector3 targetMove;

	public float idleTime;

	public AudioSource[] audioSources;

	public float TOTAL_HP;

	private bool _firstRun;

	private bool _changeState;

	private Bone _GunTip1;

	private Bone _GunTip2;

	private Vector3 _bulletPos;

	private Vector3 _bulletDirec;

	private Vector3 _velocity;

	private bool _isMiniBoss;
}
