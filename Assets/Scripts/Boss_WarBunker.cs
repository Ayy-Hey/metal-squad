using System;
using System.Collections;
using System.Collections.Generic;
using MyDataLoader;
using Newtonsoft.Json;
using Spine;
using Spine.Unity;
using UnityEngine;

public class Boss_WarBunker : BaseBoss
{
	private void Update()
	{
		if (GameManager.Instance.StateManager.EState == EGamePlay.RUNNING)
		{
			if (this._isPause)
			{
				this.Pause(false);
			}
			this.OnUpdate(Time.deltaTime);
			return;
		}
		if (this._state == Boss_WarBunker.EState.Die)
		{
			return;
		}
		if (!this._isPause)
		{
			this.Pause(true);
		}
	}

	public override void Init()
	{
		base.Init();
		float num = GameMode.Instance.GetMode();
		string text = (!SplashScreen._isLoadResourceDecrypt) ? this.Data_Encrypt.text : this.Data_Decrypt.text;
		string text2 = ProfileManager.DataEncryption.decrypt2(text);
		EnemyCharactor enemyCharactor = JsonConvert.DeserializeObject<EnemyCharactor>((!SplashScreen._isLoadResourceDecrypt) ? text2 : text);
		if (GameMode.Instance.Style == GameMode.GameStyle.MultiPlayer && GameMode.Instance.modePlay == GameMode.ModePlay.PvpMode)
		{
			num = 5f;
		}
		this.cacheEnemy = enemyCharactor.enemy[0];
		this.cacheEnemy.HP = (this.HP = this.cacheEnemy.HP * num);
		this.cacheEnemy.Damage *= num;
		this._level = (int)GameMode.Instance.EMode;
		GameManager.Instance.ListEnemy.Add(this);
		this.objWarningLaser.SetActive(false);
		this.laser.Tilling = new Vector2(5f, 1f);
		this.listBi[0].gameObject.transform.parent.parent = null;
		this.poolBi = new ObjectPooling<BulletBi>(this.listBi.Count, null, null);
		for (int i = 0; i < this.listBi.Count; i++)
		{
			this.poolBi.Store(this.listBi[i]);
		}
		this.sauHp = Mathf.Round(this.HP / 12f);
		this.listSau[0].gameObject.transform.parent.parent = null;
		this.poolSau = new ObjectPooling<EnemySau>(this.listSau.Count, null, null);
		for (int j = 0; j < this.listSau.Count; j++)
		{
			this.poolSau.Store(this.listSau[j]);
		}
		this.skeletonAnimation.state.Event += this.OnEvent;
		this.skeletonAnimation.state.Complete += this.OnComplete;
		this.bodyCollider2D.enabled = false;
		base.isInCamera = false;
		this._state = Boss_WarBunker.EState.Open;
		this._changeState = false;
		this.isInit = true;
	}

	public void OnUpdate(float deltaTime)
	{
		if (!this.isInit)
		{
			return;
		}
		if (!this._changeState)
		{
			this.StartState();
		}
		else
		{
			this.UpdateState(deltaTime);
		}
		for (int i = 0; i < this.listSau.Count; i++)
		{
			if (this.listSau[i] && this.listSau[i].isInit)
			{
				this.listSau[i].OnUpdate(deltaTime);
			}
		}
		for (int j = 0; j < this.listBi.Count; j++)
		{
			if (this.listBi[j] && this.listBi[j].isInit)
			{
				this.listBi[j].OnUpdate(deltaTime);
			}
		}
	}

	private void Pause(bool pause)
	{
		this._isPause = pause;
		this.skeletonAnimation.timeScale = (float)((!pause) ? 1 : 0);
		for (int i = 0; i < this.listSau.Count; i++)
		{
			if (this.listSau[i] && this.listSau[i].isInit)
			{
				this.listSau[i].Pause(pause);
			}
		}
	}

	private void StartState()
	{
		this._changeState = true;
		bool loop = false;
		switch (this._state)
		{
		case Boss_WarBunker.EState.Attack1:
			this._gunId = 0;
			this.skeletonAnimation.timeScale = ((!this._crazy) ? 1f : 1.3f);
			break;
		case Boss_WarBunker.EState.Attack2_1:
			this.objWarningLaser.SetActive(true);
			break;
		case Boss_WarBunker.EState.Attack2_2:
			loop = true;
			this.laser.ActiveEff();
			this.laser.SetTexture(0);
			this.laser.SetColor(Color.white);
			this._laserSize = 0.2f;
			this.laser.SetSize(this._laserSize);
			this._endPointLaser.y = this.transLaserPoint.position.y;
			this._endPointLaser.x = CameraController.Instance.LeftCamera();
			break;
		}
		this.PlayAnim(loop);
	}

