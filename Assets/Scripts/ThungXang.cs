using System;
using MyDataLoader;
using UnityEngine;

public class ThungXang : BaseEnemy
{
	public override void Hit(float damage)
	{
		base.Hit(damage);
		this.timePingPongColor = 0f;
		if (this.HP <= 0f && this.State != ECharactor.DIE)
		{
			this.thungxangGroup.RemoveObject(this);
			base.CheckMission();
			this.State = ECharactor.DIE;
			GameManager.Instance.fxManager.ShowEffect(6, this.transform.position, Vector3.one, true, true);
			try
			{
				GameManager.Instance.ListEnemy.Remove(this);
			}
			catch
			{
			}
			this.isInit = false;
			if (this.isGiftHP)
			{
				GameManager.Instance.giftManager.Create(this.transform.position, 0);
			}
			UnityEngine.Object.DestroyObject(base.gameObject);
		}
	}

	private void Start()
	{
		SpriteVisible spriteVisible = this.spriteVisible;
		spriteVisible.OnVisible = (Action)Delegate.Combine(spriteVisible.OnVisible, new Action(delegate()
		{
			if (GameManager.Instance.StateManager.EState == EGamePlay.RUNNING && !this.isInit)
			{
				GameManager.Instance.ListEnemy.Add(this);
				this.isInit = true;
				this.Init();
			}
		}));
		SpriteVisible spriteVisible2 = this.spriteVisible;
		spriteVisible2.OnInvisible = (Action)Delegate.Combine(spriteVisible2.OnInvisible, new Action(delegate()
		{
			try
			{
				bool flag = false;
				CameraController.Orientation orientaltion = CameraController.Instance.orientaltion;
				if (orientaltion != CameraController.Orientation.HORIZONTAL)
				{
					if (orientaltion == CameraController.Orientation.VERTICAL)
					{
						flag = (this.transform.position.y < CameraController.Instance.Position.y - CameraController.Instance.Size().y + 1f);
					}
				}
				else
				{
					flag = (this.transform.position.x <= CameraController.Instance.Position.x - CameraController.Instance.Size().x + 1f);
				}
				if (this.isInit && flag)
				{
					GameManager.Instance.ListEnemy.Remove(this);
					UnityEngine.Object.DestroyObject(base.gameObject);
				}
			}
			catch
			{
			}
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

	private void Init()
	{
		if (this.State == ECharactor.DIE)
		{
			return;
		}
		Enemy[] array = new Enemy[]
		{
			new Enemy()
		};
		this.HP = (array[0].HP = this.HP_Current);
		this.InitEnemy(new EnemyCharactor
		{
			enemy = array
		}, 0);
		this.ID = 200003;
		this.rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
	}

	private void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.CompareTag("Ground") || coll.gameObject.CompareTag("Tank"))
		{
			this.rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
			this.isMain = coll.gameObject.CompareTag("Ground");
		}
	}

	[SerializeField]
	private float HP_Current = 200f;

	[SerializeField]
	private SpriteVisible spriteVisible;

	[SerializeField]
	private bool isGiftHP;

	[SerializeField]
	private SpriteRenderer sprite;

	public bool isMain;

	public ThungXangGroup thungxangGroup;
}
