using System;
using System.Collections;
using System.Collections.Generic;
using Com.LuisPedroFonseca.ProCamera2D;
using MyDataLoader;
using Newtonsoft.Json;
using Spine;
using Spine.Unity;
using UnityEngine;

public class Boss_MaraDevil : BaseBoss
{
	private void Start()
	{
	}

	public override void Init()
	{
		this.isInit = false;
		this.mainCamera = Camera.main;
		this.GetGameMode();
		this.GetRUNMaxCountByGameMode();
		base.Init();
		this.InitEnemy();
	}

	public void InitEnemy()
	{
		this.LoadBossData();
		this.InitSkeletonAnimation();
		this.InitAnimations();
		this.GetBone();
		this.InitBullets();
		float mode = GameMode.Instance.GetMode();
		this.bossData.enemy[0].Damage *= mode;
		this.InitEnemy(this.bossData, 0);
		this.InitBoxBoss();
		base.isInCamera = true;
		this.bossState = Boss_MaraDevil.EState.START_OPEN;
		this.ClearDoneFlag();
		this.UpdateBossState();
		this.ramboTransform = GameManager.Instance.player.transform;
		this.isInit = true;
		try
		{
			ProCamera2D.Instance.OffsetX = 0f;
		}
		catch (Exception ex)
		{
		}
	}

	private void GetGameMode()
	{
		this.gameMode = GameMode.Instance.MODE;
	}

	private void InitBullets()
	{
		if (this.listBulletBoss != null)
		{
			this.listBulletBoss[0].gameObject.transform.parent.parent = null;
			this.poolingBulletsBoss = new ObjectPooling<StraightBullet>(this.listBulletBoss.Count, null, null);
			for (int i = 0; i < this.listBulletBoss.Count; i++)
			{
				this.poolingBulletsBoss.Store(this.listBulletBoss[i]);
			}
		}
	}

	private void UpdateBullet(float deltaTime)
	{
		for (int i = 0; i < this.listBulletBoss.Count; i++)
		{
			if (this.listBulletBoss[i].isInit)
			{
				this.listBulletBoss[i].UpdateObject(deltaTime);
			}
		}
	}

	private IEnumerator CreatBullet(Vector3 startPos, Vector3 endPos)
	{
		this._bullet = null;
		this._bullet = this.poolingBulletsBoss.New();
		if (this._bullet == null || this._bullet.isInit)
		{
			this._bullet = UnityEngine.Object.Instantiate<StraightBullet>(this.listBulletBoss[0], this.transform.position, Quaternion.identity);
			this._bullet.gameObject.transform.parent = this.listBulletBoss[0].gameObject.transform.parent;
			this.listBulletBoss.Add(this._bullet);
		}
		Vector3 director = (endPos - startPos).normalized;
		this._bullet.InitObject(this.ATTACK_3DamagePercent * this.cacheEnemy.Damage, this.ATTACK_3ShootSpeedPercent * this.bossData.enemy[0].Speed, 0f, endPos, director, new Action<StraightBullet>(this.OnHideBullet));
		this.PlaySound(this.ATTACK_3Clip, this.ATTACK_3_Volume);
		yield return new WaitForSeconds(0.1f);
		yield break;
	}

	private void OnHideBullet(StraightBullet bullet)
	{
		this.poolingBulletsBoss.Store(bullet);
	}

	private void Update()
	{
		if (!this.isInit)
		{
			return;
		}
		this.AutoBoxBossControl();
		if (this.PauseCondition())
		{
			this.PauseGame();
			return;
		}
		this.ResumeGame();
		this.UpdateCachePosition();
		this.UpdateCameraCornerPosition();
		if (this.ChangeBossState())
		{
			this.changeState = true;
		}
		else
		{
			this.changeState = false;
		}
		if (this.changeState)
		{
			this.UpdateBossState();
			this.changeState = false;
		}
		if (this.bossState == Boss_MaraDevil.EState.RUN || this.bossState == Boss_MaraDevil.EState.FAST_RUN)
		{
			if (Vector2.Distance(this.transform.position, this.targetRun) < 2f)
			{
				this.transform.position = Vector3.MoveTowards(this.transform.position, this.targetRun, this.RUNSpeedPercent * Time.fixedDeltaTime * this.bossData.enemy[0].Speed);
			}
			else
			{
				this.transform.position = Vector3.MoveTowards(this.transform.position, this.targetRun, this.currentRUNSpeedPercent * Time.fixedDeltaTime * this.bossData.enemy[0].Speed);
			}
		}
		else if (this.bossState == Boss_MaraDevil.EState.DIE)
		{
			this.transform.position = Vector3.MoveTowards(this.transform.position, this.targetRun, Time.fixedDeltaTime * this.bossData.enemy[0].Speed);
		}
		else if (this.bossState == Boss_MaraDevil.EState.ATTACK_1)
		{
			this.targetRun = new Vector3(this.ramboPosition.x + UnityEngine.Random.Range(-2f, 2f), this.roadMinYPos + UnityEngine.Random.Range(-1f, 1f), this.transform.position.z);
			this.transform.position = Vector3.MoveTowards(this.transform.position, this.targetRun, this.ATTACK_1SpeedPercent * Time.fixedDeltaTime * this.bossData.enemy[0].Speed);
		}
		else if (this.bossState == Boss_MaraDevil.EState.ATTACK_2)
		{
			this.transform.position = Vector3.MoveTowards(this.transform.position, this.targetRun, this.ATTACK_2SpeedPercent * Time.fixedDeltaTime * this.bossData.enemy[0].Speed);
		}
		else if (this.bossState == Boss_MaraDevil.EState.ATTACK_3)
		{
			this.targetRun = new Vector3(this.ramboPosition.x + UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(this.roadMinYPos, this.topLeftCameraPos.y), this.transform.position.z);
			this.transform.position = Vector3.MoveTowards(this.transform.position, this.targetRun, this.ATTACK_3SpeedPercent * Time.fixedDeltaTime * this.bossData.enemy[0].Speed);
		}
		else if (this.bossState == Boss_MaraDevil.EState.COMBO_ATTACK_1)
		{
			this.transform.position = Vector3.MoveTowards(this.transform.position, this.targetRun, this.COMBO_ATTACK_1SpeedPercent * Time.fixedDeltaTime * this.bossData.enemy[0].Speed);
		}
		else if (this.bossState == Boss_MaraDevil.EState.COMBO_ATTACK_2)
		{
			if (this.COMBO_ATTACK_2_Step == 1 || this.COMBO_ATTACK_2_Step == 2)
			{
				this.transform.position = Vector3.MoveTowards(this.transform.position, this.targetRun, this.COMBO_ATTACK_2Step2SpeedPercent * Time.fixedDeltaTime * this.bossData.enemy[0].Speed);
			}
			else
			{
				this.transform.position = Vector3.MoveTowards(this.transform.position, this.targetRun, this.COMBO_ATTACK_2SpeedPercent * Time.fixedDeltaTime * this.bossData.enemy[0].Speed);
			}
		}
		else if (this.bossState == Boss_MaraDevil.EState.COMBO_ATTACK_3)
		{
			this.transform.position = Vector3.MoveTowards(this.transform.position, this.targetRun, this.COMBO_ATTACK_3SpeedPercent * Time.fixedDeltaTime * this.bossData.enemy[0].Speed);
		}
		else if (this.bossState == Boss_MaraDevil.EState.COMBO_ATTACK_1_2)
		{
			if (this.COMBO_ATTACK_1_2_Step == 3 || this.COMBO_ATTACK_1_2_Step == 4 || this.COMBO_ATTACK_1_2_Step == 5)
			{
				this.transform.position = Vector3.MoveTowards(this.transform.position, this.targetRun, this.COMBO_ATTACK_2Step2SpeedPercent * Time.fixedDeltaTime * this.bossData.enemy[0].Speed);
			}
			else
			{
				this.transform.position = Vector3.MoveTowards(this.transform.position, this.targetRun, this.COMBO_ATTACK_1SpeedPercent * Time.fixedDeltaTime * this.bossData.enemy[0].Speed);
			}
		}
		float z = Mathf.LerpAngle(this.transform.eulerAngles.z, this.rotationAngle, Time.fixedDeltaTime * this.bossData.enemy[0].Speed);
		this.transform.eulerAngles = new Vector3(0f, 0f, z);
		this.UpdateBullet(Time.fixedDeltaTime);
	}

