using System;
using MyDataLoader;
using UnityEngine;

public class BongDinh : BaseEnemy
{
	public void InitObject(float hp, float damage, float speed, Vector3 pos, Vector3 posTarget, Action<BongDinh> hide)
	{
		if (this.cacheEnemy == null)
		{
			this.InitEnemy(new EnemyCharactor
			{
				enemy = new Enemy[]
				{
					new Enemy()
				}
			}, 0);
		}
		Enemy cacheEnemy = this.cacheEnemy;
		this.HP = hp;
		cacheEnemy.HP = hp;
		this.damage = damage;
		this.speed = speed;
		this.target = posTarget;
		this.actionHide = hide;
		this.time = 0f;
		this.isDown = false;
		this.idCurve = UnityEngine.Random.Range(0, 5);
		base.gameObject.SetActive(true);
		this.transform.position = pos;
		GameManager.Instance.ListEnemy.Add(this);
		this.isInit = true;
	}

	public void UpdateObject(float deltaTime)
	{
		this.pos = this.transform.position;
		if (this.isDown)
		{
			this.pos.y = this.pos.y - this.speed * deltaTime;
			this.pos.x = this.oldX + this.curves[this.idCurve].Evaluate(this.time);
			this.time += deltaTime;
		}
		else if (this.pos != this.target)
		{
			this.pos = Vector3.MoveTowards(this.pos, this.target, this.speed * deltaTime);
		}
		else
		{
			this.oldX = this.pos.x;
			this.isDown = true;
			this.speed *= 0.8f;
		}
		if (this.speed > 1f)
		{
			this.speed -= deltaTime / 3f;
		}
		this.transform.position = this.pos;
	}

	public override void Hit(float damage)
	{
		if (this.HP <= 0f)
		{
			this.InActive();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Rambo"))
		{
			try
			{
				ISkill component = collision.GetComponent<ISkill>();
				if (component != null && component.IsInVisible())
				{
					return;
				}
				collision.GetComponent<IHealth>().AddHealthPoint(-this.damage, EWeapon.NONE);
				collision.GetComponent<BaseCharactor>().DownSpeed();
			}
			catch
			{
				UnityEngine.Debug.Log("loi roi");
			}
			this.InActive();
		}
		if (collision.CompareTag("Ground"))
		{
			this.InActive();
		}
	}

	private void InActive()
	{
		try
		{
			if (ProfileManager.settingProfile.IsSound && this.audioS)
			{
				this.audioS.Play();
			}
			this.isInit = false;
			this.isDown = false;
			this.actionHide(this);
			base.gameObject.SetActive(false);
			GameManager.Instance.ListEnemy.Remove(this);
			GameManager.Instance.fxManager.ShowEffect(5, this.transform.position, Vector3.one, true, true);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(ex.Message);
		}
	}

	[SerializeField]
	private AnimationCurve[] curves;

	[SerializeField]
	private AudioSource audioS;

	private Action<BongDinh> actionHide;

	private float damage;

	private float speed;

	private Vector3 target;

	private Vector3 pos;

	private float oldX;

	private bool isDown;

	private float time;

	private int idCurve;
}
