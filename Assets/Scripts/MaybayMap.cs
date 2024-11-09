using System;
using System.Collections;
using Com.LuisPedroFonseca.ProCamera2D;
using MyDataLoader;
using UnityEngine;

public class MaybayMap : BaseEnemy
{
	public override void Hit(float damage)
	{
		base.Hit(damage);
		this.timePingPongColor = 0f;
		if (this.HP <= 0f && this.State != ECharactor.DIE)
		{
			base.CheckMission();
			base.StartCoroutine(this.StartEffect());
			LeanTween.rotateZ(this.objMaybay1, 15f, 0.5f).setOnComplete(delegate()
			{
				this.objMaybay1.SetActive(false);
				this.objMaybay2.SetActive(true);
				this.polygon.enabled = false;
			});
			this.isInit = false;
			try
			{
				GameManager.Instance.ListEnemy.Remove(this);
			}
			catch
			{
			}
			this.State = ECharactor.DIE;
		}
	}

	private void Start()
	{
		BaseTrigger baseTrigger = this.zoomCameraPro;
		baseTrigger.OnEnteredTrigger = (Action)Delegate.Combine(baseTrigger.OnEnteredTrigger, new Action(delegate()
		{
			this.Init();
		}));
	}

	private void LateUpdate()
	{
		if (!this.isInit || GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
		this.sprite.color = base.PingPongColor();
	}

	public void Init()
	{
		if (this.State == ECharactor.DIE)
		{
			return;
		}
		if (!GameManager.Instance.ListEnemy.Contains(this))
		{
			GameManager.Instance.ListEnemy.Add(this);
		}
		this.timeStartEffectDelay = new WaitForSeconds(0.5f);
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
		base.isInCamera = true;
		this.ID = 200002;
		this.rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
	}

	private IEnumerator StartEffect()
	{
		for (int i = 0; i < 5; i++)
		{
			GameManager.Instance.fxManager.ShowEffect(4, this.tfTarget[UnityEngine.Random.Range(0, this.tfTarget.Count)].position, 2f * Vector3.one, true, true);
			yield return this.timeStartEffectDelay;
		}
		yield break;
	}

	[SerializeField]
	private float HP_Current = 200f;

	[SerializeField]
	private BaseTrigger zoomCameraPro;

	[SerializeField]
	private GameObject objMaybay1;

	[SerializeField]
	private GameObject objMaybay2;

	[SerializeField]
	private PolygonCollider2D polygon;

	private WaitForSeconds timeStartEffectDelay;

	[SerializeField]
	private SpriteRenderer sprite;
}