	private void AutoBoxBossControl()
	{
		if (this.meshRenderer.isVisible)
		{
			if (!GameManager.Instance.ListEnemy.Contains(this.boxBoss[0]))
			{
				GameManager.Instance.ListEnemy.Add(this.boxBoss[0]);
			}
		}
		else
		{
			GameManager.Instance.ListEnemy.Remove(this.boxBoss[0]);
		}
	}

	private void GetBone()
	{
		this.leftHandBone = this.skeletonAnimation.skeleton.FindBone(this.leftHandBoneName);
		this.rightHandBone = this.skeletonAnimation.skeleton.FindBone(this.rightHandBoneName);
		this.leftShoulderBone = this.skeletonAnimation.skeleton.FindBone(this.leftShoulderBoneName);
		this.rightShoulderBone = this.skeletonAnimation.skeleton.FindBone(this.rightShoulderBoneName);
		this.listStartGunBone = new List<Bone>();
		this.listEndGunBone = new List<Bone>();
		for (int i = 0; i < this.listStartGunName.Count; i++)
		{
			this.listStartGunBone.Add(this.skeletonAnimation.skeleton.FindBone(this.listStartGunName[i]));
			this.listEndGunBone.Add(this.skeletonAnimation.skeleton.FindBone(this.listEndGunName[i]));
		}
	}

	private void UpdateCachePosition()
	{
		this.leftHandPosition = this.leftHandBone.GetWorldPosition(this.skeletonAnimation.transform);
		this.rightHandPosition = this.rightHandBone.GetWorldPosition(this.skeletonAnimation.transform);
		this.leftShoulderPosition = this.leftShoulderBone.GetWorldPosition(this.skeletonAnimation.transform);
		this.rightShoulderPosition = this.rightShoulderBone.GetWorldPosition(this.skeletonAnimation.transform);
		if (GameManager.Instance.player.IsInVisible())
		{
			this.ramboPosition = new Vector3(UnityEngine.Random.Range(this.topLeftCameraPos.x, this.topRightCameraPos.x), this.ramboTransform.position.y, this.ramboTransform.position.z);
		}
		else
		{
			this.ramboPosition = this.ramboTransform.position;
		}
		for (int i = 0; i < this.listStartGunName.Count; i++)
		{
			this.listStartGunBonePosition[i] = this.listStartGunBone[i].GetWorldPosition(this.skeletonAnimation.transform);
			this.listEndGunBonePosition[i] = this.listEndGunBone[i].GetWorldPosition(this.skeletonAnimation.transform);
		}
	}

	private void UpdateCameraCornerPosition()
	{
		this.bottomLeftCameraPos = this.mainCamera.ScreenToWorldPoint(new Vector3(0f, 0f, this.mainCamera.nearClipPlane));
		this.topLeftCameraPos = this.mainCamera.ScreenToWorldPoint(new Vector3(0f, (float)this.mainCamera.pixelHeight, this.mainCamera.nearClipPlane));
		this.bottomRightCameraPos = this.mainCamera.ScreenToWorldPoint(new Vector3((float)this.mainCamera.pixelWidth, 0f, this.mainCamera.nearClipPlane));
		this.topRightCameraPos = this.mainCamera.ScreenToWorldPoint(new Vector3((float)this.mainCamera.pixelWidth, (float)this.mainCamera.pixelHeight, this.mainCamera.nearClipPlane));
		this.bottomLeftCameraPos += new Vector3(0f, 2f, -this.bottomLeftCameraPos.z + this.transform.position.z);
		this.topLeftCameraPos += new Vector3(0f, -6f, -this.topLeftCameraPos.z + this.transform.position.z);
		this.bottomRightCameraPos += new Vector3(0f, 2f, -this.bottomRightCameraPos.z + this.transform.position.z);
		this.topRightCameraPos += new Vector3(0f, -6f, -this.topRightCameraPos.z + this.transform.position.z);
	}

	private void UpdateBossState()
	{
		if (this.State == ECharactor.DIE)
		{
			return;
		}
		switch (this.bossState)
		{
		case Boss_MaraDevil.EState.ATTACK_1:
			this.OnATTACK_1();
			break;
		case Boss_MaraDevil.EState.ATTACK_2:
			this.OnATTACK_2();
			break;
		case Boss_MaraDevil.EState.ATTACK_3:
			this.OnATTACK_3();
			break;
		case Boss_MaraDevil.EState.DIE:
			this.OnDIE();
			break;
		case Boss_MaraDevil.EState.HIT:
			this.OnHIT();
			break;
		case Boss_MaraDevil.EState.IDLE:
			this.OnIDLE();
			break;
		case Boss_MaraDevil.EState.RUN:
			this.currentRUNSpeedPercent = this.RUNSpeedPercent;
			this.OnRUN();
			break;
		case Boss_MaraDevil.EState.START:
			this.OnSTART();
			break;
		case Boss_MaraDevil.EState.START_OPEN:
			this.OnSTART_OPEN();
			break;
		case Boss_MaraDevil.EState.COMBO_ATTACK_1:
			this.OnCOMBO_ATTACK_1();
			break;
		case Boss_MaraDevil.EState.COMBO_ATTACK_2:
			this.OnCOMBO_ATTACK_2();
			break;
		case Boss_MaraDevil.EState.COMBO_ATTACK_3:
			this.OnCOMBO_ATTACK_3();
			break;
		case Boss_MaraDevil.EState.FAST_RUN:
			this.currentRUNSpeedPercent = this.FAST_RUNSpeedPercent;
			this.OnRUN();
			break;
		case Boss_MaraDevil.EState.COMBO_ATTACK_1_2:
			this.OnCOMBO_ATTACK_1_2();
			break;
		default:
			this.OnIDLE();
			break;
		}
	}

