using System;
using System.Collections;
using MyDataLoader;
using UnityEngine;

public class USungMay : BaseEnemy
{
	private IEnumerator Start()
	{
		yield return new WaitUntil(() => GameManager.Instance.StateManager.EState == EGamePlay.RUNNING);
		this.OnInitSung();
		yield break;
	}

	private void Update()
	{
		if (!base.isInCamera || !this.isInit || GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			if (!base.isInCamera && this.isInit && GameManager.Instance.player.transform.position.x > this.transform.position.x)
			{
				if (this._coolDownHide > 0f)
				{
					this._coolDownHide -= Time.fixedDeltaTime;
				}
				else
				{
					this.Die();
					base.gameObject.SetActive(false);
				}
			}
			return;
		}
		if (!this._isAddEnemy)
		{
			this._isAddEnemy = true;
			GameManager.Instance.ListEnemy.Add(this);
		}
		this.ramboPos = GameManager.Instance.player.transform.position;
		this._flipSung = (this.ramboPos.x < this.transformSung.position.x);
		if (this.spriteRendererSung.flipX != this._flipSung)
		{
			this.spriteRendererSung.flipX = this._flipSung;
		}
		this._rotate = Quaternion.LookRotation(this.ramboPos - this.transformSung.position, Vector3.back);
		this._rotate.x = (this._rotate.y = 0f);
		this._eulerRotate = this._rotate.eulerAngles;
		this._eulerRotate.z = this._eulerRotate.z + (float)((!this.spriteRendererSung.flipX) ? 90 : -90);
		this._eulerSung.z = Mathf.SmoothDampAngle(this._eulerSung.z, this._eulerRotate.z, ref this._velocity, 0.5f);
		this.transformSung.eulerAngles = this._eulerSung;
		if (this._reloadTime > 0f)
		{
			this._reloadTime -= Time.deltaTime;
		}
		else
		{
			this.animatorSung.Play("Fire");
			this._reloadTime = this.reloadTime;
		}
	}

	private void OnInitSung()
	{
		this.cacheEnemy = new Enemy();
		this.cacheEnemy.HP = (this.HP = this.totalHP * GameMode.Instance.GetMode());
		this.damage *= GameMode.Instance.GetMode();
		this._flipSung = (GameManager.Instance.player.transform.position.x < this.transformSung.position.x);
		this.spriteRendererSung.flipX = this._flipSung;
		this._eulerSung = this.transformSung.eulerAngles;
		this._reloadTime = this.reloadTime;
		this.isInit = true;
	}

	private void OnFire()
	{
		SingletonGame<AudioController>.Instance.PlaySound(this.audioFire, 1f);
		Vector3 vector = this.transformSung.position + this.transformSung.right * ((!this._flipSung) ? 2.6f : -2.6f);
		GameManager.Instance.bulletManager.CreateBulletEnemy(12, vector - this.transformSung.position, vector, this.damage, this.speedBullet, 0f).spriteRenderer.flipX = false;
	}

	public override void Hit(float damage)
	{
		if (this.HP <= 0f)
		{
			this.Die();
			base.StartCoroutine(this.SungDie());
		}
	}

	private void Die()
	{
		this.isInit = false;
		this.bodyCollider2D.enabled = false;
		this.transformSung.gameObject.SetActive(false);
		if (this.lineBloodEnemy)
		{
			this.lineBloodEnemy.Hide();
		}
		GameManager.Instance.ListEnemy.Remove(this);
	}

	private IEnumerator SungDie()
	{
		this.objEff.SetActive(true);
		yield return new WaitForSeconds(1.35f);
		this.objEff.SetActive(false);
		yield break;
	}

	[SerializeField]
	private float totalHP;

	[SerializeField]
	private float damage;

	[SerializeField]
	private float speedBullet;

	[SerializeField]
	private Animator animatorSung;

	[SerializeField]
	private SpriteRenderer spriteRendererSung;

	[SerializeField]
	private Transform transformSung;

	[SerializeField]
	private GameObject objEff;

	[SerializeField]
	private float reloadTime;

	[SerializeField]
	private AudioClip audioFire;

	private Quaternion _rotate;

	private Vector3 _eulerRotate;

	private Vector3 _eulerSung;

	private Vector3 ramboPos;

	private bool _flipSung;

	private float _reloadTime;

	private float _velocity;

	private bool _isAddEnemy;

	private float _coolDownHide = 5f;
}
