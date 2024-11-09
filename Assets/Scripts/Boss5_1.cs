using System;
using System.Collections;
using Com.LuisPedroFonseca.ProCamera2D;
using MyDataLoader;
using Newtonsoft.Json;
using PVPManager;
using Spine;
using Spine.Unity;
using UnityEngine;

public class Boss5_1 : BaseBoss
{
	private void PingPongColorRambo()
	{
		if (this.hitRambo)
		{
			this.timePingPongColorRambo += Time.deltaTime;
			if (this.timePingPongColorRambo >= 0.3f)
			{
				GameManager.Instance.player.skeletonAnimation.skeleton.SetColor(Color.white);
				this.hitRambo = false;
				this.timePingPongColorRambo = 0f;
				return;
			}
			Color color = Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time * 10f, 1f));
			GameManager.Instance.player.skeletonAnimation.skeleton.SetColor(color);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.transform.CompareTag("Ground"))
		{
			this._isGround = true;
			this.posBegin = new Vector2(CameraController.Instance.Position.x + CameraController.Instance.Size().x - 2f, this.transform.position.y);
		}
	}

	private void Update()
	{
		if (!this.isInit || this.State == ECharactor.DIE || GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
		if (!this._isGround)
		{
			return;
		}
		if (!this.Begin())
		{
			return;
		}
		this.UpdateState();
	}

	private void HandleEvent(Spine.AnimationState state, int trackIndex, Spine.Event e)
	{
		if (state.GetCurrent(trackIndex) == null)
		{
			return;
		}
	}

	private void InitModeEnemy()
	{
	}

	public override void Init()
	{
		base.Init();
		this.InitEnemy();
		try
		{
			ProCamera2D.Instance.OffsetX = 0f;
		}
		catch (Exception ex)
		{
		}
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
			else if (GameMode.Instance.modePlay == GameMode.ModePlay.CoOpMode)
			{
				num = 1f;
			}
			enemyCharactor.enemy[0].Damage *= num;
			this.InitEnemy(enemyCharactor, 0);
			this.cacheEnemy.HP = this.HP;
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.Log(ex.Message);
		}
		this.State = ECharactor.ATTACK;
		this._ramboTransform = GameManager.Instance.player.transform;
		this._attackCount = 1;
		this._changeAnim = false;
		this._rageState = false;
		this.effJumpAnimator.transform.parent = null;
		this.skeletonAnimation.state.SetAnimation(1, this.walkAnim, true);
		SkillBoss5_1 skillBoss5_ = this.lazerBoss;
		skillBoss5_.AttackDamage = (Action<IHealth>)Delegate.Combine(skillBoss5_.AttackDamage, new Action<IHealth>(this.OnAttack));
		SkillBoss5_1 skillBoss5_2 = this.cutBoss;
		skillBoss5_2.AttackDamage = (Action<IHealth>)Delegate.Combine(skillBoss5_2.AttackDamage, new Action<IHealth>(this.OnAttack));
		SkillBoss5_1 skillBoss5_3 = this.boxingBoss;
		skillBoss5_3.AttackDamage = (Action<IHealth>)Delegate.Combine(skillBoss5_3.AttackDamage, new Action<IHealth>(this.OnAttack));
		SkillBoss5_1 skillBoss5_4 = this.trampBoss;
		skillBoss5_4.AttackDamage = (Action<IHealth>)Delegate.Combine(skillBoss5_4.AttackDamage, new Action<IHealth>(this.OnAttack));
		this.skeletonAnimation.skeleton.SetColor(new Color(1f, 1f, 1f, 1f));
		this.skeletonAnimation.state.Complete += this.HandleComplete;
		this.transform.rotation = Quaternion.identity;
		this.rigidbody2D.gravityScale = 3f;
		this._isBegin = false;
		this.isInit = true;
	}

	private void HandleComplete(TrackEntry entry)
	{
		if (entry == null)
		{
			return;
		}
		string text = entry.ToString();
		if (text != null)
		{
			if (text == "Death")
			{
				if (GameMode.Instance.Style == GameMode.GameStyle.MultiPlayer && GameMode.Instance.modePlay == GameMode.ModePlay.CoOpMode)
				{
					CoOpManager.Instance.EndOfGame(true);
				}
			}
		}
	}

	private bool Begin()
	{
		if (this._isBegin)
		{
			return true;
		}
		this.transform.position = Vector2.MoveTowards(this.transform.position, this.posBegin, Time.deltaTime * this.cacheEnemy.Speed);
		float num = Vector2.Distance(this.transform.position, this.posBegin);
		this.skeletonAnimation.skeleton.FlipX = false;
		if (num <= 0.1f)
		{
			CameraController.Instance.Shake(CameraController.ShakeType.Explosion);
			this.rigidbody2D.gravityScale = 1f;
			this._isBegin = true;
			GameManager.Instance.ListEnemy.Add(this);
		}
		return false;
	}

	private void OnAttack(IHealth hpScript)
	{
		float num = 0f;
		ECharactor state = this.State;
		switch (state)
		{
		case ECharactor.ATTACK:
			num = this.GetDamage(Boss5_1.Skill.Laser);
			break;
		case ECharactor.ATTACK_2:
			num = this.GetDamage(Boss5_1.Skill.Cut);
			this.PlayAudio(this.audioChemTrung);
			break;
		case ECharactor.ATTACK_3:
			GameManager.Instance.fxManager.ShowFxNoSpine01(1, this.boxingBoss.transform.position, Vector3.one);
			num = this.GetDamage(Boss5_1.Skill.Boxing);
			this.boxingBoss.rigid.velocity = Vector2.zero;
			this.boxingBoss.gameObject.SetActive(false);
			this.boxingBoss.transform.localPosition = this._boxingLocalPos;
			this.ChangeState();
			break;
		default:
			if (state == ECharactor.JUMP)
			{
				num = this.GetDamage(Boss5_1.Skill.Tramp);
			}
			break;
		}
		this.PlayAudio(this.audioBanTrung);
		this.hitRambo = true;
		hpScript.AddHealthPoint(-num, EWeapon.ROCKET);
	}

	private void UpdateState()
	{
		if (this.State == ECharactor.DIE)
		{
			return;
		}
		base.PingPongColor();
		this.PingPongColorRambo();
		switch (this.State)
		{
		case ECharactor.ATTACK:
			if (!this._changeAnim)
			{
				this.SetAnimation(this.lazerAnim, false);
				base.StartCoroutine(this.OnFireLazer());
				this.PlayAudio(this.audioBanLaser);
			}
			break;
		case ECharactor.ATTACK_2:
			if (!this._changeAnim)
			{
				this.SetAnimation(this.cutAnim, false);
				base.StartCoroutine(this.OnCut());
				this.PlayAudio(this.audioChemTruot);
			}
			break;
		case ECharactor.ATTACK_3:
			if (!this._changeAnim)
			{
				this.SetAnimation(this.boxingAnim, false);
				this._timer = 0f;
				this._stateBoxing = 0;
				try
				{
					this._targetMove = this._ramboTransform.position;
				}
				catch
				{
				}
			}
			this.OnBoxing();
			break;
		case ECharactor.RUN:
			if (!this._changeAnim)
			{
				this.SetAnimation(this.walkAnim, true);
				this.PlayAudio(this.audioDiChuyen);
			}
			try
			{
				if (this._distanceRamboX > 0f)
				{
					this._targetMove = new Vector3(this._ramboTransform.position.x + 2f, this.transform.position.y, this.transform.position.z);
				}
				else
				{
					this._targetMove = new Vector3(this._ramboTransform.position.x - 2f, this.transform.position.y, this.transform.position.z);
				}
			}
			catch
			{
			}
			this.OnMove(this.cacheEnemy.Speed);
			break;
		case ECharactor.RUN_BACK:
			if (!this._changeAnim)
			{
				this.effWalkBack.gameObject.SetActive(true);
				this.PlayAudio(this.audioBayLui);
				this.SetAnimation(this.runBackAnim, true);
			}
			if (this._distanceRamboX > 0f)
			{
				this._targetMove = new Vector3((this.transform.position.x >= CameraController.Instance.Position.x + CameraController.Instance.Size().x - 4f) ? (CameraController.Instance.Position.x + CameraController.Instance.Size().x - 2f) : (this.transform.position.x + 2f), this.transform.position.y, this.transform.position.z);
			}
			else
			{
				this._targetMove = new Vector3((this.transform.position.x <= CameraController.Instance.Position.x - CameraController.Instance.Size().x + 4f) ? (CameraController.Instance.Position.x - CameraController.Instance.Size().x + 2f) : (this.transform.position.x - 2f), this.transform.position.y, this.transform.position.z);
			}
			this.OnMove(this.cacheEnemy.Speed * 1.5f);
			break;
		case ECharactor.IDLE:
			if (!this._changeAnim)
			{
				this.SetAnimation(this.idleAnim, true);
				base.StartCoroutine(this.OnIdle());
			}
			break;
		case ECharactor.JUMP:
			if (!this._changeAnim)
			{
				this.effJumpAnimator.transform.position = this.transform.position;
				this.effJumpAnimator.Play("EffJumpR");
				this.SetAnimation(this.jumpAnim, true);
				try
				{
					this._targetDown = new Vector3(this._ramboTransform.position.x, this.transform.position.y, 0f) + ((!this.skeletonAnimation.skeleton.FlipX) ? (Vector3.right / 4f) : (Vector3.left / 4f));
				}
				catch
				{
				}
				this._targetMove = this._targetDown + Vector3.up * 6f;
				this._timer = 0f;
				this._changeAnimDown = false;
				this._timeStartDown = Vector3.Distance(this._targetMove, this.transform.position) / this.GetSpeed(Boss5_1.Skill.Tramp);
				this.PlayAudio(this.audioNhayLen);
			}
			this.OnTramp();
			break;
		}
	}

	private void PlayAudio(AudioSource audio)
	{
		if (ProfileManager.settingProfile.IsSound)
		{
			audio.Play();
		}
	}

	private void StopAudio(AudioSource audio)
	{
		if (ProfileManager.settingProfile.IsSound)
		{
			audio.Stop();
		}
	}

	private IEnumerator OnFireLazer()
	{
		if (this.lazerBoss == null)
		{
			Log.Error("null");
			yield return null;
		}
		yield return new WaitForSeconds(0.3f);
		this.lazerBoss.gameObject.SetActive(true);
		yield return new WaitForSeconds(0.3f);
		this.lazerBoss.gameObject.SetActive(false);
		this.ChangeState();
		yield break;
	}

	private IEnumerator OnCut()
	{
		if (this.cutBoss == null)
		{
			Log.Error("null");
			yield return null;
		}
		yield return new WaitForSeconds(0.3f);
		this.cutBoss.gameObject.SetActive(true);
		yield return new WaitForSeconds(0.3f);
		this.cutBoss.gameObject.SetActive(false);
		this.ChangeState();
		yield break;
	}

	private IEnumerator OnIdle()
	{
		yield return new WaitForSeconds(1.67f);
		this.ChangeState();
		yield break;
	}

	private void OnBoxing()
	{
		if (this.boxingBoss == null)
		{
			Log.Error("null");
			return;
		}
		if (this.boxingBoss.rigid == null)
		{
			Log.Error("null");
			return;
		}
		this.OnUpdateTimer();
		if (this._stateBoxing == 0 && this._timer > 0.3f)
		{
			this._stateBoxing++;
			this.boxingBoss.gameObject.SetActive(true);
			this.boxingBoss.transform.rotation = Quaternion.identity;
			this.boxingBoss.rigid.gravityScale = 1f;
			this.boxingBoss.rigid.velocity = new Vector2((float)((this._distanceRamboX <= 0f) ? 2 : -2), 4f);
			this.PlayAudio(this.audioTayDam);
		}
		if (this._stateBoxing == 1 && this._timer > 1f)
		{
			this._stateBoxing++;
			this.boxingBoss.rigid.gravityScale = 0f;
			this._deltaSpeedBoxing = Time.deltaTime;
			this._velocityBoxing = this._targetMove + Vector3.up - this.boxingBoss.transform.position;
			this._velocityBoxing.Normalize();
		}
		if (this._stateBoxing == 2)
		{
			this.boxingBoss.rigid.velocity = this._velocityBoxing * this.GetSpeed(Boss5_1.Skill.Boxing) * (this._deltaSpeedBoxing * this._deltaSpeedBoxing + 1f);
			this._deltaSpeedBoxing += Time.deltaTime * 3f;
			float num = this.boxingBoss.transform.position.x - CameraController.Instance.Position.x;
			if (num < -9f || num > 9f)
			{
				this.boxingBoss.rigid.velocity = Vector2.zero;
				this.boxingBoss.gameObject.SetActive(false);
				this.boxingBoss.transform.localPosition = this._boxingLocalPos;
				this.ChangeState();
			}
		}
	}

	private void OnTramp()
	{
		if (this.trampBoss == null)
		{
			Log.Error("null");
			return;
		}
		this.OnUpdateTimer();
		if (this.transform.position.y < CameraController.Instance.Position.y + 4f)
		{
			this.transform.position = Vector3.MoveTowards(this.transform.position, this._targetMove, this.GetSpeed(Boss5_1.Skill.Tramp) * Time.deltaTime);
		}
		if (this._timer > this._timeStartDown)
		{
			if (!this._changeAnimDown)
			{
				this._changeAnimDown = true;
				this.skeletonAnimation.state.SetAnimation(1, this.downAnim, true);
				this.trampBoss.gameObject.SetActive(true);
			}
			this.transform.position = Vector3.MoveTowards(this.transform.position, this._targetDown, this.GetSpeed(Boss5_1.Skill.Tramp) * Time.deltaTime * 4f);
			if (this.transform.position == this._targetDown)
			{
				this.PlayAudio(this.audioChamDat);
				CameraController.Instance.Shake(CameraController.ShakeType.Explosion);
				this.effJumpAnimator.transform.position = this.transform.position;
				this.effJumpAnimator.Play("EffJumpR");
				this.trampBoss.gameObject.SetActive(false);
				this.ChangeState();
			}
		}
	}

	private void OnMove(float speed)
	{
		int num = (!base.isInCamera) ? 3 : 1;
		this.transform.position = Vector3.MoveTowards(this.transform.position, this._targetMove, speed * Time.deltaTime * (float)num);
		if (this._rageState && this.State == ECharactor.RUN)
		{
			try
			{
				if (Mathf.Abs(this.transform.position.x - this._ramboTransform.position.x) < 1f)
				{
					this.ChangeState();
				}
			}
			catch
			{
			}
		}
		if (this.transform.position == this._targetMove)
		{
			if (this.State == ECharactor.RUN_BACK)
			{
				this.effWalkBack.gameObject.SetActive(false);
			}
			else
			{
				this.StopAudio(this.audioDiChuyen);
			}
			this.ChangeState();
		}
	}

	private void OnUpdateTimer()
	{
		this._timer += Time.deltaTime;
	}

	private void SetAnimation(string anim, bool loop = false)
	{
		this.skeletonAnimation.state.SetAnimation(1, anim, loop);
		this._changeAnim = true;
	}

	private void ChangeState()
	{
		try
		{
			this._distanceRamboX = this.transform.position.x - this._ramboTransform.position.x;
		}
		catch
		{
		}
		this._distanceCamX = this.transform.position.x - CameraController.Instance.Position.x;
		if (this._attackCount < this.maxAttack)
		{
			if (this._rageState)
			{
				if (this.State == ECharactor.ATTACK || this.State == ECharactor.ATTACK_2 || this.State == ECharactor.ATTACK_3 || this.State == ECharactor.JUMP)
				{
					if (5f < Mathf.Abs(this._distanceRamboX))
					{
						this.RandomState(new int[]
						{
							0,
							4,
							5,
							2,
							0,
							2,
							0,
							4,
							5,
							2,
							0,
							2,
							0,
							4,
							5,
							2,
							0,
							2
						});
					}
					else if (3f < Mathf.Abs(this._distanceRamboX))
					{
						if (4f > Mathf.Abs(this._distanceCamX))
						{
							this.RandomState(new int[]
							{
								0,
								3,
								2,
								4,
								5,
								6,
								1,
								3,
								2,
								0,
								1,
								0,
								3,
								2,
								4,
								5,
								6,
								1,
								3,
								2,
								0,
								1
							});
						}
						else
						{
							this.RandomState(new int[]
							{
								4,
								5,
								0,
								1,
								4,
								5,
								0,
								1,
								4,
								5,
								0,
								1,
								4,
								5,
								0,
								1
							});
						}
					}
					else if (4f > Mathf.Abs(this._distanceCamX))
					{
						this.RandomState(new int[]
						{
							3,
							1,
							3,
							1,
							3,
							4,
							6,
							1,
							3,
							4,
							6,
							1,
							1,
							3,
							1,
							3
						});
					}
					else
					{
						this.RandomState(new int[]
						{
							3,
							1,
							4,
							1,
							1,
							1,
							4,
							1,
							4,
							3,
							3,
							3,
							3,
							3
						});
					}
				}
				else if (5f < Mathf.Abs(this._distanceRamboX))
				{
					this.RandomState(new int[]
					{
						0,
						2,
						0,
						2,
						0,
						2,
						2,
						0,
						0,
						4,
						0,
						2,
						0,
						2,
						4,
						0,
						0,
						2,
						0,
						2,
						2
					});
				}
				else if (2.5f < Mathf.Abs(this._distanceRamboX))
				{
					this.RandomState(new int[]
					{
						0,
						1,
						2,
						3,
						1,
						0,
						2,
						3,
						4,
						0,
						1,
						2,
						3,
						1,
						0,
						2,
						3,
						0,
						1,
						2,
						3,
						1,
						0,
						2,
						3,
						4
					});
				}
				else
				{
					this.RandomState(new int[]
					{
						1,
						3,
						1,
						3,
						1,
						3,
						1,
						3
					});
				}
			}
			else if (this.State == ECharactor.ATTACK || this.State == ECharactor.ATTACK_2 || this.State == ECharactor.ATTACK_3 || this.State == ECharactor.JUMP)
			{
				if (5f < Mathf.Abs(this._distanceRamboX))
				{
					this.RandomState(new int[]
					{
						0,
						0,
						0,
						4,
						0,
						0,
						4,
						5,
						0,
						0,
						0,
						5,
						0
					});
				}
				else if (3f < Mathf.Abs(this._distanceRamboX))
				{
					if (4f > Mathf.Abs(this._distanceCamX))
					{
						this.RandomState(new int[]
						{
							0,
							1,
							1,
							0,
							0,
							1,
							0,
							4,
							5,
							6,
							1,
							0,
							4,
							5,
							6,
							1,
							0,
							1
						});
					}
					else
					{
						this.RandomState(new int[]
						{
							1,
							0,
							1,
							4,
							5,
							0,
							1,
							0,
							0,
							1,
							4,
							5,
							0,
							1
						});
					}
				}
				else if (4f > Mathf.Abs(this._distanceCamX))
				{
					this.RandomState(new int[]
					{
						1,
						1,
						4,
						1,
						1,
						1,
						1,
						6,
						6,
						1,
						1,
						1
					});
				}
				else
				{
					this.RandomState(new int[]
					{
						4,
						1,
						1,
						1,
						1,
						1,
						1,
						1,
						4,
						1,
						1,
						1
					});
				}
			}
			else if (4f < Mathf.Abs(this._distanceRamboX))
			{
				int[] array = new int[10];
				array[5] = 4;
				this.RandomState(array);
			}
			else if (2.5f < Mathf.Abs(this._distanceRamboX))
			{
				this.RandomState(new int[]
				{
					0,
					1,
					0,
					1,
					0,
					0,
					1,
					0,
					1,
					0,
					1,
					4,
					0,
					1,
					4,
					1,
					1,
					1
				});
			}
			else
			{
				this.RandomState(new int[]
				{
					1,
					1,
					1,
					1,
					1,
					1,
					4,
					1,
					1,
					1
				});
			}
		}
		else if (5f < Mathf.Abs(this._distanceRamboX))
		{
			this.RandomState(new int[]
			{
				5,
				5,
				4,
				5,
				4,
				5,
				4,
				5,
				4,
				5,
				4,
				5
			});
		}
		else if (3f < Mathf.Abs(this._distanceRamboX))
		{
			if (3f > Mathf.Abs(this._distanceCamX))
			{
				this.RandomState(new int[]
				{
					4,
					5,
					6,
					4,
					5,
					6,
					4,
					5,
					6,
					4,
					5,
					6,
					4,
					5,
					6
				});
			}
			else
			{
				this.RandomState(new int[]
				{
					4,
					5,
					4,
					5,
					4,
					5,
					4,
					5,
					4,
					5
				});
			}
		}
		else if (3f > Mathf.Abs(this._distanceCamX))
		{
			this.RandomState(new int[]
			{
				4,
				6,
				4,
				6,
				4,
				6,
				4,
				6,
				4,
				6
			});
		}
		else
		{
			this.RandomState(new int[]
			{
				4
			});
		}
	}

	public void ChangeState(int state)
	{
		try
		{
			this.skeletonAnimation.skeleton.FlipX = (this.transform.position.x <= this._ramboTransform.position.x);
		}
		catch
		{
		}
		this._changeAnim = false;
		switch (state)
		{
		case 0:
			this.State = ECharactor.ATTACK;
			break;
		case 1:
			this.State = ECharactor.ATTACK_2;
			break;
		case 2:
			this.State = ECharactor.ATTACK_3;
			break;
		case 3:
			this.State = ECharactor.JUMP;
			break;
		case 4:
			this.State = ECharactor.IDLE;
			break;
		case 5:
			this.State = ECharactor.RUN;
			break;
		case 6:
			this.State = ECharactor.RUN_BACK;
			break;
		}
		if (!base.isInCamera)
		{
			this.State = ECharactor.RUN;
		}
		this._attackCount = ((this.State != ECharactor.ATTACK && this.State != ECharactor.ATTACK_2 && this.State != ECharactor.ATTACK_3 && this.State != ECharactor.JUMP) ? 0 : (this._attackCount + 1));
		if (this.State == ECharactor.IDLE && this._idleCount == 1)
		{
			this.ChangeState();
			return;
		}
		this._idleCount = ((this.State != ECharactor.IDLE) ? 0 : 1);
	}

	private void RandomState(int[] arrayNextState)
	{
		if (!this.IsRemoteBoss)
		{
			int num = UnityEngine.Random.Range(0, arrayNextState.Length);
			if (this.syncBossHeavyMechState != null)
			{
				int randomRamboActorNumber = this.GetRandomRamboActorNumber();
				this.ChangeBossTarget(randomRamboActorNumber);
				this.syncBossHeavyMechState.SendRpcChanState(arrayNextState[num], randomRamboActorNumber);
			}
			this.ChangeState(arrayNextState[num]);
		}
	}

	private float GetDamage(Boss5_1.Skill skill)
	{
		return Mathf.Round(this.ratioDamages[(int)skill] * this.cacheEnemy.Damage);
	}

	private float GetSpeed(Boss5_1.Skill skill)
	{
		return Mathf.Round(this.ratioSpeeds[(int)skill] * this.cacheEnemy.Speed);
	}

	public override void Hit(float damage)
	{
		if (this.State == ECharactor.DIE || GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
		if (GameMode.Instance.modePlay == GameMode.ModePlay.Boss_Mode)
		{
			this.hit++;
			if (this.hit >= 10)
			{
				this.hit = 0;
				GameManager.Instance.giftManager.CreateItemWeapon(this.transform.position);
			}
		}
		if (!this._rageState && this.HP < this.cacheEnemy.HP / 2f)
		{
			this._rageState = true;
			this.cacheEnemy.Speed *= 1.3f;
			this.maxAttack++;
		}
		this.PlayAudio(this.audioBiBan);
		this.timePingPongColor = 0f;
		GameManager.Instance.bossManager.ShowLineBloodBoss(this.HP, this.cacheEnemy.HP);
		if (this.HP <= 0f && this.State != ECharactor.DIE)
		{
			DailyQuestManager.Instance.MissionBoss(4, 0, delegate(bool isCompleted, DailyQuestManager.InforQuest infor)
			{
				if (isCompleted)
				{
					UIShowInforManager.Instance.OnShowDailyQuest(infor.Desc, infor.item, infor.amount);
				}
			});
			if (this.syncBossHeavyMechState != null)
			{
				this.syncBossHeavyMechState.SendRpc_Die();
			}
			this.Die();
			GameManager.Instance.bossManager.ShowLineBloodBoss(0f, this.cacheEnemy.HP);
			GameManager.Instance.hudManager.HideControl();
			PlayerManagerStory.Instance.OnRunGameOver();
			GameManager.Instance.ListEnemy.Remove(this);
			if (GameMode.Instance.Style == GameMode.GameStyle.SinglPlayer && GameMode.Instance.modePlay == GameMode.ModePlay.Campaign && !ProfileManager.bossModeProfile.bossProfiles[14].CheckUnlock(GameMode.Mode.NORMAL))
			{
				ProfileManager.bossModeProfile.bossProfiles[14].SetUnlock(GameMode.Mode.NORMAL, true);
				UIShowInforManager.Instance.ShowUnlock("Heavy Mech has been unlocked in BossMode!");
			}
			base.StartCoroutine(this.EffectDie());
		}
	}

	public void Die()
	{
		if (this.State == ECharactor.DIE)
		{
			return;
		}
		this.State = ECharactor.DIE;
		this.skeletonAnimation.skeleton.SetColor(Color.white);
		this.skeletonAnimation.state.ClearTracks();
		this.skeletonAnimation.state.SetAnimation(2, this.deathAnim, false);
		if (this.lineBloodEnemy != null)
		{
			this.lineBloodEnemy.Hide();
		}
	}

	private IEnumerator EffectDie()
	{
		Vector3 pos = this.transform.position;
		Vector3 pos2 = pos;
		Vector3 pos3 = pos;
		pos2.x -= 0.62f;
		pos2.y += 1.82f;
		pos3.x -= 69f;
		pos3.y += 4.4f;
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, pos2, Vector3.one);
		yield return new WaitForSeconds(0.25f);
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, pos3, Vector3.one);
		yield return new WaitForSeconds(0.5f);
		pos = this.transform.position;
		pos2 = pos;
		pos3 = pos;
		pos2.x -= 0.62f;
		pos2.y += 1.82f;
		pos3.x -= 69f;
		pos3.y += 4.4f;
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, pos2, Vector3.one);
		yield return new WaitForSeconds(0.25f);
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, pos3, Vector3.one);
		yield return new WaitForSeconds(0.5f);
		pos = this.transform.position;
		pos2 = pos;
		pos3 = pos;
		pos2.x -= 0.62f;
		pos2.y += 1.82f;
		pos3.x -= 69f;
		pos3.y += 4.4f;
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, pos2, Vector3.one);
		yield return new WaitForSeconds(0.25f);
		GameManager.Instance.fxManager.ShowFxNoSpine01(0, pos3, Vector3.one);
		yield break;
	}

	public void ChangeBossTarget(int actorNumber)
	{
		try
		{
			this._ramboTransform = GameManager.Instance.ListRambo.Find((BaseCharactor rambo) => ((PlayerMain)rambo).actorNumber == actorNumber).transform;
		}
		catch
		{
		}
	}

	private int GetRandomRamboActorNumber()
	{
		PlayerMain playerMain;
		do
		{
			int index = UnityEngine.Random.Range(0, GameManager.Instance.ListRambo.Count);
			playerMain = (GameManager.Instance.ListRambo[index] as PlayerMain);
		}
		while (!(playerMain != null));
		return playerMain.actorNumber;
	}

	[Header("*******************************")]
	[SerializeField]
	private SkeletonAnimation skeletonAnimation;

	[SpineAnimation("", "", true, false)]
	public string walkAnim;

	[SpineAnimation("", "", true, false)]
	public string idleAnim;

	[SpineAnimation("", "", true, false)]
	public string lazerAnim;

	[SpineAnimation("", "", true, false)]
	public string cutAnim;

	[SpineAnimation("", "", true, false)]
	public string boxingAnim;

	[SpineAnimation("", "", true, false)]
	public string jumpAnim;

	[SpineAnimation("", "", true, false)]
	public string downAnim;

	[SpineAnimation("", "", true, false)]
	public string runBackAnim;

	[SpineAnimation("", "", true, false)]
	public string deathAnim;

	[SerializeField]
	private SkillBoss5_1 lazerBoss;

	[SerializeField]
	private SkillBoss5_1 cutBoss;

	[SerializeField]
	private SkillBoss5_1 boxingBoss;

	[SerializeField]
	private SkillBoss5_1 trampBoss;

	[SerializeField]
	private int maxAttack;

	[SerializeField]
	private float[] ratioDamages;

	[SerializeField]
	private float[] ratioSpeeds;

	[SerializeField]
	private float timeIdle;

	[SerializeField]
	private Animator effJumpAnimator;

	[SerializeField]
	private ParticleSystem effWalkBack;

	[SerializeField]
	private AudioSource audioBanLaser;

	[SerializeField]
	private AudioSource audioBanTrung;

	[SerializeField]
	private AudioSource audioBiBan;

	[SerializeField]
	private AudioSource audioChemTrung;

	[SerializeField]
	private AudioSource audioChemTruot;

	[SerializeField]
	private AudioSource audioNhayLen;

	[SerializeField]
	private AudioSource audioDiChuyen;

	[SerializeField]
	private AudioSource audioChamDat;

	[SerializeField]
	private AudioSource audioTayDam;

	[SerializeField]
	private AudioSource audioBayLui;

	public Transform _ramboTransform;

	private Vector3 _targetMove;

	private Vector3 _targetDown;

	private Vector3 _boxingLocalPos = new Vector3(0.2f, -0.045f, 0f);

	private Vector2 _velocityBoxing;

	private bool _rageState;

	private bool _isBegin;

	private bool _changeAnim;

	private bool _changeAnimDown;

	private float _timeStartDown;

	private float _distanceRamboX;

	private float _distanceCamX;

	private float _timer;

	private float _deltaSpeedBoxing;

	private int _attackCount;

	private int _idleCount;

	private int _stateBoxing;

	private float timePingPongColorRambo;

	private bool hitRambo;

	private Vector2 posBegin;

	private bool _isGround;

	public bool IsRemoteBoss;

	public SyncBossHeavyMechState syncBossHeavyMechState;

	private int hit;

	private enum Skill
	{
		Laser,
		Cut,
		Boxing,
		Tramp
	}
}