	private bool ChangeBossState()
	{
		if (this.State == ECharactor.DIE)
		{
			return false;
		}
		bool flag = false;
		switch (this.bossState)
		{
		case Boss_MaraDevil.EState.ATTACK_1:
			if (this.TriggerIDLE())
			{
				this.OnExitATTACK_1();
				this.bossState = Boss_MaraDevil.EState.IDLE;
				flag = true;
			}
			break;
		case Boss_MaraDevil.EState.ATTACK_2:
			if (this.TriggerIDLE())
			{
				this.OnExitATTACK_2();
				this.bossState = Boss_MaraDevil.EState.IDLE;
				flag = true;
			}
			break;
		case Boss_MaraDevil.EState.ATTACK_3:
			if (this.TriggerIDLE())
			{
				this.bossState = Boss_MaraDevil.EState.IDLE;
				flag = true;
			}
			break;
		case Boss_MaraDevil.EState.DIE:
			break;
		case Boss_MaraDevil.EState.HIT:
			if (this.TriggerIDLE())
			{
				this.bossState = Boss_MaraDevil.EState.IDLE;
				flag = true;
			}
			break;
		case Boss_MaraDevil.EState.IDLE:
			if (this.TriggerATTACK_1() || this.TriggerATTACK_2() || this.TriggerATTACK_3())
			{
				this.RandomBossAttack();
				flag = true;
			}
			else if (this.TriggerDIE())
			{
				this.bossState = Boss_MaraDevil.EState.DIE;
				flag = true;
			}
			else if (this.TriggerHIT())
			{
				this.bossState = Boss_MaraDevil.EState.HIT;
				flag = true;
			}
			else if (this.TriggerIDLE())
			{
				this.bossState = Boss_MaraDevil.EState.IDLE;
				flag = true;
			}
			else if (this.TriggerRUN())
			{
				if (UnityEngine.Random.Range(1, 10) % 3 == 1)
				{
					this.RandomBossAttack();
				}
				else
				{
					this.RUNCount = 0;
					if (UnityEngine.Random.Range(1, 10) % 3 == 1)
					{
						this.bossState = Boss_MaraDevil.EState.FAST_RUN;
					}
					else
					{
						this.bossState = Boss_MaraDevil.EState.RUN;
					}
				}
				flag = true;
			}
			break;
		case Boss_MaraDevil.EState.RUN:
		case Boss_MaraDevil.EState.FAST_RUN:
			if (this.TriggerIDLE())
			{
				if (this.RUNCount < this.currentRUNMaxCount || !this.ReachedToTarget(this.transform.position, this.targetRun))
				{
					flag = true;
				}
				else
				{
					this.bossState = Boss_MaraDevil.EState.IDLE;
					flag = true;
				}
			}
			break;
		case Boss_MaraDevil.EState.START:
			if (this.TriggerIDLE())
			{
				this.meshRenderer.sortingOrder = this.afterInitOrderInLayer;
				this.meshRenderer.sortingLayerName = this.afterInitLayer;
				this.bossState = Boss_MaraDevil.EState.IDLE;
				flag = true;
			}
			break;
		case Boss_MaraDevil.EState.START_OPEN:
			if (this.TriggerSTART())
			{
				this.bossState = Boss_MaraDevil.EState.START;
				flag = true;
			}
			break;
		case Boss_MaraDevil.EState.COMBO_ATTACK_1:
			flag = this.ChangeCOMBO_ATTACK_1Step();
			break;
		case Boss_MaraDevil.EState.COMBO_ATTACK_2:
			flag = this.ChangeCOMBO_ATTACK_2Step();
			break;
		case Boss_MaraDevil.EState.COMBO_ATTACK_3:
			flag = this.ChangeCOMBO_ATTACK_3Step();
			break;
		case Boss_MaraDevil.EState.COMBO_ATTACK_1_2:
			flag = this.ChangeCOMBO_ATTACK_1_2Step();
			break;
		default:
			flag = false;
			break;
		}
		if (flag)
		{
			this.ClearDoneFlag();
		}
		return flag;
	}

	private bool ChangeCOMBO_ATTACK_1Step()
	{
		bool result = false;
		if (this.COMBO_ATTACK_1_Step == 0 && this.ReachedToTarget(this.transform.position, this.targetRun))
		{
			this.COMBO_ATTACK_1_Step = 1;
			result = true;
		}
		else if (this.COMBO_ATTACK_1_Step == 1 && this.ReachedToTarget(this.transform.position, this.targetRun))
		{
			this.COMBO_ATTACK_1_Step = 2;
			result = true;
		}
		else if (this.doneATTACK_1 && this.COMBO_ATTACK_1_Step == 2 && this.ReachedToTarget(this.transform.position, this.targetRun))
		{
			this.COMBO_ATTACK_1_Step = 3;
			result = true;
		}
		else if (this.COMBO_ATTACK_1_Step == 3 && this.ReachedToTarget(this.transform.position, this.targetRun))
		{
			this.COMBO_ATTACK_1_Step = 4;
			result = true;
		}
		else if (this.COMBO_ATTACK_1_Step == 4 && this.ReachedToTarget(this.transform.position, this.targetRun))
		{
			this.COMBO_ATTACK_1_Step = 5;
			result = true;
		}
		else if (this.doneATTACK_1 && this.COMBO_ATTACK_1_Step == 5 && this.ReachedToTarget(this.transform.position, this.targetRun))
		{
			this.COMBO_ATTACK_1_Step = 6;
			result = true;
		}
		else if (this.COMBO_ATTACK_1_Step == 6 && this.ReachedToTarget(this.transform.position, this.targetRun))
		{
			this.bossState = Boss_MaraDevil.EState.IDLE;
			result = true;
		}
		return result;
	}

	private bool ChangeCOMBO_ATTACK_2Step()
	{
		bool result = false;
		if (this.COMBO_ATTACK_2_Step == 0 && this.ReachedToTarget(this.transform.position, this.targetRun))
		{
			this.ShakedCame = false;
			this.COMBO_ATTACK_2_Step = 1;
			result = true;
		}
		else if (this.eventAttack_2 && this.COMBO_ATTACK_2_Step == 1 && this.ReachedToTarget(this.transform.position, this.targetRun))
		{
			this.COMBO_ATTACK_2_Step = 2;
			result = true;
		}
		else if (this.COMBO_ATTACK_2_Step == 2 && this.ReachedToTarget(this.transform.position, this.targetRun))
		{
			if (!this.ShakedCame)
			{
				this.ShakeCam(false);
				this.ShakedCame = true;
			}
			if (this.doneATTACK_2)
			{
				this.COMBO_ATTACK_2_Step = 3;
				result = true;
			}
		}
		else if (this.COMBO_ATTACK_2_Step == 3 && this.ReachedToTarget(this.transform.position, this.targetRun))
		{
			this.bossState = Boss_MaraDevil.EState.IDLE;
			result = true;
		}
		return result;
	}

