using System;
using MyDataLoader;
using Newtonsoft.Json;
using Spine;
using Spine.Unity;
using UnityEngine;

public class NhenNho : BaseEnemy
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
		}
		catch (Exception ex)
		{
			MonoBehaviour.print(ex.Message);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Ground"))
		{
			Vector3 v = this.transform.position + Vector3.up;
			RaycastHit2D raycastHit2D = Physics2D.Raycast(v, Vector2.down, 5f, this.maskGround);
			if (raycastHit2D.collider)
			{
				this.transform.position = raycastHit2D.point;
			}
			NhenNho.EState state = this._state;
			if (state == NhenNho.EState.Attack2_4)
			{
				float num = Mathf.Abs(this._pos.x - GameManager.Instance.player.transform.position.x);
				if (num <= 0.75f && !GameManager.Instance.player.isInVisible)
				{
					GameManager.Instance.player.GetComponent<IHealth>().AddHealthPoint(-this.cacheEnemy.Damage, EWeapon.NONE);
				}
				this.ChangeState();
			}
		}
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
			if (this.OnHide != null)
			{
				this.OnHide(this);
			}
		}
		catch
		{
		}
	}

	public void InitEnemy(EnemyDataInfo info, Action<NhenNho> hide = null)
	{
		this.cacheEnemyData = info;
		if (this.cacheEnemyData != null)
		{
			this.ID = this.cacheEnemyData.type;
		}
		string text = (!SplashScreen._isLoadResourceDecrypt) ? this.Data_Encrypt.text : this.Data_Decrypt.text;
		string text2 = ProfileManager.DataEncryption.decrypt2(text);
		EnemyCharactor enemyCharactor = JsonConvert.DeserializeObject<EnemyCharactor>((!SplashScreen._isLoadResourceDecrypt) ? text2 : text);
		float hp = enemyCharactor.enemy[info.level].HP * GameMode.Instance.GetMode();
		float damage = enemyCharactor.enemy[info.level].Damage * GameMode.Instance.GetMode();
		this._pos = info.Vt2;
		if (Mathf.Abs(this._pos.x - CameraController.Instance.camPos.x) >= CameraController.Instance.Size().x)
		{
			float num = UnityEngine.Random.Range(3f, 6f);
			this._pos.x = ((this._pos.x <= CameraController.Instance.camPos.x) ? (CameraController.Instance.camPos.x - num) : (CameraController.Instance.camPos.x + num));
		}
		RaycastHit2D raycastHit2D = Physics2D.Raycast(this._pos, Vector2.down, 10f, this.maskGround);
		if (raycastHit2D.collider)
		{
			this._pos = raycastHit2D.point;
		}
		this.InitObject(damage, hp, enemyCharactor.enemy[info.level].Speed, this._pos, hide, false);
	}

	public void InitObject(float damage, float hp, float speed, Vector3 pos, Action<NhenNho> hide = null, bool useByBoss = true)
	{
		if (this.isFirst)
		{
			this.isFirst = false;
		}
		this.OnHide = hide;
		this._useByBoss = useByBoss;
		this.bodyCollider2D.enabled = false;
		base.gameObject.SetActive(true);
		if (this.cacheEnemy == null)
		{
			this.cacheEnemy = new Enemy();
		}
		this.HP = hp;
		this.cacheEnemy.HP = Mathf.Round(hp / GameMode.Instance.GetMode());
		this.cacheEnemy.Speed = speed;
		this.cacheEnemy.Damage = damage;
		if (this.lineBloodEnemy)
		{
			this.lineBloodEnemy.Reset();
		}
		this.transform.position = pos;
		this.skeletonAnimation.state.Event += this.OnEvent;
		this.skeletonAnimation.state.Complete += this.OnComplete;
		this.skeletonAnimation.timeScale = this.skeletonTimeScale;
		this.skeletonAnimation.state.ClearTracks();
		this._coolDownHide = 5f;
		this._state = NhenNho.EState.Born;
		this._changeState = false;
		this._isJump = false;
		this._timeJump = 0f;
		this._veloY = 0f;
		this.PlayAnim(false);
		this.isInit = true;
	}

	public void PauseObject(bool pause)
	{
		this.skeletonAnimation.timeScale = ((!pause) ? this.skeletonTimeScale : 0f);
	}

	public void UpdateObject(float deltaTime)
	{
		if (!this.isInit || GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
		if (!base.isInCamera && !this._useByBoss)
		{
			this._coolDownHide -= deltaTime;
			if (this._coolDownHide <= 0f)
			{
				this.Die();
			}
			return;
		}
		this._pos = this.transform.position;
		if (!this._changeState)
		{
			this.StartState();
		}
		else
		{
			this.UpdateState(deltaTime);
		}
	}

	private void StartState()
	{
		bool loop = false;
		switch (this._state)
		{
		case NhenNho.EState.Attack2_2:
			this._isJump = true;
			this._timeJump = 0f;
			break;
		case NhenNho.EState.Attack2_5:
			this._isJump = false;
			break;
		case NhenNho.EState.Run:
			loop = true;
			break;
		}
		this.PlayAnim(loop);
		this._changeState = true;
	}

	private void UpdateState(float deltaTime)
	{
		if (this._isJump)
		{
			this._pos = this.transform.position;
			this._pos.x = this._pos.x + this.cacheEnemy.Speed * this._momentX * deltaTime;
			this._veloY = 5f - 11f * this._timeJump * this._timeJump;
			this._pos.y = this._pos.y + this._veloY * deltaTime;
			this._timeJump += deltaTime;
			this.transform.position = this._pos;
		}
		switch (this._state)
		{
		case NhenNho.EState.Attack2_3:
			if (this._veloY <= 0f)
			{
				this.ChangeState();
			}
			break;
		case NhenNho.EState.Run:
		{
			this._pos.x = this._pos.x + this.cacheEnemy.Speed * this._momentX * deltaTime;
			this.transform.position = this._pos;
			float num = Mathf.Abs(this._pos.x - GameManager.Instance.player.transform.position.x);
			if (num <= this.cacheEnemy.Speed)
			{
				this.ChangeState();
			}
			break;
		}
		}
	}

	private void ChangeState()
	{
		this._changeState = false;
		switch (this._state)
		{
		case NhenNho.EState.Born:
			this.bodyCollider2D.enabled = true;
			this.CheckAttack();
			break;
		case NhenNho.EState.Attack2_1:
			this._state = NhenNho.EState.Attack2_2;
			break;
		case NhenNho.EState.Attack2_2:
			this._state = NhenNho.EState.Attack2_3;
			break;
		case NhenNho.EState.Attack2_3:
			this._state = NhenNho.EState.Attack2_4;
			break;
		case NhenNho.EState.Attack2_4:
			this._state = NhenNho.EState.Attack2_5;
			break;
		case NhenNho.EState.Attack2_5:
		case NhenNho.EState.Attack3_1:
		case NhenNho.EState.Attack3_2:
		case NhenNho.EState.Attack4:
			this._state = NhenNho.EState.Idle;
			break;
		case NhenNho.EState.Idle:
		case NhenNho.EState.Run:
			this.CheckAttack();
			break;
		}
	}

	private void CheckAttack()
	{
		float num = this.transform.position.x - GameManager.Instance.player.transform.position.x;
		float num2 = Mathf.Abs(num);
		if (num2 <= 2f)
		{
			if (!this.skeletonAnimation.skeleton.FlipX)
			{
				this._state = ((num >= 0f) ? NhenNho.EState.Attack3_2 : NhenNho.EState.Attack3_1);
			}
			else
			{
				this._state = ((num >= 0f) ? NhenNho.EState.Attack3_1 : NhenNho.EState.Attack3_2);
			}
		}
		else if (num2 <= this.cacheEnemy.Speed * 1.3f)
		{
			this._state = NhenNho.EState.Attack2_1;
			this._momentX = (float)((num <= 0f) ? 1 : -1);
			this._momentX += UnityEngine.Random.Range(-0.1f, 0.1f);
		}
		else
		{
			this._state = NhenNho.EState.Run;
			this.skeletonAnimation.skeleton.FlipX = (num > 0f);
			this._momentX = (float)((num <= 0f) ? 1 : -1);
		}
	}

	private void PlayAnim(bool loop = false)
	{
		this.skeletonAnimation.state.SetAnimation(0, this.anims[(int)this._state], loop);
	}

	public override void Hit(float damage)
	{
		base.Hit(damage);
		if (this.HP <= 0f)
		{
			this.skeletonAnimation.state.ClearTracks();
			try
			{
				GameManager.Instance.fxManager.ShowEffect(5, this.transform.position, Vector3.one, true, true);
				this.skeletonAnimation.state.Complete -= this.OnComplete;
				this.skeletonAnimation.state.Event -= this.OnEvent;
				if (!this._useByBoss)
				{
					base.CalculatorToDie(true, false);
				}
				this.Die();
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.Log(ex.Message);
			}
			return;
		}
		this.skeletonAnimation.state.SetAnimation(1, this.anims[0], false);
	}

	private void Die()
	{
		base.gameObject.SetActive(false);
	}

	private void OnEvent(TrackEntry trackEntry, Spine.Event e)
	{
		NhenNho.EState state = this._state;
		if (state != NhenNho.EState.Attack3_1)
		{
			if (state == NhenNho.EState.Attack3_2)
			{
				float num = this._pos.x - GameManager.Instance.player.transform.position.x;
				if (this.skeletonAnimation.skeleton.FlipX == num < 0f && Mathf.Abs(num) <= 1.5f)
				{
					ISkill component = GameManager.Instance.player.GetComponent<ISkill>();
					if (component != null && component.IsInVisible())
					{
						return;
					}
					GameManager.Instance.player.GetComponent<IHealth>().AddHealthPoint(-this.cacheEnemy.Damage, EWeapon.NONE);
				}
			}
		}
		else
		{
			float num2 = this._pos.x - GameManager.Instance.player.transform.position.x;
			if (this.skeletonAnimation.skeleton.FlipX == num2 > 0f && Mathf.Abs(num2) <= 1.5f)
			{
				ISkill component2 = GameManager.Instance.player.GetComponent<ISkill>();
				if (component2 != null && component2.IsInVisible())
				{
					return;
				}
				GameManager.Instance.player.GetComponent<IHealth>().AddHealthPoint(-this.cacheEnemy.Damage, EWeapon.NONE);
			}
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
		case NhenNho.EState.Born:
		case NhenNho.EState.Attack2_1:
		case NhenNho.EState.Attack2_2:
		case NhenNho.EState.Attack2_5:
		case NhenNho.EState.Attack3_1:
		case NhenNho.EState.Attack3_2:
		case NhenNho.EState.Attack4:
		case NhenNho.EState.Idle:
			this.ChangeState();
			break;
		}
	}

	private Action<NhenNho> OnHide;

	private bool _useByBoss;

	[SerializeField]
	protected TextAsset Data_Encrypt;

	[SerializeField]
	protected TextAsset Data_Decrypt;

	[SerializeField]
	private SkeletonAnimation skeletonAnimation;

	[SerializeField]
	private float skeletonTimeScale = 1.3f;

	[SpineAnimation("", "", true, false)]
	[SerializeField]
	private string[] anims;

	public LayerMask maskGround;

	private NhenNho.EState _state;

	private bool _isJump;

	private bool _changeState;

	private float _momentX;

	private Vector3 _pos;

	private float _timeJump;

	private float _veloY;

	private float _coolDownHide = 5f;

	private bool isFirst = true;

	private enum EState
	{
		Hit,
		Born,
		Attack2_1,
		Attack2_2,
		Attack2_3,
		Attack2_4,
		Attack2_5,
		Attack3_1,
		Attack3_2,
		Attack4,
		Idle,
		Run
	}
}
