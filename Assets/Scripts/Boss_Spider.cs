using System;
using System.Collections;
using System.Collections.Generic;
using Com.LuisPedroFonseca.ProCamera2D;
using MyDataLoader;
using Newtonsoft.Json;
using Spine;
using Spine.Unity;
using UnityEngine;

public class Boss_Spider : BaseBoss
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
		if (this._state == Boss_Spider.EState.Die)
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
		this.InitBoss();
		try
		{
			ProCamera2D.Instance.OffsetX = 0f;
		}
		catch (Exception ex)
		{
		}
	}

	public void InitBoss()
	{
		try
		{
			float num = GameMode.Instance.GetMode();
			string text = (!SplashScreen._isLoadResourceDecrypt) ? this.Data_Encrypt.text : this.Data_Decrypt.text;
			string text2 = ProfileManager.DataEncryption.decrypt2(text);
			EnemyCharactor enemyCharactor = JsonConvert.DeserializeObject<EnemyCharactor>((!SplashScreen._isLoadResourceDecrypt) ? text2 : text);
			if (GameMode.Instance.Style == GameMode.GameStyle.MultiPlayer && GameMode.Instance.modePlay == GameMode.ModePlay.PvpMode)
			{
				num = 5f;
			}
			this.cacheEnemy = enemyCharactor.enemy[0];
			this.cacheEnemy.Damage *= num;
			this.cacheEnemy.HP *= num;
			this.HP = this.cacheEnemy.HP;
			this._level = (int)GameMode.Instance.EMode;
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(ex.Message);
		}
		base.gameObject.SetActive(true);
		GameManager.Instance.ListEnemy.Add(this);
		this.skeletonAnimation.state.Event += this.OnEvent;
		this.skeletonAnimation.state.Complete += this.OnComplete;
		this.listRockets[0].gameObject.transform.parent.parent = null;
		this.poolRocket = new ObjectPooling<RocketSpider>(this.listRockets.Count, null, null);
		for (int i = 0; i < this.listRockets.Count; i++)
		{
			this.poolRocket.Store(this.listRockets[i]);
		}
		this.listBombs[0].gameObject.transform.parent.parent = null;
		this.poolBomb = new ObjectPooling<BombSpider>(this.listBombs.Count, null, null);
		for (int j = 0; j < this.listBombs.Count; j++)
		{
			this.poolBomb.Store(this.listBombs[j]);
		}
		this.listNhenNhays[0].gameObject.transform.parent.parent = null;
		this.poolNhenNhay = new ObjectPooling<NhenNhay>(this.listNhenNhays.Count, null, null);
		for (int k = 0; k < this.listNhenNhays.Count; k++)
		{
			this.poolNhenNhay.Store(this.listNhenNhays[k]);
		}
		this.nhenNhayHp = Mathf.Round(this.HP / 15f);
		this._state = Boss_Spider.EState.Run;
		this._changeState = false;
		this._isPause = false;
		this._countNhenNhay = 0;
		this._countTurnRocket = 0;
		this._coolDownBomb = 0f;
		this._run2Back = false;
		this.isInit = true;
	}

	private void OnUpdate(float deltaTime)
	{
		if (!this.isInit || this._state == Boss_Spider.EState.Die)
		{
			return;
		}
		this.CachePos();
		if (!this.isBegin)
		{
			if (!this._changeState)
			{
				this.PlayAnim(0, true);
				this._changeState = true;
			}
			this._pos.x = Mathf.MoveTowards(this._pos.x, CameraController.Instance.camPos.x, this.cacheEnemy.Speed * 2f * deltaTime);
			this.transform.position = this._pos;
			if (Mathf.Abs(this._pos.x - CameraController.Instance.camPos.x) <= 4f)
			{
				this._changeState = false;
				this.ChangeStateToAttack();
				this.isBegin = true;
			}
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
		for (int i = 0; i < this.listNhenNhays.Count; i++)
		{
			if (this.listNhenNhays[i] && this.listNhenNhays[i].isInit)
			{
				this.listNhenNhays[i].OnUpdate(deltaTime);
			}
		}
		for (int j = 0; j < this.listRockets.Count; j++)
		{
			if (this.listRockets[j] && this.listRockets[j].isInit)
			{
				this.listRockets[j].OnUpdate(deltaTime);
			}
		}
	}

	private void CachePos()
	{
		this._pos = this.transform.position;
		this._posRambo = GameManager.Instance.player.transform.position;
	}

	private void StartState()
	{
		this._changeState = true;
		bool loop = false;
		switch (this._state)
		{
		case Boss_Spider.EState.Attack1_2:
		case Boss_Spider.EState.Attack3_2:
			loop = true;
			break;
		case Boss_Spider.EState.Attack3_1:
		case Boss_Spider.EState.Attack3_1_1:
			this._rocketId = 0;
			this.Flip(this._pos.x < this._posRambo.x);
			break;
		case Boss_Spider.EState.Run:
			loop = true;
			this.GetTargetX();
			this.Flip(this._pos.x < this._targetX);
			this._speedMove = this.cacheEnemy.Speed * 2f;
			break;
		case Boss_Spider.EState.Run2:
			loop = true;
			this.GetTargetX();
			this._run2Back = false;
			this.Flip(this._pos.x > this._targetX);
			this._speedMove = this.cacheEnemy.Speed * 2f;
			break;
		case Boss_Spider.EState.Run_Attack2_1:
			this.Flip(this._pos.x < CameraController.Instance.camPos.x);
			break;
		case Boss_Spider.EState.Run_Attack2_2:
			loop = true;
			this.Flip(this._pos.x < CameraController.Instance.camPos.x);
			this._coolDownBomb = this.coolDownbomb / 4f;
			this._speedMove = this.cacheEnemy.Speed * 3f;
			break;
		case Boss_Spider.EState.Walk:
			loop = true;
			this.GetTargetX();
			this._speedMove = this.cacheEnemy.Speed;
			break;
		}
		this.PlayAnim(0, loop);
	}

	private void UpdateState(float deltaTime)
	{
		switch (this._state)
		{
		case Boss_Spider.EState.Run:
		case Boss_Spider.EState.Walk:
			this.MoveToTarget(deltaTime);
			if (this._pos.x == this._targetX)
			{
				this.ChangeState();
			}
			break;
		case Boss_Spider.EState.Run2:
			this.MoveToTarget(deltaTime);
			if (this._pos.x == this._targetX)
			{
				if (!this._run2Back)
				{
					this._run2Back = true;
					this.Flip(this._pos.x > CameraController.Instance.camPos.x);
					this._targetX = ((Mathf.Abs(this._pos.x - CameraController.Instance.camPos.x) <= 7f) ? this._targetX : ((this._pos.x <= CameraController.Instance.camPos.x) ? (CameraController.Instance.camPos.x - 7f) : (CameraController.Instance.camPos.x + 7f)));
				}
				else
				{
					this.ChangeState();
				}
			}
			break;
		case Boss_Spider.EState.Run_Attack2_2:
		{
			this.transform.Translate((!this.skeletonAnimation.skeleton.FlipX) ? (-this._speedMove * deltaTime) : (this._speedMove * deltaTime), 0f, 0f);
			bool flag = !base.isInCamera && this.skeletonAnimation.skeleton.FlipX == this.transform.position.x > CameraController.Instance.camPos.x;
			if (!base.isInCamera)
			{
				this.ChangeState();
			}
			else
			{
				if (this._coolDownBomb > 0f)
				{
					this._coolDownBomb -= deltaTime;
				}
				if (this._coolDownBomb <= 0f)
				{
					this.CreateBomb();
					this._coolDownBomb = this.coolDownbomb - this.coolDownbomb * (float)this._level / 5f;
				}
			}
			break;
		}
		}
	}

	private void Pause(bool pause)
	{
		this._isPause = pause;
		this.skeletonAnimation.timeScale = (float)((!pause) ? 1 : 0);
		for (int i = 0; i < this.listBombs.Count; i++)
		{
			if (this.listBombs[i] && this.listBombs[i].isActiveAndEnabled)
			{
				this.listBombs[i].Pause(pause);
			}
		}
	}

	private void ChangeState()
	{
		switch (this._state)
		{
		case Boss_Spider.EState.Attack1_1:
			this._state = Boss_Spider.EState.Attack1_2;
			break;
		case Boss_Spider.EState.Attack1_2:
			this._state = Boss_Spider.EState.Attack1_3;
			break;
		case Boss_Spider.EState.Attack1_3:
		case Boss_Spider.EState.Attack3_3:
		case Boss_Spider.EState.Run_Attack2_2:
			this.ChangeStateToRefresh();
			break;
		case Boss_Spider.EState.Attack3_1:
		case Boss_Spider.EState.Attack3_1_1:
			this._state = Boss_Spider.EState.Attack3_2;
			break;
		case Boss_Spider.EState.Attack3_2:
			this._state = Boss_Spider.EState.Attack3_3;
			break;
		case Boss_Spider.EState.Idle:
		case Boss_Spider.EState.Run:
		case Boss_Spider.EState.Walk:
			this.ChangeStateToAttack();
			break;
		case Boss_Spider.EState.Run2:
			this._state = Boss_Spider.EState.Attack1_1;
			break;
		case Boss_Spider.EState.Run_Attack2_1:
			this._state = Boss_Spider.EState.Run_Attack2_2;
			break;
		}
		this._changeState = false;
	}

	private void ChangeStateToAttack()
	{
		if (Mathf.Abs(this._pos.x - CameraController.Instance.camPos.x) < 2f || Mathf.Abs(this._pos.x - this._posRambo.x) >= 6f)
		{
			this._state = ((UnityEngine.Random.Range(0, 2) != 1) ? Boss_Spider.EState.Attack3_1_1 : Boss_Spider.EState.Attack3_1);
		}
		else
		{
			this._state = ((UnityEngine.Random.Range(0, 2) != 1) ? Boss_Spider.EState.Run_Attack2_1 : Boss_Spider.EState.Run2);
		}
	}

	private void ChangeStateToRefresh()
	{
		if (!base.isInCamera)
		{
			this._state = Boss_Spider.EState.Run;
			return;
		}
		if (Mathf.Abs(this._pos.x - CameraController.Instance.camPos.x) < 2f)
		{
			this._state = Boss_Spider.EState.Walk;
		}
		else
		{
			this._state = ((UnityEngine.Random.Range(0, 2) != 1) ? Boss_Spider.EState.Run : Boss_Spider.EState.Idle);
		}
	}

	private void GetTargetX()
	{
		switch (this._state)
		{
		case Boss_Spider.EState.Run:
		{
			float num = UnityEngine.Random.Range(1f, 5f);
			if (base.isInCamera)
			{
				this._targetX = ((this._pos.x <= CameraController.Instance.camPos.x) ? (CameraController.Instance.camPos.x + num) : (CameraController.Instance.camPos.x - num));
			}
			else
			{
				this._targetX = ((this._pos.x <= CameraController.Instance.camPos.x) ? (CameraController.Instance.camPos.x - num) : (CameraController.Instance.camPos.x + num));
			}
			break;
		}
		case Boss_Spider.EState.Run2:
		{
			float num = CameraController.Instance.Size().x + 4f;
			this._targetX = ((this._pos.x <= CameraController.Instance.camPos.x) ? (CameraController.Instance.camPos.x - num) : (CameraController.Instance.camPos.x + num));
			break;
		}
		case Boss_Spider.EState.Run_Attack2_2:
			this._targetX = ((this._pos.x <= CameraController.Instance.camPos.x) ? (CameraController.Instance.camPos.x + 8f) : (CameraController.Instance.camPos.x - 8f));
			break;
		case Boss_Spider.EState.Walk:
			this._targetX = ((this._pos.x <= CameraController.Instance.camPos.x) ? (CameraController.Instance.camPos.x - 5f) : (CameraController.Instance.camPos.x + 5f));
			break;
		}
	}

	private void PlayAnim(int track, bool loop = false)
	{
		this.skeletonAnimation.state.SetAnimation(track, this.anims[(int)this._state], loop);
	}

	private void PlayAnim(Boss_Spider.EState state, int track = 0, bool loop = false)
	{
		this.skeletonAnimation.state.SetAnimation(track, this.anims[(int)state], loop);
	}

	private void Flip(bool flip)
	{
		for (int i = 0; i < this.colliders.Length; i++)
		{
			this._offset = this.colliders[i].offset;
			this._offset.x = ((flip != this.skeletonAnimation.skeleton.FlipX) ? (-this._offset.x) : this._offset.x);
			this.colliders[i].offset = this._offset;
		}
		if (this.skeletonAnimation.skeleton.FlipX != flip)
		{
			this.tfGroup.localScale = new Vector3(-this.tfGroup.localScale.x, 1f, 1f);
			this.skeletonAnimation.skeleton.FlipX = flip;
		}
	}

	private void MoveToTarget(float deltaTime)
	{
		this._pos.x = Mathf.MoveTowards(this._pos.x, this._targetX, this._speedMove * deltaTime);
		this.transform.position = this._pos;
	}

	private void CreateBomb()
	{
		BombSpider bombSpider = this.poolBomb.New();
		if (!bombSpider)
		{
			bombSpider = UnityEngine.Object.Instantiate<BombSpider>(this.listBombs[0]);
			this.listBombs.Add(bombSpider);
			bombSpider.gameObject.transform.parent = this.listBombs[0].gameObject.transform.parent;
		}
		Vector3 pos = this.transform.position + ((!this.skeletonAnimation.skeleton.FlipX) ? (Vector3.right * 4f) : (Vector3.left * 4f));
		bombSpider.Init(this.skeletonAnimation.skeleton.FlipX, this.GetDamage(), pos, new Action<BombSpider>(this.BombHide));
	}

	private void BombHide(BombSpider bomb)
	{
		this.poolBomb.Store(bomb);
	}

	private void CreateNhenNhay()
	{
		NhenNhay nhenNhay = this.poolNhenNhay.New();
		if (!nhenNhay)
		{
			nhenNhay = UnityEngine.Object.Instantiate<NhenNhay>(this.listNhenNhays[0]);
			this.listNhenNhays.Add(nhenNhay);
			nhenNhay.gameObject.transform.parent = this.listNhenNhays[0].gameObject.transform.parent;
		}
		nhenNhay.Init(this.GetDamage(), this.GetSpeed(), this.nhenNhayHp, !this.skeletonAnimation.skeleton.FlipX, this.nhenNhayCreatePoin.position, new Action<NhenNhay>(this.NhenNhayHide), true);
	}

	private void NhenNhayHide(NhenNhay nhen)
	{
		this.poolNhenNhay.Store(nhen);
	}

	private void CreateRocket(int id)
	{
		RocketSpider rocketSpider = this.poolRocket.New();
		if (!rocketSpider)
		{
			rocketSpider = UnityEngine.Object.Instantiate<RocketSpider>(this.listRockets[0]);
			rocketSpider.gameObject.transform.parent = this.listRockets[0].gameObject.transform.parent;
			this.listRockets.Add(rocketSpider);
		}
		rocketSpider.Init(id, this.GetDamage(), this.GetSpeed(), this.rocketPoints[id].position, this.rocketPoints[id].position - this.rocketPoints[id + 3].position, new Action<RocketSpider>(this.RocketHide));
	}

	private void RocketHide(RocketSpider rocket)
	{
		this.poolRocket.Store(rocket);
	}

	private float GetSpeed()
	{
		Boss_Spider.EState state = this._state;
		if (state == Boss_Spider.EState.Attack1_2)
		{
			return this.cacheEnemy.Speed * this.pSpeedNheNhay;
		}
		if (state != Boss_Spider.EState.Attack3_2)
		{
			return this.cacheEnemy.Speed;
		}
		return this.cacheEnemy.Speed * this.pSpeedRocket;
	}

	private float GetDamage()
	{
		Boss_Spider.EState state = this._state;
		switch (state)
		{
		case Boss_Spider.EState.Attack1_2:
			return this.cacheEnemy.Damage * this.pDamageNhenNhay;
		default:
			if (state != Boss_Spider.EState.Run_Attack2_2)
			{
				return this.cacheEnemy.Damage;
			}
			return this.cacheEnemy.Damage * this.pDamageBomb;
		case Boss_Spider.EState.Attack3_2:
			return this.cacheEnemy.Damage * this.pDamageRocket;
		}
	}

	public void ShakeCamera(int numberOfShake, float distance)
	{
		if (GameManager.Instance != null && GameManager.Instance.StateManager.EState != EGamePlay.WIN)
		{
			CameraController.Instance.Shake(CameraController.ShakeType.ShakeEnemy);
		}
	}

	public override void Hit(float damage)
	{
		base.Hit(damage);
		if (GameMode.Instance.modePlay == GameMode.ModePlay.Boss_Mode)
		{
			this.hit++;
			if (this.hit >= 10)
			{
				this.hit = 0;
				GameManager.Instance.giftManager.CreateItemWeapon(this.transform.position);
			}
		}
		if (this.HP > 0f)
		{
			this.PlayAnim(Boss_Spider.EState.Hit, 1, false);
			GameManager.Instance.bossManager.ShowLineBloodBoss(this.HP, this.cacheEnemy.HP);
		}
		else
		{
			this.Die();
			GameManager.Instance.hudManager.HideControl();
			if (GameMode.Instance.Style == GameMode.GameStyle.SinglPlayer && GameMode.Instance.modePlay == GameMode.ModePlay.Campaign && !ProfileManager.bossModeProfile.bossProfiles[3].CheckUnlock(GameMode.Mode.NORMAL))
			{
				ProfileManager.bossModeProfile.bossProfiles[3].SetUnlock(GameMode.Mode.NORMAL, true);
				UIShowInforManager.Instance.ShowUnlock("Spider Toxic has been unlocked in BossMode!");
			}
			GameManager.Instance.bossManager.ShowLineBloodBoss(0f, this.cacheEnemy.HP);
			this.skeletonAnimation.state.SetEmptyAnimations(0f);
			this._state = Boss_Spider.EState.Die;
			this.PlayAnim(1, false);
		}
	}

	private void Die()
	{
		if (this.State == ECharactor.DIE)
		{
			return;
		}
		this.State = ECharactor.DIE;
		for (int i = 0; i < this.listNhenNhays.Count; i++)
		{
			if (this.listNhenNhays[i].isInit)
			{
				GameManager.Instance.fxManager.ShowEffect(5, this.listNhenNhays[i].transform.position, Vector3.one, true, true);
				this.listNhenNhays[i].gameObject.SetActive(false);
			}
		}
		if (this.lineBloodEnemy != null)
		{
			this.lineBloodEnemy.Hide();
		}
		base.StartCoroutine(this.EffectDie());
	}

	private IEnumerator EffectDie()
	{
		float time = 0.45f;
		for (int i = 0; i < this.tfTarget.Count; i++)
		{
			Vector3 pos = this.tfTarget[i].position;
			this.ShakeCamera(4, 0.25f);
			GameManager.Instance.fxManager.ShowFxNoSpine01(0, pos, Vector3.one);
			yield return new WaitForSeconds(time);
			time -= 0.05f;
		}
		PlayerManagerStory.Instance.OnRunGameOver();
		yield break;
	}

	private void OnEvent(TrackEntry trackEntry, Spine.Event e)
	{
		if (e.Data.Name.Contains("no"))
		{
			this.ShakeCamera(4, 0.25f);
			GameManager.Instance.fxManager.ShowFxNoSpine01(0, this.transform.position, Vector3.one * 1.5f);
		}
		Boss_Spider.EState state = this._state;
		if (state != Boss_Spider.EState.Attack1_2)
		{
			if (state == Boss_Spider.EState.Attack3_2)
			{
				this.CreateRocket(this._rocketId);
				this._rocketId++;
			}
		}
		else
		{
			this.CreateNhenNhay();
		}
	}

	private void OnComplete(TrackEntry trackEntry)
	{
		if (trackEntry.Animation.Name.Contains("hit"))
		{
			return;
		}
		switch (this._state)
		{
		case Boss_Spider.EState.Attack1_1:
		case Boss_Spider.EState.Attack1_3:
		case Boss_Spider.EState.Attack3_1:
		case Boss_Spider.EState.Attack3_3:
		case Boss_Spider.EState.Attack3_1_1:
		case Boss_Spider.EState.Idle:
		case Boss_Spider.EState.Run_Attack2_1:
			this.ChangeState();
			break;
		case Boss_Spider.EState.Attack1_2:
			this._countNhenNhay++;
			if (this._countNhenNhay > this._level)
			{
				this._countNhenNhay = 0;
				this.ChangeState();
			}
			break;
		case Boss_Spider.EState.Attack3_2:
			this._countTurnRocket++;
			this._rocketId = 0;
			if (this._countTurnRocket > this._level)
			{
				this._countTurnRocket = 0;
				this.ChangeState();
			}
			break;
		}
	}

	[SerializeField]
	private SkeletonAnimation skeletonAnimation;

	[SpineAnimation("", "", true, false)]
	[SerializeField]
	private string[] anims;

	[SerializeField]
	private Collider2D[] colliders;

	[SerializeField]
	private Transform tfGroup;

	[SerializeField]
	private float pDamageRocket;

	[SerializeField]
	private float pDamageNhenNhay;

	[SerializeField]
	private float pDamageBomb;

	[SerializeField]
	private float pSpeedRocket;

	[SerializeField]
	private float pSpeedNheNhay;

	[SerializeField]
	private float coolDownbomb;

	[SerializeField]
	private List<RocketSpider> listRockets;

	[SerializeField]
	private Transform[] rocketPoints;

	[SerializeField]
	private List<BombSpider> listBombs;

	[SerializeField]
	private List<NhenNhay> listNhenNhays;

	[SerializeField]
	private Transform nhenNhayCreatePoin;

	[SerializeField]
	private float nhenNhayHp;

	private Boss_Spider.EState _state;

	private bool _changeState;

	private bool _isPause;

	private Vector3 _pos;

	private Vector3 _posRambo;

	private int _countNhenNhay;

	private int _level;

	private int _countTurnRocket;

	private float _targetX;

	private float _speedMove;

	private bool _run2Back;

	private float _coolDownBomb;

	private int _rocketId;

	private ObjectPooling<RocketSpider> poolRocket;

	private ObjectPooling<BombSpider> poolBomb;

	private ObjectPooling<NhenNhay> poolNhenNhay;

	private bool isBegin;

	private Vector2 _offset;

	private int hit;

	private enum EState
	{
		Attack_3,
		Attack1_1,
		Attack1_2,
		Attack1_3,
		Attack3_1,
		Attack3_2,
		Attack3_3,
		Attack3_1_1,
		Die,
		Hit,
		Idle,
		Run,
		Run2,
		Run_Attack2_1,
		Run_Attack2_2,
		Walk
	}
}