	private bool ChangeCOMBO_ATTACK_3Step()
	{
		bool result = false;
		if (this.COMBO_ATTACK_3_Step == 0 && this.ReachedToTarget(this.transform.position, this.targetRun))
		{
			this.COMBO_ATTACK_3_Step = 1;
			result = true;
		}
		else if (this.doneATTACK_3 && this.COMBO_ATTACK_3_Step == 1 && this.ReachedToTarget(this.transform.position, this.targetRun))
		{
			this.COMBO_ATTACK_3_Step = 2;
			result = true;
		}
		else if (this.doneATTACK_3 && this.COMBO_ATTACK_3_Step == 2 && this.ReachedToTarget(this.transform.position, this.targetRun))
		{
			this.bossState = Boss_MaraDevil.EState.IDLE;
			result = true;
		}
		return result;
	}

	private bool ChangeCOMBO_ATTACK_1_2Step()
	{
		bool result = false;
		if (this.COMBO_ATTACK_1_2_Step == 0 && this.ReachedToTarget(this.transform.position, this.targetRun))
		{
			this.ShakedCame = false;
			this.COMBO_ATTACK_1_2_Step = 1;
			result = true;
		}
		else if (this.COMBO_ATTACK_1_2_Step == 1 && this.ReachedToTarget(this.transform.position, this.targetRun))
		{
			this.COMBO_ATTACK_1_2_Step = 2;
			result = true;
		}
		else if (this.doneATTACK_1 && this.COMBO_ATTACK_1_2_Step == 2 && this.ReachedToTarget(this.transform.position, this.targetRun))
		{
			this.COMBO_ATTACK_1_2_Step = 3;
			result = true;
		}
		else if (this.COMBO_ATTACK_1_2_Step == 3 && this.ReachedToTarget(this.transform.position, this.targetRun))
		{
			this.COMBO_ATTACK_1_2_Step = 4;
			result = true;
		}
		else if (this.eventAttack_2 && this.COMBO_ATTACK_1_2_Step == 4 && this.ReachedToTarget(this.transform.position, this.targetRun))
		{
			this.COMBO_ATTACK_1_2_Step = 5;
			result = true;
		}
		else if (this.COMBO_ATTACK_1_2_Step == 5 && this.ReachedToTarget(this.transform.position, this.targetRun))
		{
			if (!this.ShakedCame)
			{
				this.ShakeCam(false);
				this.ShakedCame = true;
			}
			if (this.doneATTACK_2)
			{
				this.COMBO_ATTACK_1_2_Step = 6;
				result = true;
			}
		}
		else if (this.COMBO_ATTACK_1_2_Step == 6 && this.ReachedToTarget(this.transform.position, this.targetRun))
		{
			this.bossState = Boss_MaraDevil.EState.IDLE;
			result = true;
		}
		return result;
	}

	private bool ReachedToTarget(Vector3 curPos, Vector3 target)
	{
		return Vector2.Distance(curPos, target) < 0.1f;
	}

	private void RandomBossAttack()
	{
		if (UnityEngine.Random.Range(1, 10) % 5 == 0)
		{
			this.RUNCount = 0;
			if (UnityEngine.Random.Range(1, 10) % 3 == 1)
			{
				this.bossState = Boss_MaraDevil.EState.FAST_RUN;
			}
			else
			{
				this.bossState = Boss_MaraDevil.EState.RUN;
			}
		}
		else
		{
			this.bossState = (Boss_MaraDevil.EState)(UnityEngine.Random.Range(0, 6) % 3);
			if (this.bossState == Boss_MaraDevil.EState.ATTACK_1)
			{
				if (UnityEngine.Random.Range(1, 10) % 3 == 0)
				{
					this.OnPreCOMBO_ATTACK_1();
					this.bossState = Boss_MaraDevil.EState.COMBO_ATTACK_1;
				}
				else if (UnityEngine.Random.Range(1, 10) % 3 == 1)
				{
					this.OnPreCOMBO_ATTACK_1_2();
					this.bossState = Boss_MaraDevil.EState.COMBO_ATTACK_1_2;
				}
				else
				{
					this.bossState = Boss_MaraDevil.EState.ATTACK_1;
				}
			}
			else if (this.bossState == Boss_MaraDevil.EState.ATTACK_2)
			{
				if (UnityEngine.Random.Range(1, 10) % 3 == 0)
				{
					this.OnPreCOMBO_ATTACK_2();
					this.bossState = Boss_MaraDevil.EState.COMBO_ATTACK_2;
				}
				else
				{
					this.bossState = Boss_MaraDevil.EState.ATTACK_2;
				}
			}
			else if (this.bossState == Boss_MaraDevil.EState.ATTACK_3)
			{
				if (UnityEngine.Random.Range(1, 10) % 2 == 0)
				{
					this.OnPreCOMBO_ATTACK_3();
					this.bossState = Boss_MaraDevil.EState.COMBO_ATTACK_3;
				}
				else
				{
					this.bossState = Boss_MaraDevil.EState.ATTACK_3;
				}
			}
		}
	}

	private bool TriggerATTACK_1()
	{
		return Mathf.Abs(this.ramboPosition.x - this.tfOrigin.position.x) < this.ATTACK_1AreaRadius;
	}

	private bool TriggerATTACK_2()
	{
		return Mathf.Abs(this.ramboPosition.x - this.leftHandPosition.x) < this.ATTACK_2AreaRadius || Mathf.Abs(this.ramboPosition.x - this.rightHandPosition.x) < this.ATTACK_2AreaRadius;
	}

	private bool TriggerATTACK_3()
	{
		return Mathf.Abs(this.ramboPosition.x - this.leftShoulderPosition.x + 0.5f) < this.ATTACK_3AreaRadius || Mathf.Abs(this.ramboPosition.x - this.rightShoulderPosition.x - 0.5f) < this.ATTACK_3AreaRadius;
	}

	private bool TriggerDIE()
	{
		return false;
	}

	private bool TriggerHIT()
	{
		return false;
	}

	private bool TriggerIDLE()
	{
		return this.doneATTACK_1 || this.doneATTACK_2 || this.doneATTACK_3 || this.doneIDLE || this.doneRUN || this.doneSTART;
	}

	private bool TriggerRUN()
	{
		return Mathf.Abs(this.ramboPosition.x - this.tfOrigin.position.x) > this.runAreaRadius;
	}

	private bool TriggerSTART()
	{
		return this.doneSTART_OPEN;
	}

