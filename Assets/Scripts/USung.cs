using System;
using System.Collections;
using MyDataLoader;
using Spine;
using Spine.Unity;
using UnityEngine;

public class USung : BaseEnemy
{
	public override void Hit(float damage)
	{
		base.Hit(damage);
		this.skeletonAnimation.state.SetAnimation(1, this.hit, false);
		if (this.HP <= 0f && this.State != ECharactor.DIE)
		{
			base.CheckMission();
			GameManager.Instance.fxManager.ShowEffect(6, this.transform.position, Vector3.one, true, true);
			this.skeletonAnimation.state.ClearTracks();
			this.skeletonAnimation.state.SetAnimation(0, this.dead, false);
			if (this.isGift)
			{
				GameManager.Instance.giftManager.Create(this.transform.position, 0);
			}
			this.State = ECharactor.DIE;
			base.gameObject.SetActive(false);
		}
	}

	public void TryInit()
	{
		this.Init();
		SpriteVisible componentInChildren = base.GetComponentInChildren<SpriteVisible>();
		if (componentInChildren != null)
		{
			SpriteVisible spriteVisible = componentInChildren;
			spriteVisible.OnInvisible = (Action)Delegate.Combine(spriteVisible.OnInvisible, new Action(delegate()
			{
				try
				{
					if (this.isInit)
					{
						GameManager.Instance.ListEnemy.Remove(this);
					}
				}
				catch
				{
				}
			}));
			SpriteVisible spriteVisible2 = componentInChildren;
			spriteVisible2.OnVisible = (Action)Delegate.Combine(spriteVisible2.OnVisible, new Action(delegate()
			{
				try
				{
					if (this.isInit)
					{
						GameManager.Instance.ListEnemy.Add(this);
					}
				}
				catch
				{
				}
			}));
		}
		else
		{
			try
			{
				GameManager.Instance.ListEnemy.Add(this);
			}
			catch
			{
			}
		}
	}

	private IEnumerator Start()
	{
		yield return new WaitUntil(() => GameManager.Instance.StateManager.EState == EGamePlay.RUNNING);
		this.TryInit();
		yield break;
	}

	private void LateUpdate()
	{
		if (!this.isInit || GameManager.Instance.StateManager.EState != EGamePlay.RUNNING || !CameraController.Instance.IsInView(GameManager.Instance.player.transform))
		{
			return;
		}
		if (GameManager.Instance.player.IsInVisible())
		{
			return;
		}
		if (this.IsRotate)
		{
			Vector2 vector = GameManager.Instance.player.GetPosition() - base.Origin();
			vector.Normalize();
			float num = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			this.transform.rotation = Quaternion.Euler(0f, 0f, num + 90f);
		}
		this.timeAttack += Time.deltaTime;
		if (this.timeAttack >= this.TimeAttack && base.isInCamera)
		{
			this.skeletonAnimation.state.SetAnimation(0, this.attack, false);
			this.timeAttack = 0f;
		}
	}

	public void Init()
	{
		this.meshRenderer.enabled = true;
		Enemy[] array = new Enemy[]
		{
			new Enemy()
		};
		this.HP = (array[0].HP = this.HP_Current);
		this.InitEnemy(new EnemyCharactor
		{
			enemy = array
		}, 0);
		this.isInit = true;
		GameMode.Mode emode = GameMode.Instance.EMode;
		if (emode != GameMode.Mode.NORMAL)
		{
			if (emode != GameMode.Mode.HARD)
			{
				if (emode == GameMode.Mode.SUPER_HARD)
				{
					this.TimeAttack = 0.5f;
					this.DAMAGED *= 4f;
				}
			}
			else
			{
				this.TimeAttack = 0.8f;
				this.DAMAGED *= 2f;
			}
		}
		else
		{
			this.TimeAttack = 1f;
		}
		this.ID = 100000;
		this.skeletonAnimation.state.Event += this.HandleEvent;
		this.rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
	}

	private void HandleEvent(TrackEntry entry, Spine.Event e)
	{
		if (entry == null)
		{
			return;
		}
		if (e.Data.Name.Equals("shoot"))
		{
			Vector2 vector = base.Origin();
			Vector2 direction = GameManager.Instance.player.Origin() - base.Origin();
			if (!this.IsRotate)
			{
				direction = (Vector2)this.tfBullet.position - base.Origin();
			}
			direction.Normalize();
			BulletEnemy bulletEnemy = GameManager.Instance.bulletManager.CreateBulletEnemy(5, direction, this.tfBullet.position, this.DAMAGED, 7f, 0f);
			bulletEnemy.transform.localScale = new Vector3(2f, 2f, 2f);
			bulletEnemy.spriteRenderer.flipX = false;
			bulletEnemy.SetSkipGround();
		}
	}

	private void OnDisable()
	{
		try
		{
			GameManager.Instance.ListEnemy.Remove(this);
		}
		catch
		{
		}
		this.isInit = false;
		this.circle.enabled = false;
		base.StopAllCoroutines();
	}

	[SpineAnimation("", "", true, false)]
	public string dead;

	[SpineAnimation("", "", true, false)]
	public string attack;

	[SpineAnimation("", "", true, false)]
	public string hit;

	[SerializeField]
	private CircleCollider2D circle;

	private float timeAttack;

	[SerializeField]
	private Transform tfBullet;

	[SerializeField]
	private float HP_Current = 200f;

	[SerializeField]
	private float DAMAGED = 10f;

	[SerializeField]
	private bool isGift = true;

	private float TimeAttack = 1f;

	[SerializeField]
	private bool IsRotate = true;

	public SkeletonAnimation skeletonAnimation;
}