	private void UpdateState(float deltaTime)
	{
		switch (this._state)
		{
		case Boss_WarBunker.EState.Attack2_2:
			this._hit = Physics2D.Raycast(this.transLaserPoint.position, Vector2.left, 100f, this.ramboMask);
			if (this._hit.collider && this._hit.collider.CompareTag("Rambo"))
			{
				try
				{
					ISkill component = this._hit.collider.GetComponent<ISkill>();
					if (component == null || !component.IsInVisible())
					{
						this._hit.collider.GetComponent<IHealth>().AddHealthPoint(-this.GetDamage(), EWeapon.NONE);
					}
				}
				catch
				{
				}
			}
			this.laser.OnShow(deltaTime, this.transLaserPoint.position, this._endPointLaser);
			if (this._laserSize < 1f)
			{
				this._laserSize = Mathf.SmoothDamp(this._laserSize, 1.01f, ref this._veloSize, 0.3f);
				this.laser.SetSize(this._laserSize);
			}
			break;
		case Boss_WarBunker.EState.Attack2_3:
			this._laserSize = Mathf.SmoothDamp(this._laserSize, 0.1f, ref this._veloSize, 0.3f);
			this.laser.SetSize(this._laserSize);
			this.laser.OnShow(deltaTime, this.transLaserPoint.position, this._endPointLaser);
			break;
		}
	}

	private void ChangeState()
	{
		if (!this._crazy)
		{
			this._crazy = (this.HP < this.cacheEnemy.HP * 3f / 5f);
		}
		switch (this._state)
		{
		case Boss_WarBunker.EState.Attack1:
			this._oldAttack = this._state;
			this.skeletonAnimation.timeScale = 1f;
			this._state = (this._crazy ? Boss_WarBunker.EState.Attack3_1 : Boss_WarBunker.EState.Idle);
			break;
		case Boss_WarBunker.EState.Attack2_1:
			this.objWarningLaser.SetActive(false);
			this._state = Boss_WarBunker.EState.Attack2_2;
			break;
		case Boss_WarBunker.EState.Attack2_2:
			this._state = Boss_WarBunker.EState.Attack2_3;
			break;
		case Boss_WarBunker.EState.Attack2_3:
			this.laser.DisableEff();
			this.laser.Off();
			this._oldAttack = this._state;
			this._state = (this._crazy ? Boss_WarBunker.EState.Attack3_1 : Boss_WarBunker.EState.Idle);
			break;
		case Boss_WarBunker.EState.Attack3_1:
			this._state = Boss_WarBunker.EState.Attack3_2;
			break;
		case Boss_WarBunker.EState.Attack3_2:
			this._state = Boss_WarBunker.EState.Attack3_3;
			break;
		case Boss_WarBunker.EState.Attack3_3:
			if (!this._crazy)
			{
				this._oldAttack = this._state;
				this._state = Boss_WarBunker.EState.Idle;
			}
			else
			{
				this._state = ((this._oldAttack != Boss_WarBunker.EState.Attack1) ? Boss_WarBunker.EState.Attack1 : Boss_WarBunker.EState.Attack2_1);
			}
			break;
		case Boss_WarBunker.EState.Idle:
			switch (this._oldAttack)
			{
			case Boss_WarBunker.EState.Attack1:
				this._state = Boss_WarBunker.EState.Attack2_1;
				break;
			case Boss_WarBunker.EState.Attack2_1:
			case Boss_WarBunker.EState.Attack2_2:
			case Boss_WarBunker.EState.Attack2_3:
				this._state = Boss_WarBunker.EState.Attack3_1;
				break;
			case Boss_WarBunker.EState.Attack3_1:
			case Boss_WarBunker.EState.Attack3_2:
			case Boss_WarBunker.EState.Attack3_3:
				this._state = Boss_WarBunker.EState.Attack1;
				break;
			default:
				this._state = Boss_WarBunker.EState.Attack1;
				break;
			}
			break;
		case Boss_WarBunker.EState.Open:
			this._state = Boss_WarBunker.EState.Attack1;
			this._oldAttack = this._state;
			break;
		case Boss_WarBunker.EState.Close:
			this._state = Boss_WarBunker.EState.Open;
			break;
		}
		this._changeState = false;
	}

	private void PlayAnim(bool loop)
	{
		this.skeletonAnimation.state.SetAnimation(0, this.anims[(int)this._state], loop);
	}