	private void ClearDoneFlag()
	{
		this.doneATTACK_1 = false;
		this.doneATTACK_2 = false;
		this.doneATTACK_3 = false;
		this.doneDIE = false;
		this.doneHIT = false;
		this.doneIDLE = false;
		this.doneRUN = false;
		this.doneSTART = false;
		this.doneSTART_OPEN = false;
		this.eventAttack_2 = false;
	}

	private void OnATTACK_1()
	{
		this.RandomMiniAttack();
		this.rotationAngle = UnityEngine.Random.Range(-5f, 5f);
		this.ATTACK_1Box.Active(this.ATTACK_1DamagePercent * this.cacheEnemy.Damage, true, null);
		this.ATTACK_2BoxLeft.Active(this.ATTACK_2DamagePercent * this.cacheEnemy.Damage, true, null);
		this.ATTACK_2BoxRight.Active(this.ATTACK_2DamagePercent * this.cacheEnemy.Damage, true, null);
		this.TriggerMINI_THONG_BAO();
		this.PlayAnim(Boss_MaraDevil.EState.ATTACK_1, false, 1f);
		this.PlaySound(this.ATTACK_1Clip, this.ATTACK_1_Volume);
	}

	private void OnExitATTACK_1()
	{
		this.ATTACK_1Box.Deactive();
		this.ATTACK_2BoxLeft.Deactive();
		this.ATTACK_2BoxRight.Deactive();
	}

	private void OnATTACK_2()
	{
		this.RandomMiniAttack();
		this.rotationAngle = 0f;
		this.targetRun = new Vector3(this.transform.position.x, this.roadMinYPos, this.transform.position.z);
		this.ATTACK_2BoxLeft.Active(this.ATTACK_2DamagePercent * this.cacheEnemy.Damage, true, null);
		this.ATTACK_2BoxRight.Active(this.ATTACK_2DamagePercent * this.cacheEnemy.Damage, true, null);
		this.TriggerMINI_THONG_BAO();
		this.PlayAnim(Boss_MaraDevil.EState.ATTACK_2, false, 1f);
	}

	private void OnExitATTACK_2()
	{
		this.ATTACK_2BoxLeft.Deactive();
		this.ATTACK_2BoxRight.Deactive();
	}

	private void OnATTACK_3()
	{
		this.rotationAngle = ((this.ramboPosition.x >= this.transform.position.x) ? UnityEngine.Random.Range(0f, 5f) : UnityEngine.Random.Range(-5f, 0f));
		this.TriggerMINI_THONG_BAO();
		this.PlayAnim(Boss_MaraDevil.EState.ATTACK_3, false, 1f);
	}

	private void OnDIE()
	{
		this.targetRun = new Vector3(this.transform.position.x, this.roadMinYPos, this.transform.position.z);
		this.bossState = Boss_MaraDevil.EState.DIE;
		this.PlayAnim(Boss_MaraDevil.EState.DIE, false, 1f);
		this.ShakeCam(false);
		if (this.lineBloodEnemy != null)
		{
			this.lineBloodEnemy.Hide();
		}
	}

	private void OnHIT()
	{
		this.PlayAnim(Boss_MaraDevil.EState.HIT, false, 1f);
	}

	private void OnIDLE()
	{
		this.rotationAngle = 0f;
		this.RandomMiniAttack();
		this.PlayAnim(Boss_MaraDevil.EState.IDLE, false, 1f);
	}

	private void OnRUN()
	{
		this.rotationAngle = ((this.ramboPosition.x <= this.transform.position.x) ? UnityEngine.Random.Range(0f, 5f) : UnityEngine.Random.Range(-5f, 0f));
		this.RandomMiniAttack();
		if (this.HP > this.cacheEnemy.HP * 0.67f)
		{
			this.currentRUNMaxCount = this.RUNMaxCount;
			if (this.RUNCount < this.currentRUNMaxCount)
			{
				this.targetRun = new Vector3(this.ramboPosition.x + UnityEngine.Random.Range(-6f, 6f), UnityEngine.Random.Range(this.roadMinYPos, this.topLeftCameraPos.y), this.transform.position.z);
				this.RUNCount++;
			}
		}
		else if (this.HP < this.cacheEnemy.HP * 0.67f && this.HP > this.cacheEnemy.HP * 0.33f)
		{
			this.currentRUNMaxCount = this.RUNMaxCount - 1;
			if (this.RUNCount < this.currentRUNMaxCount)
			{
				this.targetRun = new Vector3(this.ramboPosition.x + UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(this.roadMinYPos, this.topLeftCameraPos.y), this.transform.position.z);
				this.RUNCount++;
			}
		}
		else
		{
			this.currentRUNMaxCount = this.RUNMaxCount - 2;
			if (this.RUNCount < this.currentRUNMaxCount)
			{
				this.targetRun = new Vector3(this.ramboPosition.x + UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(this.roadMinYPos, this.topLeftCameraPos.y), this.transform.position.z);
				this.RUNCount++;
			}
		}
		this.PlayAnim(Boss_MaraDevil.EState.RUN, false, 1f);
	}

	private void GetRUNMaxCountByGameMode()
	{
		if (this.gameMode != GameMode.Mode.NORMAL)
		{
			if (this.gameMode == GameMode.Mode.HARD)
			{
				this.RUNMaxCount--;
			}
			else if (this.gameMode == GameMode.Mode.SUPER_HARD)
			{
				this.RUNMaxCount -= 2;
			}
		}
	}

	private void OnSTART()
	{
		this.rotationAngle = 0f;
		this.PlayAnim(Boss_MaraDevil.EState.START, false, 1f);
		this.PlaySound(this.STARTClip, this.START_Volume);
	}

	private void OnSTART_OPEN()
	{
		this.rotationAngle = 0f;
		this.PlayAnim(Boss_MaraDevil.EState.START_OPEN, false, 1f);
	}

	private void OnPreCOMBO_ATTACK_1()
	{
		this.COMBO_ATTACK_1_Step = 0;
	}

