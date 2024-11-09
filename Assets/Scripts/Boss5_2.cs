using System;
using System.Collections;
using System.Collections.Generic;
using Com.LuisPedroFonseca.ProCamera2D;
using MyDataLoader;
using Newtonsoft.Json;
using Spine;
using Spine.Unity;
using UnityEngine;

public class Boss5_2 : BaseBoss
{
	private void Update()
	{
		if (!this.isInit || this.State == ECharactor.DIE || GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
		if (!this.Begin())
		{
			return;
		}
		this.OnUpdate();
	}

	private bool Begin()
	{
		if (this._isBegin)
		{
			return true;
		}
		this.transform.position = Vector3.MoveTowards(this.transform.position, this.posBegin, Time.deltaTime);
		if (this.transform.position.y == this.posBegin.y)
		{
			this._isBegin = true;
			this.State = ECharactor.ATTACK_2;
			this._changeState = false;
		}
		return false;
	}

	public override void Init()
	{
		base.Init();
		this.InitEnemy();
	}

	public void InitEnemy()
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
			enemyCharactor.enemy[0].Damage *= num;
			enemyCharactor.enemy[0].Speed *= ((num > 2f) ? 2f : num);
			this.InitEnemy(enemyCharactor, 0);
			this.cacheEnemy.HP = this.HP;
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(ex.Message);
		}
		this._listMiniBossNewCreate = new List<MiniBoss5_2>();
		this._listHutCollider = new List<Collider2D>();
		this.damageHut = this.GetDamage(Boss5_2.Skill.Hut);
		this.deathBody = "Death_body";
		this.InitHpUnit();
		this.InitUnitEnemy();
		this.InitStringAnimDeathUnit();
		this.InitUnitAction();
		this.InitEventAnim();
		this.InitHutCollider();
		this.InitActionSkillSet();
		this._camTransform = CameraController.Instance.transform;
		this._ramboTransform = GameManager.Instance.player.transform;
		this.isInit = true;
		this._isBegin = false;
		this._rageState = false;
		try
		{
			ProCamera2D.Instance.OffsetX = 0f;
		}
		catch (Exception ex2)
		{
		}
	}

	private void InitUnitEnemy()
	{
		for (int i = 0; i < this.listBoxUnit.Length; i++)
		{
			this.listBoxUnit[i].InitEnemy(this.HP);
			GameManager.Instance.ListEnemy.Add(this.listBoxUnit[i]);
		}
	}

	private void InitHpUnit()
	{
		this._hpPerOneUnit = (float)((int)this.cacheEnemy.HP / 8);
		for (int i = 0; i < 8; i++)
		{
			this._listHpUnitBoss[i] = this._hpPerOneUnit;
		}
		this._listHpUnitBoss[8] = 1f;
		this._listHpUnitBoss[9] = 1f;
	}

	private void InitStringAnimDeathUnit()
	{
		this.deathUnits = new string[8];
		this.deathIdleUnits = new string[8];
		for (int i = 0; i < 8; i++)
		{
			this.deathUnits[i] = "Death0" + (i + 1).ToString();
			this.deathIdleUnits[i] = "Idle_Death0" + (i + 1).ToString();
		}
	}

	private void InitUnitAction()
	{
		for (int i = 0; i < this.listBoxUnit.Length; i++)
		{
			this.listBoxUnit[i].OnHitDamage = new Action<int, float>(this.OnHitDamge);
		}
	}

	private void InitEventAnim()
	{
		this.skeletonAnimations[8].state.Event += this.HandleEventBody;
		this.skeletonAnimation.skeleton.FlipX = false;
	}

	private void InitHutCollider()
	{
	}

	private void InitActionSkillSet()
	{
		for (int i = 0; i < this.listSet.Length; i++)
		{
			this.listSet[i].OnSkillSet = new Action<IHealth>(this.OnSkillSet);
		}
	}

	private void ResetAction(MiniBoss5_2 obj)
	{
	}

	private void OnetimeInitAction(MiniBoss5_2 obj)
	{
	}

	private void ResetAction(BulletMiniBoss5_2 obj)
	{
	}

	private void OnetimeInitAction(BulletMiniBoss5_2 obj)
	{
	}

	private void OnUpdate()
	{
		if (this.State == ECharactor.DIE)
		{
			return;
		}
		if (!this._rageState && this.HP < this.cacheEnemy.HP / 2f)
		{
			this._rageState = true;
		}
		this.OnUpdateState();
	}

	private void OnUpdateState()
	{
		if (!this._changeState)
		{
			this._changeState = true;
			switch (this.State)
			{
			case ECharactor.ATTACK:
				this.PlayAnimation(this.attacks[0], false);
				this._lerpT = Time.deltaTime;
				break;
			case ECharactor.ATTACK_2:
				base.StartCoroutine(this.OnAttack2());
				this._midLerpPosZero = this.transform.position;
				this._minLerpX = this.transform.position.x - 0.75f;
				this._maxLerpX = this.transform.position.x + 0.75f;
				this._lerpT = 0.5f;
				break;
			case ECharactor.ATTACK_3:
				this.PlayAnimation(this.attacks[2], false);
				break;
			case ECharactor.IDLE:
				base.StartCoroutine(this.OnIdle());
				break;
			}
		}
		switch (this.State)
		{
		case ECharactor.ATTACK:
			if (this.transform.position.x != this._midLerpPosZero.x)
			{
				this.transform.position = Vector3.Lerp(this.transform.position, this._midLerpPosZero, this._lerpT);
				this._lerpT += Time.deltaTime * 0.035f;
			}
			break;
		case ECharactor.ATTACK_2:
			this.transform.position = new Vector3(Mathf.Lerp(this._minLerpX, this._maxLerpX, this._lerpT), this.transform.position.y, this.transform.position.z);
			this._lerpT += Time.deltaTime * 0.35f;
			if (this._lerpT > 1f)
			{
				this._maxLerpX += this._minLerpX;
				this._minLerpX = this._maxLerpX - this._minLerpX;
				this._maxLerpX -= this._minLerpX;
				this._lerpT = 0f;
			}
			break;
		}
	}

	private void EditorTest()
	{
	}

	private void CreateMiniBoss()
	{
		MiniBoss5_2 miniBoss5_ = EnemyManager.Instance.CreateMiniShip();
		miniBoss5_.targetMove.x = UnityEngine.Random.Range(-2f, 2f);
		miniBoss5_.targetMove.y = UnityEngine.Random.Range(0f, 1f);
		miniBoss5_.targetMove.z = 0f;
		miniBoss5_.targetMove += this.transform.position;
		miniBoss5_.cacheEnemyData = new EnemyDataInfo();
		miniBoss5_.cacheEnemyData.pos_x = this.transform.position.x;
		miniBoss5_.cacheEnemyData.pos_y = this.transform.position.y + 3f;
		miniBoss5_.cacheEnemyData.level = miniBoss5_.data.datas.Length - 1;
		miniBoss5_.InitEnemy(true, delegate(MiniBoss5_2 m)
		{
			EnemyManager.Instance.PoolMiniShip.Store(m);
		});
		this._listMiniBossNewCreate.Add(miniBoss5_);
		GameManager.Instance.ListEnemy.Add(miniBoss5_);
	}

	private void OnHutAction(Collider2D obj, IHealth hpScript)
	{
	}

	private void OnSkillSet(IHealth obj)
	{
		obj.AddHealthPoint(-this.GetDamage(Boss5_2.Skill.Set), EWeapon.NONE);
	}

	private void PlayAnimation(string anim, bool loop = false)
	{
		for (int i = 0; i < this.skeletonAnimations.Length; i++)
		{
			if (this._listHpUnitBoss[i] > 0f)
			{
				this.skeletonAnimations[i].state.SetAnimation(1, anim, loop);
			}
			else if (anim == this.idle)
			{
				this.skeletonAnimations[i].state.SetAnimation(3, this.idleDeath, loop);
			}
		}
	}

	private void PlayAnimationSet(int[] listID)
	{
		this._timePlayAnimSet = this._timeSetZero;
		for (int i = 0; i < listID.Length; i++)
		{
			if (this._listHpUnitBoss[listID[i]] > 0f)
			{
				base.StartCoroutine(this.OnActiveSet(listID[i]));
				this._timePlayAnimSet = this._timeSetOne / 2f;
				this.skeletonAnimations[listID[i]].state.SetAnimation(1, this.attacks[1], false);
			}
		}
	}

	private IEnumerator OnActiveSet(int v)
	{
		yield return new WaitForSeconds(0.28f);
		this.listSet[v].gameObject.SetActive(true);
		yield return new WaitForSeconds(this._timeSetActive);
		this.listSet[v].gameObject.SetActive(false);
		yield break;
	}

	private void ActiveSet(int setId)
	{
		this.listSet[setId].gameObject.SetActive(true);
	}

	private IEnumerator OnAttack1()
	{
		yield return new WaitForSeconds(0.2f);
		for (int i = 0; i < this.numberMiniBossPerOne; i++)
		{
			this.CreateMiniBoss();
		}
		yield return new WaitForSeconds(2.467f);
		this.State = ECharactor.IDLE;
		this._changeState = false;
		yield break;
	}

	private IEnumerator OnAttack2()
	{
		this.PlayAnimationSet(new int[]
		{
			0,
			2,
			4
		});
		yield return new WaitForSeconds(this._timeSetOne);
		this.PlayAnimationSet(new int[]
		{
			1,
			3,
			6
		});
		yield return new WaitForSeconds(this._timeSetOne);
		for (int i = 0; i < 7; i++)
		{
			this.PlayAnimationSet(new int[]
			{
				i
			});
			yield return new WaitForSeconds(this._timePlayAnimSet);
		}
		this.PlayAnimationSet(new int[]
		{
			7
		});
		yield return new WaitForSeconds(1.1f);
		this.State = ECharactor.ATTACK;
		this._changeState = false;
		yield break;
	}

	private IEnumerator OnAttack3()
	{
		this.PlaySound(1);
		this.hutCollider.gameObject.SetActive(true);
		yield return new WaitForSeconds(2.1f);
		this.hutCollider.gameObject.SetActive(false);
		for (int i = 0; i < GameManager.Instance.ListRambo.Count; i++)
		{
			GameManager.Instance.ListRambo[i].UnFreeze();
		}
		yield return new WaitForSeconds(0.3f);
		this.State = ECharactor.ATTACK_2;
		this._changeState = false;
		yield break;
	}

	private IEnumerator OnIdle()
	{
		for (int i = 0; i < 10; i++)
		{
			if (this._listHpUnitBoss[i] > 0f)
			{
				this.skeletonAnimations[i].state.SetAnimation(1, this.idle, false);
			}
			else
			{
				this.skeletonAnimations[i].state.SetAnimation(3, this.deathIdleUnits[i], false);
			}
		}
		yield return new WaitForSeconds(1.34f);
		this.State = ECharactor.ATTACK_3;
		this._changeState = false;
		yield break;
	}

	private IEnumerator DisActiveSets(int id)
	{
		yield return new WaitForSeconds(this._timeSetActive);
		this.listSet[id].gameObject.SetActive(false);
		yield break;
	}

	private void PlaySound(int id)
	{
		if (ProfileManager.settingProfile.IsSound)
		{
			this.audioSources[id].Play();
		}
	}

	private float GetDamage(Boss5_2.Skill skill)
	{
		return Mathf.Round(this.ratioDamages[(int)skill] * this.cacheEnemy.Damage);
	}

	private float GetSpeed(Boss5_2.Skill skill)
	{
		return Mathf.Round(this.ratioSpeed[(int)skill] * this.cacheEnemy.Speed);
	}

	private void HandleEvent(Spine.AnimationState state, int trackIndex, Spine.Event e)
	{
		if (state.GetCurrent(trackIndex) == null)
		{
			return;
		}
		this.isSet = false;
		string name = e.Data.Name;
		switch (name)
		{
		case "Attack02":
			this.isSet = true;
			this.ActiveSet(0);
			base.StartCoroutine(this.DisActiveSets(0));
			break;
		case "Attack02_02":
			this.isSet = true;
			this.ActiveSet(1);
			base.StartCoroutine(this.DisActiveSets(1));
			break;
		case "Attack02_03":
			this.isSet = true;
			this.ActiveSet(2);
			base.StartCoroutine(this.DisActiveSets(2));
			break;
		case "Attack02_04":
			this.isSet = true;
			this.ActiveSet(3);
			base.StartCoroutine(this.DisActiveSets(3));
			break;
		case "Attack02_05":
			this.isSet = true;
			this.ActiveSet(4);
			base.StartCoroutine(this.DisActiveSets(4));
			break;
		case "Attack02_06":
			this.isSet = true;
			this.ActiveSet(5);
			base.StartCoroutine(this.DisActiveSets(5));
			break;
		case "Attack02_07":
			this.isSet = true;
			this.ActiveSet(6);
			base.StartCoroutine(this.DisActiveSets(6));
			break;
		case "Attack02_08":
			this.isSet = true;
			this.ActiveSet(7);
			base.StartCoroutine(this.DisActiveSets(7));
			break;
		}
		if (this.isSet)
		{
			this.PlaySound(0);
		}
	}

	private void HandleEventBody(TrackEntry entry, Spine.Event e)
	{
		if (entry == null)
		{
			return;
		}
		string name = e.Data.Name;
		if (name != null)
		{
			if (!(name == "Attack01"))
			{
				if (name == "Attack03")
				{
					base.StartCoroutine(this.OnAttack3());
				}
			}
			else
			{
				base.StartCoroutine(this.OnAttack1());
			}
		}
	}

	private void HandleComplete(TrackEntry entry)
	{
		if (entry == null)
		{
			return;
		}
	}

	private void OnHitDamge(int unitId, float damage)
	{
		if (this.State == ECharactor.DIE || GameManager.Instance.StateManager.EState != EGamePlay.RUNNING || !this._isBegin)
		{
			return;
		}
		if (this._listHpUnitBoss[unitId] <= 0f)
		{
			return;
		}
		if (GameMode.Instance.modePlay == GameMode.ModePlay.Boss_Mode)
		{
			this._hit++;
			if (this._hit >= 10)
			{
				this._hit = 0;
				GameManager.Instance.giftManager.CreateItemWeapon(this.transform.position);
			}
		}
		this._listHpUnitBoss[unitId] += damage;
		GameManager.Instance.bossManager.ShowLineBloodBoss(this._listHpUnitBoss[unitId], this._hpPerOneUnit);
		if (this._listHpUnitBoss[unitId] > 0f)
		{
			this.skeletonAnimations[unitId].state.SetAnimation(2, this.hit, false);
		}
		else
		{
			this._countUnitDie++;
			if (this._countUnitDie == 3)
			{
				this.numberMiniBossPerOne = 4;
				this._timeSetOne -= 0.3f;
				this.minusSpeedAffterHut += 0.25f;
			}
			GameManager.Instance.bossManager.ShowLineBloodBoss(0f, this._hpPerOneUnit);
			GameManager.Instance.ListEnemy.Remove(this.listBoxUnit[unitId]);
			this.listBoxUnit[unitId].gameObject.SetActive(false);
			this.skeletonAnimations[unitId].skeleton.SetColor(Color.white);
			this.skeletonAnimations[unitId].state.ClearTracks();
			this.skeletonAnimations[unitId].state.SetAnimation(3, this.deathUnits[unitId], false);
			base.StartCoroutine(this.EffectDieUnit(this.listBoxUnit[unitId].transform.position));
			this.CheckUnit();
		}
	}

	public void CheckUnit()
	{
		for (int i = 0; i < 8; i++)
		{
			if (this._listHpUnitBoss[i] > 0f)
			{
				return;
			}
		}
		if (this.State != ECharactor.DIE)
		{
			DailyQuestManager.Instance.MissionBoss(4, 1, delegate(bool isCompleted, DailyQuestManager.InforQuest infor)
			{
				if (isCompleted)
				{
					UIShowInforManager.Instance.OnShowDailyQuest(infor.Desc, infor.item, infor.amount);
				}
			});
			base.StopAllCoroutines();
			PlayerManagerStory.Instance.OnRunGameOver();
			if (GameMode.Instance.Style == GameMode.GameStyle.SinglPlayer && GameMode.Instance.modePlay == GameMode.ModePlay.Campaign && !ProfileManager.bossModeProfile.bossProfiles[15].CheckUnlock(GameMode.Mode.NORMAL))
			{
				ProfileManager.bossModeProfile.bossProfiles[15].SetUnlock(GameMode.Mode.NORMAL, true);
				UIShowInforManager.Instance.ShowUnlock("U_F_O has been unlocked in BossMode!");
			}
			GameManager.Instance.bossManager.ShowLineBloodBoss(0f, this.cacheEnemy.HP);
			GameManager.Instance.hudManager.HideControl();
			this.skeletonAnimations[8].state.ClearTracks();
			this.skeletonAnimations[9].state.ClearTracks();
			this.skeletonAnimations[8].state.SetAnimation(3, this.deathBody, false);
			this.skeletonAnimations[9].state.SetAnimation(3, this.death, false);
			base.StartCoroutine(this.EffectDie());
			this.State = ECharactor.DIE;
		}
	}

	private IEnumerator EffectDieUnit(Vector3 posDie)
	{
		GameManager.Instance.fxManager.ShowEffect(5, posDie, Vector3.one, true, true);
		yield return new WaitForSeconds(0.2f);
		Vector3 pos = posDie;
		Vector3 pos2 = posDie;
		pos.x += 0.8f;
		pos.y += 1.2f;
		pos2.x -= 1f;
		pos2.y += 0.6f;
		GameManager.Instance.fxManager.ShowEffect(5, pos, Vector3.one, true, true);
		GameManager.Instance.fxManager.ShowEffect(5, pos2, Vector3.one, true, true);
		yield break;
	}

	private IEnumerator EffectDie()
	{
		Vector3 pos = this.transform.position;
		Vector3 pos2 = pos;
		Vector3 pos3 = pos;
		pos2.x += UnityEngine.Random.Range(-3f, 3f);
		pos2.y += UnityEngine.Random.Range(1.5f, 3.3f);
		pos3.x += UnityEngine.Random.Range(-3f, 3f);
		pos3.y += UnityEngine.Random.Range(1.5f, 3.3f);
		GameManager.Instance.fxManager.ShowEffect(5, pos2, Vector3.one, true, true);
		GameManager.Instance.fxManager.ShowEffect(5, pos3, Vector3.one, true, true);
		yield return new WaitForSeconds(0.3f);
		base.StartCoroutine(this.EffectDie());
		yield break;
	}

	[SerializeField]
	[Header("*******************************")]
	private SkeletonAnimation skeletonAnimation;

	[SerializeField]
	public SkeletonAnimation[] skeletonAnimations;

	[Header("Animations String:")]
	[SpineAnimation("", "", true, false)]
	public string start;

	[SpineAnimation("", "", true, false)]
	public string[] attacks;

	[SpineAnimation("", "", true, false)]
	public string idle;

	[SpineAnimation("", "", true, false)]
	public string hit;

	[SpineAnimation("", "", true, false)]
	public string death;

	[SpineAnimation("", "", true, false)]
	public string idleDeath;

	[SpineAnimation("", "", true, false)]
	public string deathBody;

	[SpineAnimation("", "", true, false)]
	public string[] deathUnits;

	[SpineAnimation("", "", true, false)]
	public string[] deathIdleUnits;

	[SerializeField]
	[Header("Private variable:")]
	private BoxUnit[] listBoxUnit;

	[SerializeField]
	private Animator animator;

	[SerializeField]
	private SkillSet_Boss5_2[] listSet;

	[SerializeField]
	private SkillHut_Boss5_2 hutCollider;

	[SerializeField]
	private float[] ratioDamages;

	[SerializeField]
	private float[] ratioSpeed;

	[SerializeField]
	private int numberMiniBossPerOne;

	[SerializeField]
	private AudioSource[] audioSources;

	public float total_HP_Miniboss;

	public float damageHut;

	public float minusSpeedAffterHut;

	public float timeSlowWhenHut;

	public Vector2 posBegin;

	private Transform _ramboTransform;

	private Transform _camTransform;

	private List<MiniBoss5_2> _listMiniBossNewCreate;

	private List<Collider2D> _listHutCollider;

	private float[] _listHpUnitBoss = new float[10];

	private float _hpPerOneUnit;

	private bool _isBegin;

	private float _timeSetActive = 0.21f;

	private bool _rageState;

	private int _CountAttack2;

	private bool _changeState;

	private float _timePlayAnimSet;

	private float _timeSetZero;

	private float _timeSetOne = 0.8f;

	private Vector3 _midLerpPosZero;

	private float _maxLerpX;

	private float _minLerpX;

	private float _lerpT;

	private int _countUnitDie;

	private bool isSet;

	private int _hit;

	private enum Skill
	{
		Set,
		Hut,
		Mini
	}
}
