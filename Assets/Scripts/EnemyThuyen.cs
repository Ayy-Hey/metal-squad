using System;
using System.Collections;
using MyDataLoader;
using Newtonsoft.Json;
using Spine;
using Spine.Unity;
using UnityEngine;

public class EnemyThuyen : BaseEnemy
{
	private void OnValidate()
	{
		try
		{
			if (!this.Data_Encrypt)
			{
				this.Data_Encrypt = Resources.Load<TextAsset>("Charactor/Enemies/Encrypt/" + base.gameObject.name.ToString());
			}
			if (!this.Data_Decrypt)
			{
				this.Data_Decrypt = Resources.Load<TextAsset>("Charactor/Enemies/Decrypt/" + base.gameObject.name.ToString());
			}
			if (!this.skeletonAnimThuyen)
			{
				this.skeletonAnimThuyen = base.GetComponent<SkeletonAnimation>();
			}
			Spine.Animation[] items = this.skeletonAnimThuyen.skeletonDataAsset.GetAnimationStateData().SkeletonData.Animations.Items;
			this.animsThuyen = new string[items.Length];
			for (int i = 0; i < items.Length; i++)
			{
				this.animsThuyen[i] = items[i].Name;
			}
			if (this.skeletonAnim3Rocket)
			{
				items = this.skeletonAnim3Rocket.skeletonDataAsset.GetAnimationStateData().SkeletonData.Animations.Items;
				this.anims3Rocket = new string[items.Length];
				for (int j = 0; j < items.Length; j++)
				{
					this.anims3Rocket[j] = items[j].Name;
				}
			}
			if (this.skeletonAnim1Rocket)
			{
				items = this.skeletonAnim1Rocket.skeletonDataAsset.GetAnimationStateData().SkeletonData.Animations.Items;
				this.anims1Rocket = new string[items.Length];
				for (int k = 0; k < items.Length; k++)
				{
					this.anims1Rocket[k] = items[k].Name;
				}
			}
			if (this.skeletonAnim2Gun)
			{
				items = this.skeletonAnim2Gun.skeletonDataAsset.GetAnimationStateData().SkeletonData.Animations.Items;
				this.anims2Gun = new string[items.Length];
				for (int l = 0; l < items.Length; l++)
				{
					this.anims2Gun[l] = items[l].Name;
				}
			}
			if (this.skeletonAnim1Gun)
			{
				items = this.skeletonAnim1Gun.skeletonDataAsset.GetAnimationStateData().SkeletonData.Animations.Items;
				this.anims1Gun = new string[items.Length];
				for (int m = 0; m < items.Length; m++)
				{
					this.anims1Gun[m] = items[m].Name;
				}
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(ex.Message);
		}
	}

	private void OnDisable()
	{
		try
		{
			this.isInit = false;
			EnemyManager.Instance.EnemyThuyenPool.Store(this);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(ex.Message);
		}
	}

	public void InitEnemy(EnemyDataInfo dataInfo)
	{
		base.gameObject.SetActive(true);
		this.cacheEnemyData = dataInfo;
		this.transform.position = this.cacheEnemyData.Vt2;
		string text = (!SplashScreen._isLoadResourceDecrypt) ? this.Data_Encrypt.text : this.Data_Decrypt.text;
		string text2 = ProfileManager.DataEncryption.decrypt2(text);
		EnemyCharactor enemyCharactor = JsonConvert.DeserializeObject<EnemyCharactor>((!SplashScreen._isLoadResourceDecrypt) ? text2 : text);
		this.cacheEnemy = enemyCharactor.enemy[this.cacheEnemyData.level];
		this.cacheEnemy.HP *= GameMode.Instance.GetMode();
		this._isAttack = false;
		this._coolDownCrazy = this.coolDownCrazy;
		this.ResetCoolDownAttack();
		this._stayTarget = !this.cacheEnemyData.ismove;
		if (this.cacheEnemyData.type != 13)
		{
			this.Init3Rocket(this.cacheEnemy.HP);
		}
		if (!this._stayTarget)
		{
			this._targetX = CameraController.Instance.transform.position.x;
			this.Flip(this._targetX < this.transform.position.x);
			this.PlayAnimThuyen(EnemyThuyen.EStateThuyen.Walk2, 0, true);
			this.PlayAnim1Gun(EnemyThuyen.EState1Gun.Walk2, 0, true);
			this.PlayAnim1Rocket(EnemyThuyen.EState1Rocket.Walk2, 0, true);
		}
		else
		{
			this.PlayAnimThuyen(EnemyThuyen.EStateThuyen.Walk1, 0, true);
			this.PlayAnim1Gun(EnemyThuyen.EState1Gun.Walk1, 0, true);
			this.PlayAnim1Rocket(EnemyThuyen.EState1Rocket.Walk1, 0, true);
		}
		this.isInit = true;
	}

	private void Init3Rocket(float hp)
	{
		this.box3Rocket.InitEnemy(hp);
		this.box3Rocket.OnHit = new Action<float, EWeapon>(this.OnHit3Rocket);
		this.box3Rocket.bodyCollider2D.enabled = true;
		this.box3Rocket.isInCamera = true;
		GameManager.Instance.ListEnemy.Add(this.box3Rocket);
		this.skeletonAnim3Rocket.state.Event += this.OnEvent3Rocket;
		this.skeletonAnim3Rocket.state.Complete += this.OnComplete3Rocket;
		if (this._stayTarget)
		{
			this.PlayAnim3Rocket(EnemyThuyen.EState3Rocket.Walk1, 0, false);
		}
		else
		{
			this.PlayAnim3Rocket(EnemyThuyen.EState3Rocket.Walk2, 0, false);
		}
		this._isInit3Rocket = true;
	}

	private void Init1Rocket(float hp)
	{
		this.box1Rocket.InitEnemy(hp);
		this.box1Rocket.bodyCollider2D.enabled = false;
		GameManager.Instance.ListEnemy.Add(this.box1Rocket);
		this.skeletonAnim1Rocket.state.Event += this.OnEvent1Rocket;
		this.skeletonAnim1Rocket.state.Complete += this.OnComplete1Rocket;
		this.box1Rocket.isInCamera = false;
		this._isInit1Rocket = true;
	}

	private void Init1Gun(float hp)
	{
		this.box1Gun.InitEnemy(hp);
		this.box1Gun.bodyCollider2D.enabled = false;
		GameManager.Instance.ListEnemy.Add(this.box1Gun);
		this.skeletonAnim1Gun.state.Event += this.OnEvent1Gun;
		this.skeletonAnim1Gun.state.Complete += this.OnComplete1Gun;
		this.box1Gun.isInCamera = false;
		this._isInit1Gun = true;
	}

	private void Init2Gun(float hp)
	{
		this.box2Gun.InitEnemy(hp);
		this.box2Gun.bodyCollider2D.enabled = true;
		GameManager.Instance.ListEnemy.Add(this.box2Gun);
		this.skeletonAnim2Gun.state.Event += this.OnEvent2Gun;
		this.skeletonAnim2Gun.state.Complete += this.OnComplete2Gun;
		this._isInit2Gun = true;
	}

	public void UpdateEnemy(float deltaTime)
	{
		if (!this.isInit)
		{
			return;
		}
		if (GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
		if (this._coolDownAttack > 0f)
		{
			this._coolDownAttack -= deltaTime;
		}
		if (base.isInCamera && this._stayTarget && this._coolDownAttack <= 0f && !this._isAttack && !GameManager.Instance.player.IsInVisible())
		{
			this._isAttack = true;
			if (this.cacheEnemyData.type == 12)
			{
				this.PlayAnim3Rocket(EnemyThuyen.EState3Rocket.Attack1, 0, false);
			}
		}
		if (!this._stayTarget)
		{
			this._pos = this.transform.position;
			this._pos.x = Mathf.MoveTowards(this._pos.x, this._targetX, deltaTime * this.cacheEnemy.Speed);
			this.transform.position = this._pos;
			if (this._pos.x == this._targetX || Mathf.Abs(this._pos.x - CameraController.Instance.transform.position.x) < 0.1f)
			{
				this._stayTarget = true;
				this._coolDownAttack = 0f;
				this.PlayAnimThuyen(EnemyThuyen.EStateThuyen.Walk1, 0, true);
			}
		}
		if (!base.isInCamera)
		{
			return;
		}
		if (this._coolDownCrazy > 0f)
		{
			this._coolDownCrazy -= deltaTime;
		}
	}

	public void PauseEnemy(bool paused)
	{
		if (paused)
		{
			this.skeletonAnimThuyen.timeScale = 0f;
			this.skeletonAnim1Gun.timeScale = 0f;
			this.skeletonAnim1Rocket.timeScale = 0f;
			this.skeletonAnim2Gun.timeScale = 0f;
			this.skeletonAnim3Rocket.timeScale = 0f;
		}
		else
		{
			this.skeletonAnimThuyen.timeScale = 1f;
			this.skeletonAnim1Gun.timeScale = 1f;
			this.skeletonAnim1Rocket.timeScale = 1f;
			this.skeletonAnim2Gun.timeScale = 1f;
			this.skeletonAnim3Rocket.timeScale = 1f;
		}
	}

	private void Flip(bool flip)
	{
		this._scale = this.transform.localScale;
		this._scale.x = ((!flip) ? Mathf.Abs(this._scale.x) : (-Mathf.Abs(this._scale.x)));
	}

	private void ResetCoolDownAttack()
	{
		this._coolDownAttack = this.cacheEnemy.Time_Reload_Attack;
		if (this._coolDownCrazy <= 0f)
		{
			this._coolDownAttack *= 0.7f;
		}
	}

	private void PlayAnimThuyen(EnemyThuyen.EStateThuyen state, int track = 0, bool loop = true)
	{
		this.skeletonAnimThuyen.state.SetAnimation(track, this.animsThuyen[(int)state], loop);
	}

	private void PlayAnim3Rocket(EnemyThuyen.EState3Rocket state, int track = 0, bool loop = false)
	{
		this.skeletonAnim3Rocket.state.SetAnimation(track, this.anims3Rocket[(int)state], loop);
	}

	private void PlayAnim1Rocket(EnemyThuyen.EState1Rocket state, int track = 0, bool loop = false)
	{
		this.skeletonAnim1Rocket.state.SetAnimation(track, this.anims1Rocket[(int)state], loop);
	}

	private void PlayAnim1Gun(EnemyThuyen.EState1Gun state, int track = 0, bool loop = false)
	{
		this.skeletonAnim1Gun.state.SetAnimation(track, this.anims1Gun[(int)state], loop);
	}

	private void PlayAnim2Gun(EnemyThuyen.EState2Gun state, int track = 0, bool loop = false)
	{
		this.skeletonAnim2Gun.state.SetAnimation(track, this.anims2Gun[(int)state], loop);
	}

	private void Create3Rocket(int id)
	{
		GameManager.Instance.bulletManager.CreateRocketThuyen(this.GetDamage(EnemyThuyen.EGun.Rocket3), this.cacheEnemy.Speed, this.trans3RocketPoint[id].position, this.trans3RocketPoint[id].position - this.trans3RocketPoint[id + 3].position);
	}

	private void OnHit3Rocket(float damage, EWeapon lastWeapon)
	{
		this.lastWeapon = lastWeapon;
		if (this.box3Rocket.HP <= 0f)
		{
			this._isInit3Rocket = false;
			this.PlayAnim3Rocket(EnemyThuyen.EState3Rocket.Die1, 0, false);
			GameManager.Instance.ListEnemy.Remove(this.box3Rocket);
			this.CheckDie();
		}
		else
		{
			this.PlayAnim3Rocket(EnemyThuyen.EState3Rocket.Hit, 1, false);
		}
	}

	private void OnHit1Rocket(float damage, EWeapon lastWeapon)
	{
		this.lastWeapon = lastWeapon;
		if (this.box1Rocket.HP <= 0f)
		{
			this._isInit1Rocket = false;
			this.PlayAnim1Rocket(EnemyThuyen.EState1Rocket.Die1, 0, false);
			GameManager.Instance.ListEnemy.Remove(this.box1Rocket);
			this.CheckDie();
		}
		else
		{
			this.PlayAnim1Rocket(EnemyThuyen.EState1Rocket.Hit, 0, false);
		}
	}

	private void OnHit1Gun(float damage, EWeapon lastWeapon)
	{
		this.lastWeapon = lastWeapon;
		if (this.box1Gun.HP <= 0f)
		{
			this._isInit1Gun = false;
			this.PlayAnim1Gun(EnemyThuyen.EState1Gun.Die1, 0, false);
			GameManager.Instance.ListEnemy.Remove(this.box1Gun);
			this.CheckDie();
		}
		else
		{
			this.PlayAnim1Gun(EnemyThuyen.EState1Gun.Hit, 1, false);
		}
	}

	private void OnHit2Gun(float damage, EWeapon lastWeapon)
	{
		this.lastWeapon = lastWeapon;
		if (this.box2Gun.HP <= 0f)
		{
			this._isInit2Gun = false;
			this.PlayAnim2Gun(EnemyThuyen.EState2Gun.Die1, 0, false);
			GameManager.Instance.ListEnemy.Remove(this.box2Gun);
			this.CheckDie();
		}
		else
		{
			this.PlayAnim2Gun(EnemyThuyen.EState2Gun.Hit, 1, false);
		}
	}

	private float GetDamage(EnemyThuyen.EGun gun)
	{
		return this.cacheEnemy.Damage * this.ratioDamage[(int)gun];
	}

	private void CheckDie()
	{
		if (this._isInit3Rocket || this._isInit1Rocket || this._isInit2Gun || this._isInit1Gun)
		{
			return;
		}
		base.StartCoroutine(this.Die());
	}

	private IEnumerator Die()
	{
		base.CalculatorToDie(true, false);
		Vector3 pos = this.transform.position;
		GameManager.Instance.fxManager.ShowFxNoSpine01(1, pos, Vector3.one);
		yield return this.waitExplosion;
		pos = this.transform.position + Vector3.right * 2f;
		GameManager.Instance.fxManager.ShowFxNoSpine01(1, pos, Vector3.one);
		yield return this.waitExplosion;
		pos = this.transform.position + Vector3.left * 2f;
		GameManager.Instance.fxManager.ShowFxNoSpine01(1, pos, Vector3.one);
		yield return this.waitExplosion;
		this.DisableObject();
		yield break;
	}

	private void DisableObject()
	{
		this.isInit = false;
		base.gameObject.SetActive(false);
		this.skeletonAnim3Rocket.state.Complete -= this.OnComplete3Rocket;
		this.skeletonAnim3Rocket.state.Event -= this.OnEvent3Rocket;
	}

	private void OnEvent3Rocket(TrackEntry trackEntry, Spine.Event e)
	{
		if (trackEntry != null)
		{
			string name = e.Data.Name;
			if (name != null)
			{
				if (!(name == "dan-1"))
				{
					if (!(name == "dan-2"))
					{
						if (name == "dan-3")
						{
							this.Create3Rocket(2);
						}
					}
					else
					{
						this.Create3Rocket(1);
					}
				}
				else
				{
					this.Create3Rocket(0);
				}
			}
		}
	}

	private void OnComplete3Rocket(TrackEntry trackEntry)
	{
		if (trackEntry != null)
		{
			string text = trackEntry.ToString();
			if (text != null)
			{
				if (!(text == "attack4-1"))
				{
					if (!(text == "attack4-2"))
					{
						if (text == "die")
						{
							this.PlayAnim3Rocket(EnemyThuyen.EState3Rocket.Die2, 0, false);
						}
					}
					else
					{
						this.ResetCoolDownAttack();
						this._isAttack = false;
						this.PlayAnim3Rocket(EnemyThuyen.EState3Rocket.Walk1, 0, true);
					}
				}
				else
				{
					this.PlayAnim3Rocket(EnemyThuyen.EState3Rocket.Attack2, 0, false);
				}
			}
		}
	}

	private void OnEvent1Rocket(TrackEntry trackEntry, Spine.Event e)
	{
	}

	private void OnComplete1Rocket(TrackEntry trackEntry)
	{
	}

	private void OnEvent1Gun(TrackEntry trackEntry, Spine.Event e)
	{
	}

	private void OnComplete1Gun(TrackEntry trackEntry)
	{
	}

	private void OnEvent2Gun(TrackEntry trackEntry, Spine.Event e)
	{
	}

	private void OnComplete2Gun(TrackEntry trackEntry)
	{
	}

	[SerializeField]
	protected TextAsset Data_Encrypt;

	[SerializeField]
	protected TextAsset Data_Decrypt;

	[SerializeField]
	[Header("Spine:")]
	private SkeletonAnimation skeletonAnimThuyen;

	[SpineAnimation("", "", true, false)]
	[SerializeField]
	private string[] animsThuyen;

	[SerializeField]
	private SkeletonAnimation skeletonAnim3Rocket;

	[SerializeField]
	[SpineAnimation("", "", true, false)]
	private string[] anims3Rocket;

	[SerializeField]
	private SkeletonAnimation skeletonAnim1Rocket;

	[SerializeField]
	[SpineAnimation("", "", true, false)]
	private string[] anims1Rocket;

	[SerializeField]
	private SkeletonAnimation skeletonAnim2Gun;

	[SerializeField]
	[SpineAnimation("", "", true, false)]
	private string[] anims2Gun;

	[SerializeField]
	private SkeletonAnimation skeletonAnim1Gun;

	[SerializeField]
	[SpineAnimation("", "", true, false)]
	private string[] anims1Gun;

	[SerializeField]
	[Header("-------------------------------")]
	private BoxBoss1_3 box3Rocket;

	[SerializeField]
	private BoxBoss1_3 box1Rocket;

	[SerializeField]
	private BoxBoss1_3 box2Gun;

	[SerializeField]
	private BoxBoss1_3 box1Gun;

	[SerializeField]
	private Transform[] trans3RocketPoint;

	[SerializeField]
	private float coolDownCrazy;

	[Header("Tỉ lệ damage: súng2-súng1-rocket3-rocket1")]
	[SerializeField]
	private float[] ratioDamage = new float[]
	{
		1f,
		3.5f,
		1.5f,
		5f
	};

	private float _coolDownAttack;

	private float _coolDownCrazy;

	private bool _isInit3Rocket;

	private bool _isInit1Rocket;

	private bool _isInit2Gun;

	private bool _isInit1Gun;

	private Vector3 _scale;

	private WaitForSeconds waitExplosion = new WaitForSeconds(0.25f);

	private bool _stayTarget;

	private Vector3 _pos;

	private float _targetX;

	private bool _isAttack;

	private enum EStateThuyen
	{
		Attack,
		Walk1,
		Walk2
	}

	private enum EState3Rocket
	{
		Attack1,
		Attack2,
		Die1,
		Die2,
		Hit,
		Walk1,
		Walk2
	}

	private enum EState1Rocket
	{
		Attack1,
		Attack2,
		Attack3,
		Attack4,
		Die1,
		Die2,
		Hit,
		Walk1,
		Walk2
	}

	private enum EState1Gun
	{
		Attack1,
		Attack2,
		Attack3,
		Attack4,
		Die1,
		Die2,
		Hit,
		Walk1,
		Walk2
	}

	private enum EState2Gun
	{
		Aim,
		Attack,
		Die1,
		Die2,
		Hit,
		Walk1,
		Walk2
	}

	private enum EGun
	{
		Gun2,
		Gun1,
		Rocket3,
		Rocket1
	}
}