	private void CreateBullet()
	{
		if (UnityEngine.Random.Range(0, 2) == 0)
		{
			RocketEnemy rocketEnemy = GameManager.Instance.bulletManager.CreateRocketEnemy(this.transGuns[this._gunId].position);
			rocketEnemy.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
			Vector2 v = default(Vector2);
			v = GameManager.Instance.player.transform.position;
			rocketEnemy.SetFire(1, v, this.GetDamage(), 90);
		}
		else
		{
			BulletBi bulletBi = this.poolBi.New();
			if (!bulletBi)
			{
				bulletBi = UnityEngine.Object.Instantiate<BulletBi>(this.listBi[0]);
				this.listBi.Add(bulletBi);
				bulletBi.gameObject.transform.parent = this.listBi[0].gameObject.transform.parent;
			}
			float maxVeloY = UnityEngine.Random.Range(5f, 7f);
			float startVeloY = UnityEngine.Random.Range(0f, 1f);
			bulletBi.Init(this.GetDamage(), this.GetSpeed(), startVeloY, maxVeloY, true, this.transGuns[this._gunId].position, new Action<BulletBi>(this.HideBi));
		}
	}

	private void HideBi(BulletBi bi)
	{
		this.poolBi.Store(bi);
	}

	private void CreateSau()
	{
		EnemySau enemySau = this.poolSau.New();
		if (!enemySau)
		{
			enemySau = UnityEngine.Object.Instantiate<EnemySau>(this.listSau[0]);
			this.listSau.Add(enemySau);
			enemySau.gameObject.transform.parent = this.listSau[0].gameObject.transform.parent;
		}
		enemySau.Init(this.sauHp, this.GetSpeed(), this.GetDamage(), this.transSauCreatePoin.position, new Action<EnemySau>(this.SauHide), true);
	}

	private void SauHide(EnemySau sau)
	{
		this.poolSau.Store(sau);
	}

	private float GetDamage()
	{
		switch (this._state)
		{
		case Boss_WarBunker.EState.Attack1:
			return Mathf.Round(this.cacheEnemy.Damage * this.pDamageBi);
		case Boss_WarBunker.EState.Attack2_1:
		case Boss_WarBunker.EState.Attack2_2:
		case Boss_WarBunker.EState.Attack2_3:
			return Mathf.Round(this.cacheEnemy.Damage * this.pDamageLaser);
		case Boss_WarBunker.EState.Attack3_1:
		case Boss_WarBunker.EState.Attack3_2:
		case Boss_WarBunker.EState.Attack3_3:
			return Mathf.Round(this.cacheEnemy.Damage * this.pDamageSau);
		default:
			return this.cacheEnemy.Damage;
		}
	}

	private float GetSpeed()
	{
		Boss_WarBunker.EState state = this._state;
		switch (state)
		{
		case Boss_WarBunker.EState.Attack3_1:
		case Boss_WarBunker.EState.Attack3_2:
		case Boss_WarBunker.EState.Attack3_3:
			return Mathf.Round(this.cacheEnemy.Speed * this.pSpeedSau);
		default:
			if (state != Boss_WarBunker.EState.Attack1)
			{
				return this.cacheEnemy.Speed;
			}
			return Mathf.Round(this.cacheEnemy.Speed * this.pSpeedBi);
		}
	}

	public override void Hit(float damage)
	{
		if (GameMode.Instance.modePlay == GameMode.ModePlay.Boss_Mode)
		{
			this.hit++;
			if (this.hit >= 10)
			{
				this.hit = 0;
				GameManager.Instance.giftManager.CreateItemWeapon(CameraController.Instance.Position);
			}
		}
		if (this.HP > 0f)
		{
			GameManager.Instance.bossManager.ShowLineBloodBoss(this.HP, this.cacheEnemy.HP);
			this.skeletonAnimation.state.SetAnimation(1, this.anims[8], false);
			return;
		}
		this.Die();
		this.skeletonAnimation.state.SetEmptyAnimations(0f);
		if (this._state == Boss_WarBunker.EState.Attack2_2)
		{
			this.laser.DisableEff();
			this.laser.Off();
		}
		this._state = Boss_WarBunker.EState.Die;
		this.PlayAnim(false);
		GameManager.Instance.hudManager.HideControl();
		GameManager.Instance.bossManager.ShowLineBloodBoss(0f, this.cacheEnemy.HP);
		if (GameMode.Instance.Style == GameMode.GameStyle.SinglPlayer && GameMode.Instance.modePlay == GameMode.ModePlay.Campaign && !ProfileManager.bossModeProfile.bossProfiles[4].CheckUnlock(GameMode.Mode.NORMAL))
		{
			ProfileManager.bossModeProfile.bossProfiles[4].SetUnlock(GameMode.Mode.NORMAL, true);
			UIShowInforManager.Instance.ShowUnlock("War Bunker has been unlocked in BossMode!");
		}
	}

	private void Die()
	{
		this.skeletonAnimation.state.Event -= this.OnEvent;
		this.skeletonAnimation.state.Complete -= this.OnComplete;
		base.StartCoroutine(this.EffDie());
	}