	private void OnCOMBO_ATTACK_1()
	{
		if (this.COMBO_ATTACK_1_Step == 0)
		{
			this.targetRun = new Vector3(this.topLeftCameraPos.x + UnityEngine.Random.Range(3f, 5f), UnityEngine.Random.Range(this.roadMinYPos, this.topLeftCameraPos.y), 0f);
			this.PlayAnim(Boss_MaraDevil.EState.RUN, true, 1f);
		}
		else if (this.COMBO_ATTACK_1_Step == 1)
		{
			this.OnATTACK_1();
			this.targetRun = new Vector3(this.ramboPosition.x + UnityEngine.Random.Range(-2f, 2f), this.ramboPosition.y + UnityEngine.Random.Range(-1f, -2f), this.transform.position.z);
		}
		else if (this.COMBO_ATTACK_1_Step == 2)
		{
			this.targetRun = new Vector3(this.ramboPosition.x + UnityEngine.Random.Range(-5f, 5f), this.ramboPosition.y + UnityEngine.Random.Range(-1f, -2f), this.transform.position.z);
		}
		else if (this.COMBO_ATTACK_1_Step == 3)
		{
			this.OnExitATTACK_1();
			this.targetRun = new Vector3(this.topRightCameraPos.x - UnityEngine.Random.Range(3f, 5f), UnityEngine.Random.Range(this.roadMinYPos, this.topLeftCameraPos.y), 0f);
			this.PlayAnim(Boss_MaraDevil.EState.RUN, true, 1f);
		}
		else if (this.COMBO_ATTACK_1_Step == 4)
		{
			this.OnATTACK_1();
			this.targetRun = new Vector3(this.ramboPosition.x + UnityEngine.Random.Range(-2f, 2f), this.ramboPosition.y + UnityEngine.Random.Range(-1f, -2f), this.transform.position.z);
		}
		else if (this.COMBO_ATTACK_1_Step == 5)
		{
			this.targetRun = new Vector3(this.ramboPosition.x + UnityEngine.Random.Range(-5f, 5f), this.ramboPosition.y + UnityEngine.Random.Range(-1f, -2f), this.transform.position.z);
		}
		else if (this.COMBO_ATTACK_1_Step == 6)
		{
			this.OnExitATTACK_1();
			this.targetRun = new Vector3(this.topLeftCameraPos.x + UnityEngine.Random.Range(3f, 5f), UnityEngine.Random.Range(this.roadMinYPos, this.topLeftCameraPos.y), 0f);
			this.PlayAnim(Boss_MaraDevil.EState.RUN, true, 1f);
		}
	}

	private void OnPreCOMBO_ATTACK_2()
	{
		this.COMBO_ATTACK_2_Step = 0;
	}

	private void OnCOMBO_ATTACK_2()
	{
		if (this.COMBO_ATTACK_2_Step == 0)
		{
			this.targetRun = new Vector3(this.ramboPosition.x + UnityEngine.Random.Range(-2f, 2f), this.topLeftCameraPos.y + UnityEngine.Random.Range(5f, 6f), this.transform.position.z);
			this.PlayAnim(Boss_MaraDevil.EState.RUN, true, 1f);
		}
		else if (this.COMBO_ATTACK_2_Step == 1)
		{
			this.OnATTACK_2();
			this.targetRun = new Vector3(this.ramboPosition.x + UnityEngine.Random.Range(-2f, 2f), this.transform.position.y, this.transform.position.z);
		}
		else if (this.COMBO_ATTACK_2_Step == 2)
		{
			this.targetRun = new Vector3(this.transform.position.x, this.roadMinYPos, this.transform.position.z);
		}
		else if (this.COMBO_ATTACK_2_Step == 3)
		{
			this.OnExitATTACK_2();
		}
	}

	private void OnPreCOMBO_ATTACK_3()
	{
		this.COMBO_ATTACK_3_Step = 0;
	}

	private void OnCOMBO_ATTACK_3()
	{
		if (this.COMBO_ATTACK_3_Step == 0)
		{
			this.targetRun = new Vector3(this.topRightCameraPos.x - UnityEngine.Random.Range(3f, 5f), UnityEngine.Random.Range(this.roadMinYPos, this.topLeftCameraPos.y), 0f);
			this.PlayAnim(Boss_MaraDevil.EState.RUN, true, 1f);
		}
		else if (this.COMBO_ATTACK_3_Step == 1)
		{
			this.OnATTACK_3();
			this.targetRun = new Vector3(this.topLeftCameraPos.x + UnityEngine.Random.Range(3f, 5f), UnityEngine.Random.Range(this.roadMinYPos, this.topLeftCameraPos.y), 0f);
		}
		else if (this.COMBO_ATTACK_3_Step == 2)
		{
			this.OnATTACK_3();
			this.targetRun = new Vector3(this.topRightCameraPos.x - UnityEngine.Random.Range(3f, 5f), UnityEngine.Random.Range(this.roadMinYPos, this.topLeftCameraPos.y), 0f);
		}
	}

	private void OnPreCOMBO_ATTACK_1_2()
	{
		this.COMBO_ATTACK_1_2_Step = 0;
	}

	private void OnCOMBO_ATTACK_1_2()
	{
		if (this.COMBO_ATTACK_1_2_Step == 0)
		{
			this.targetRun = new Vector3(this.topLeftCameraPos.x + UnityEngine.Random.Range(3f, 5f), UnityEngine.Random.Range(this.roadMinYPos, this.topLeftCameraPos.y), this.transform.position.z);
			this.PlayAnim(Boss_MaraDevil.EState.RUN, true, 1f);
		}
		else if (this.COMBO_ATTACK_1_2_Step == 1)
		{
			this.OnATTACK_1();
			this.targetRun = new Vector3(this.ramboPosition.x + UnityEngine.Random.Range(-2f, 2f), this.ramboPosition.y + UnityEngine.Random.Range(-1f, -2f), this.transform.position.z);
		}
		else if (this.COMBO_ATTACK_1_2_Step == 2)
		{
			this.targetRun = new Vector3(this.ramboPosition.x + UnityEngine.Random.Range(-5f, 5f), this.ramboPosition.y + UnityEngine.Random.Range(-1f, -2f), this.transform.position.z);
		}
		else if (this.COMBO_ATTACK_1_2_Step == 3)
		{
			this.OnExitATTACK_1();
			this.targetRun = new Vector3(this.ramboPosition.x + UnityEngine.Random.Range(-2f, 2f), this.topLeftCameraPos.y + UnityEngine.Random.Range(5f, 6f), this.transform.position.z);
			this.PlayAnim(Boss_MaraDevil.EState.RUN, true, 1f);
		}
		else if (this.COMBO_ATTACK_1_2_Step == 4)
		{
			this.OnATTACK_2();
			this.targetRun = new Vector3(this.ramboPosition.x + UnityEngine.Random.Range(-2f, 2f), this.transform.position.y, this.transform.position.z);
		}
		else if (this.COMBO_ATTACK_1_2_Step == 5)
		{
			this.targetRun = new Vector3(this.transform.position.x, this.roadMinYPos, this.transform.position.z);
		}
		else if (this.COMBO_ATTACK_1_2_Step == 6)
		{
			this.OnExitATTACK_2();
			this.targetRun = new Vector3(this.topRightCameraPos.x - UnityEngine.Random.Range(3f, 5f), UnityEngine.Random.Range(this.roadMinYPos, this.topLeftCameraPos.y), 0f);
			this.PlayAnim(Boss_MaraDevil.EState.RUN, true, 1f);
		}
	}

	private void RandomMiniAttack()
	{
		if (UnityEngine.Random.Range(1, 10) % 4 == 1)
		{
			if (UnityEngine.Random.Range(1, 10) % 2 == 0)
			{
				this.TriggerMINI_ATTACK_1();
			}
			else
			{
				this.TriggerMINI_ATTACK_2_1();
			}
		}
	}

	private void TriggerMINI_ATTACK_1()
	{
		this.leftMiniBoss.triggerMINI_ATTACK_1 = true;
		this.rightMiniBoss.triggerMINI_ATTACK_1 = true;
	}

	private void TriggerMINI_ATTACK_2_1()
	{
		this.leftMiniBoss.triggerMINI_ATTACK_2_1 = true;
		this.rightMiniBoss.triggerMINI_ATTACK_2_1 = true;
	}

	private void TriggerMINI_THONG_BAO()
	{
		this.leftMiniBoss.triggerMINI_THONG_BAO = true;
		this.rightMiniBoss.triggerMINI_THONG_BAO = true;
	}

	private void LoadBossData()
	{
		string text = (!SplashScreen._isLoadResourceDecrypt) ? this.Data_Encrypt.text : this.Data_Decrypt.text;
		string text2 = ProfileManager.DataEncryption.decrypt2(text);
		this.bossData = JsonConvert.DeserializeObject<EnemyCharactor>((!SplashScreen._isLoadResourceDecrypt) ? text2 : text);
	}

	private void InitSkeletonAnimation()
	{
		this.skeletonAnimation.timeScale = 0f;
		this.skeletonAnimation.state.Event += this.HandleEvent;
		this.skeletonAnimation.state.Complete += this.HandleComplete;
	}

	private void HandleEvent(TrackEntry entry, Spine.Event e)
	{
		if (entry == null || GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			return;
		}
		string name = e.Data.Name;
		if (name != null)
		{
			if (!(name == "attack-3-2"))
			{
				if (!(name == "attack-3-3"))
				{
					if (name == "attack-2")
					{
						this.eventAttack_2 = true;
						this.PlaySound(this.ATTACK_2Clip, this.ATTACK_2_Volume);
					}
				}
				else
				{
					base.StartCoroutine(this.CreatBullet(this.listStartGunBonePosition[0], this.listEndGunBonePosition[0]));
					base.StartCoroutine(this.CreatBullet(this.listStartGunBonePosition[2], this.listEndGunBonePosition[2]));
				}
			}
			else
			{
				base.StartCoroutine(this.CreatBullet(this.listStartGunBonePosition[1], this.listEndGunBonePosition[1]));
			}
		}
	}

	private void HandleComplete(TrackEntry entry)
	{
		if (entry == null)
		{
			return;
		}
		string text = entry.ToString();
		switch (text)
		{
		case "attack-1":
			this.doneATTACK_1 = true;
			break;
		case "attack-2":
			this.doneATTACK_2 = true;
			break;
		case "attack-3":
			this.doneATTACK_3 = true;
			break;
		case "die":
			this.doneDIE = true;
			break;
		case "hit":
			this.doneHIT = true;
			break;
		case "idle":
			this.doneIDLE = true;
			break;
		case "run":
			this.doneRUN = true;
			break;
		case "start":
			this.doneSTART = true;
			break;
		case "start-open":
			this.doneSTART_OPEN = true;
			break;
		}
	}

	private bool PauseCondition()
	{
		return !this.isInit || this.State == ECharactor.DIE || (GameManager.Instance.StateManager.EState != EGamePlay.RUNNING && GameManager.Instance.StateManager.EState != EGamePlay.PREVIEW) || GameManager.Instance.StateManager.EState == EGamePlay.PAUSE;
	}

	private void PauseGame()
	{
		if (this.skeletonAnimation.timeScale != 0f)
		{
			this.cacheTimeScale = this.skeletonAnimation.timeScale;
			this.skeletonAnimation.timeScale = 0f;
		}
	}

	private void ResumeGame()
	{
		if (this.skeletonAnimation.timeScale == 0f)
		{
			this.skeletonAnimation.timeScale = this.cacheTimeScale;
		}
	}

	private void InitAnimations()
	{
		this.animations = this.skeletonAnimation.skeletonDataAsset.GetAnimationStateData().SkeletonData.Animations.Items;
	}

	private void PlayAnim(Boss_MaraDevil.EState state, bool loop = false, float speedAnim = 1f)
	{
		this.skeletonAnimation.timeScale = speedAnim;
		this.skeletonAnimation.AnimationState.SetAnimation(0, this.animations[(int)state], loop);
	}

	private void PlayAnim(Boss_MaraDevil.EState state, int order)
	{
		this.skeletonAnimation.AnimationState.SetAnimation(order, this.animations[(int)state], false);
	}

	private void InitBoxBoss()
	{
		if (this.boxBoss != null)
		{
			for (int i = 0; i < this.boxBoss.Length; i++)
			{
				this.boxBoss[i].bodyCollider2D.enabled = true;
				this.boxBoss[i].InitEnemy(this.HP);
				this.boxBoss[i].OnHit = new Action<float, EWeapon>(this.Hit);
				this.boxBoss[i].tfOrigin = null;
				this.boxBoss[i].tag = "Boss";
				GameManager.Instance.ListEnemy.Add(this.boxBoss[i]);
			}
		}
	}