	private IEnumerator EffDie()
	{
		Vector3 fxPos = this.transGuns[1].position;
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, fxPos, Vector3.one);
		yield return new WaitForSeconds(0.15f);
		fxPos.y -= 1f;
		fxPos.x += 1.5f;
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, fxPos, Vector3.one);
		yield return new WaitForSeconds(0.15f);
		fxPos = this.transGuns[0].position;
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, fxPos, Vector3.one);
		yield return new WaitForSeconds(0.15f);
		fxPos.y -= 1f;
		fxPos.x += 1.5f;
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, fxPos, Vector3.one);
		yield return new WaitForSeconds(0.15f);
		fxPos = this.transLaserPoint.position;
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, fxPos, Vector3.one);
		yield return new WaitForSeconds(0.35f);
		PlayerManagerStory.Instance.OnRunGameOver();
		fxPos = this.transSauCreatePoin.position;
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, fxPos, Vector3.one);
		yield return new WaitForSeconds(0.35f);
		fxPos = this.transGuns[0].position;
		fxPos.x += 0.5f;
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, fxPos, Vector3.one);
		yield return new WaitForSeconds(0.35f);
		fxPos = this.transGuns[1].position;
		fxPos.x += 0.5f;
		fxPos.y += 0.5f;
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, fxPos, Vector3.one);
		yield break;
	}

	private void OnEvent(TrackEntry trackEntry, Spine.Event e)
	{
		string text = e.ToString();
		if (text != null)
		{
			if (text == "attack1")
			{
				CameraController.Instance.Shake(CameraController.ShakeType.GrenadePlayer);
				this.CreateBullet();
				this._gunId++;
			}
		}
	}

	private void OnComplete(TrackEntry trackEntry)
	{
		if (trackEntry.Animation.Name.Contains("hit") || this._state == Boss_WarBunker.EState.Die)
		{
			return;
		}
		switch (this._state)
		{
		case Boss_WarBunker.EState.Attack1:
		case Boss_WarBunker.EState.Attack2_1:
		case Boss_WarBunker.EState.Attack2_3:
		case Boss_WarBunker.EState.Attack3_1:
		case Boss_WarBunker.EState.Attack3_3:
		case Boss_WarBunker.EState.Idle:
			this.ChangeState();
			break;
		case Boss_WarBunker.EState.Attack2_2:
			this._coutAttackLase++;
			if (this._coutAttackLase >= 2)
			{
				this._coutAttackLase = 0;
				this.ChangeState();
			}
			break;
		case Boss_WarBunker.EState.Attack3_2:
			this.CreateSau();
			this._sauCount++;
			if (this._sauCount > this._level)
			{
				this._sauCount = 0;
				this.ChangeState();
			}
			break;
		case Boss_WarBunker.EState.Open:
			this.bodyCollider2D.enabled = true;
			base.isInCamera = true;
			this.ChangeState();
			break;
		}
	}

	[SerializeField]
	private SkeletonAnimation skeletonAnimation;

	[SpineAnimation("", "", true, false)]
	[SerializeField]
	private string[] anims;

	[SerializeField]
	private LaserHandMade laser;

	[SerializeField]
	private GameObject objWarningLaser;

	[SerializeField]
	private Transform transLaserPoint;

	[SerializeField]
	private LayerMask ramboMask;

	[SerializeField]
	private Transform[] transGuns;

	[SerializeField]
	private List<BulletBi> listBi;

	[SerializeField]
	private Transform transSauCreatePoin;

	[SerializeField]
	private float sauHp = 30f;

	[SerializeField]
	private List<EnemySau> listSau;

	[SerializeField]
	private float pDamageBi;

	[SerializeField]
	private float pDamageSau;

	[SerializeField]
	private float pDamageLaser;

	[SerializeField]
	private float pSpeedBi;

	[SerializeField]
	private float pSpeedSau;

	private int _level;

	private Boss_WarBunker.EState _state;

	private Boss_WarBunker.EState _oldAttack;

	private bool _changeState;

	private bool _isPause;

	private int _gunId;

	private int _sauCount;

	private Vector3 _endPointLaser;

	private RaycastHit2D _hit;

	private ObjectPooling<BulletBi> poolBi;

	private ObjectPooling<EnemySau> poolSau;

	private float _laserSize;

	private float _veloSize;

	private int _coutAttackLase;

	private bool _crazy;

	private int hit;

	private enum EState
	{
		Attack1,
		Attack2_1,
		Attack2_2,
		Attack2_3,
		Attack3_1,
		Attack3_2,
		Attack3_3,
		Die,
		Hit,
		Idle,
		Open,
		Close
	}
}