	public void Hit(float damage, EWeapon eweapon)
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
		this.HP += damage;
		this.lastWeapon = eweapon;
		float mode = GameMode.Instance.GetMode();
		if (this.HP <= 0f && this.State != ECharactor.DIE)
		{
			base.StopAllCoroutines();
			this.OnDIE();
			GameManager.Instance.bossManager.ShowLineBloodBoss(0f, this.cacheEnemy.HP * mode);
			GameManager.Instance.hudManager.HideControl();
			base.StartCoroutine(this.EffectDie());
			if (!ProfileManager.bossModeProfile.bossProfiles[(int)this.boss].CheckUnlock(GameMode.Mode.NORMAL))
			{
				ProfileManager.bossModeProfile.bossProfiles[(int)this.boss].SetUnlock(GameMode.Mode.NORMAL, true);
				UIShowInforManager.Instance.ShowUnlock("Mara Devil has been unlocked in BossMode!");
			}
			return;
		}
		this.PlayAnim(Boss_MaraDevil.EState.HIT, 2);
		GameManager.Instance.bossManager.ShowLineBloodBoss(this.HP, this.cacheEnemy.HP * mode);
	}

	private IEnumerator EffectDie()
	{
		yield return new WaitForSeconds(3.66f);
		base.StartCoroutine(this.WaitMissionCompleted());
		PlayerManagerStory.Instance.OnRunGameOver();
		DailyQuestManager.Instance.MissionBoss(0, 1, delegate(bool isCompleted, DailyQuestManager.InforQuest infor)
		{
			if (isCompleted)
			{
				UIShowInforManager.Instance.OnShowDailyQuest(infor.Desc, infor.item, infor.amount);
			}
		});
		yield break;
	}

	private IEnumerator WaitMissionCompleted()
	{
		if (GameMode.Instance.Style == GameMode.GameStyle.SinglPlayer && GameMode.Instance.modePlay == GameMode.ModePlay.Campaign && !ProfileManager.bossModeProfile.bossProfiles[18].CheckUnlock(GameMode.Mode.NORMAL))
		{
			ProfileManager.bossModeProfile.bossProfiles[18].SetUnlock(GameMode.Mode.NORMAL, true);
			yield return new WaitForSeconds(1f);
			UIShowInforManager.Instance.ShowUnlock("BossMode is unlocked!");
			yield return new WaitForSeconds(0.5f);
			UIShowInforManager.Instance.ShowUnlock("Mara_Devil has been unlocked in BossMode!");
		}
		yield break;
	}

	private void ShakeCam(bool isBegin = false)
	{
		try
		{
			if (GameManager.Instance.StateManager.EState != EGamePlay.WIN)
			{
				if (!isBegin)
				{
					CameraController.Instance.Shake(CameraController.ShakeType.Explosion);
				}
				else
				{
					CameraController.Instance.Shake(CameraController.ShakeType.BulletPlayer);
				}
			}
		}
		catch
		{
		}
	}

	private void PlaySound(AudioClip sound, float volume = 1f)
	{
		try
		{
			SingletonGame<AudioController>.Instance.PlaySound(sound, volume);
		}
		catch
		{
		}
	}

	[Header("**************** Boss_MaraDevil ***************")]
	private EnemyCharactor bossData;

	private GameMode.Mode gameMode;

	public float roadMinYPos;

	[SerializeField]
	[Header("============ Spine Anim ===========")]
	private SkeletonAnimation skeletonAnimation;

	private float cacheTimeScale;

	private Spine.Animation[] animations;

	[SerializeField]
	private BoxBossMaraDevil[] boxBoss;

	[SerializeField]
	private string afterInitLayer;

	[SerializeField]
	private int afterInitOrderInLayer;

	[SerializeField]
	private Camera mainCamera;

	private Vector3 topLeftCameraPos;

	private Vector3 topRightCameraPos;

	private Vector3 bottomLeftCameraPos;

	private Vector3 bottomRightCameraPos;

	[Header("============ Attack ===============")]
	private Transform ramboTransform;

	private Vector3 ramboPosition;

	private float rotationAngle;

	[SerializeField]
	[Header("__________ Attack Xoay")]
	private float ATTACK_1AreaRadius;

	[SerializeField]
	private AttackBox ATTACK_1Box;

	[SerializeField]
	private float ATTACK_1DamagePercent;

	[SerializeField]
	private float ATTACK_1SpeedPercent;

	[SerializeField]
	private AudioClip ATTACK_1Clip;

	[SerializeField]
	private float ATTACK_1_Volume;

	[SerializeField]
	[Header("__________ Attack Dam Xuong")]
	private string leftHandBoneName;

	private Bone leftHandBone;

	private Vector3 leftHandPosition;

	[SerializeField]
	private AttackBox ATTACK_2BoxLeft;

	[SerializeField]
	private string rightHandBoneName;

	private Bone rightHandBone;

	private Vector3 rightHandPosition;

	[SerializeField]
	private AttackBox ATTACK_2BoxRight;

	[SerializeField]
	private float ATTACK_2AreaRadius;

	[SerializeField]
	private float ATTACK_2DamagePercent;

	[SerializeField]
	private float ATTACK_2SpeedPercent;

	[SerializeField]
	private AudioClip ATTACK_2Clip;

	[SerializeField]
	private float ATTACK_2_Volume;

	[Header("__________ Attack Ban Dan")]
	[SerializeField]
	private string leftShoulderBoneName;

	private Bone leftShoulderBone;

	private Vector3 leftShoulderPosition;

	[SerializeField]
	private string rightShoulderBoneName;

	private Bone rightShoulderBone;

	private Vector3 rightShoulderPosition;

	[SerializeField]
	private float ATTACK_3AreaRadius;

	[SerializeField]
	private float ATTACK_3DamagePercent;

	[SerializeField]
	private float ATTACK_3SpeedPercent;

	[SerializeField]
	private float ATTACK_3ShootSpeedPercent;

	[SerializeField]
	private List<StraightBullet> listBulletBoss;

	private ObjectPooling<StraightBullet> poolingBulletsBoss;

	private StraightBullet _bullet;

	[SerializeField]
	private List<string> listStartGunName;

	private List<Bone> listStartGunBone;

	[SerializeField]
	private Vector3[] listStartGunBonePosition;

	[SerializeField]
	private List<string> listEndGunName;

	private List<Bone> listEndGunBone;

	[SerializeField]
	private Vector3[] listEndGunBonePosition;

	[SerializeField]
	private AudioClip ATTACK_3Clip;

	[SerializeField]
	private float ATTACK_3_Volume;

	[SerializeField]
	[Header("__________ RUN")]
	private float runAreaRadius;

	private Vector3 targetRun;

	[SerializeField]
	private float RUNSpeedPercent;

	[SerializeField]
	private int RUNMaxCount;

	private int RUNCount;

	private int currentRUNMaxCount;

	private float currentRUNSpeedPercent;

	[Header("__________ FAST_RUN")]
	[SerializeField]
	private float FAST_RUNSpeedPercent;

	[SerializeField]
	[Header("__________ COMBO_ATTACK_1")]
	private float COMBO_ATTACK_1SpeedPercent;

	private int COMBO_ATTACK_1_Step;

	[Header("__________ COMBO_ATTACK_2")]
	[SerializeField]
	private float COMBO_ATTACK_2SpeedPercent;

	[SerializeField]
	private float COMBO_ATTACK_2Step2SpeedPercent;

	private int COMBO_ATTACK_2_Step;

	private bool ShakedCame;

	[SerializeField]
	[Header("__________ COMBO_ATTACK_3")]
	private float COMBO_ATTACK_3SpeedPercent;

	private int COMBO_ATTACK_3_Step;

	[SerializeField]
	[Header("__________ COMBO_ATTACK_1_2")]
	private int COMBO_ATTACK_1_2_Step;

	[SerializeField]
	[Header("__________ START")]
	private AudioClip STARTClip;

	[SerializeField]
	private float START_Volume;

	[Header("__________ Mini Boss")]
	public Boss_Mini_MaraDevil leftMiniBoss;

	public Boss_Mini_MaraDevil rightMiniBoss;

	[SerializeField]
	[Header("============ State ===============")]
	private Boss_MaraDevil.EState bossState;

	private bool changeState;

	private bool doneATTACK_1;

	private bool doneATTACK_2;

	private bool eventAttack_2;

	private bool doneATTACK_3;

	private bool doneDIE;

	private bool doneHIT;

	private bool doneIDLE;

	private bool doneRUN;

	private bool doneSTART;

	private bool doneSTART_OPEN;

	private int hit;

	private enum EState
	{
		ATTACK_1,
		ATTACK_2,
		ATTACK_3,
		DIE,
		HIT,
		IDLE,
		RUN,
		START,
		START_OPEN,
		COMBO_ATTACK_1,
		COMBO_ATTACK_2,
		COMBO_ATTACK_3,
		FAST_RUN,
		COMBO_ATTACK_1_2
	}
}
