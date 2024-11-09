using System;
using System.Collections;
using System.Collections.Generic;
using Com.LuisPedroFonseca.ProCamera2D;
using MyDataLoader;
using Newtonsoft.Json;
using Spine;
using Spine.Unity;
using UnityEngine;

public class Boss_Sunray : BaseBoss
{
	public override void Init()
	{
		this.isInit = false;
		this.mainCamera = Camera.main;
		this.GetGameMode();
		base.Init();
		this.InitEnemy();
		this.attackArr = new int[]
		{
			0,
			1,
			2,
			3,
			9
		};
		this.afterDisappearArr = new int[]
		{
			11,
			12,
			13
		};
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
		this.bossState = Boss_Sunray.EState.START;
		this.ClearDoneFlag();
		this.UpdateBossState();
		this.ramboTransform = GameManager.Instance.player.transform;
		this.meshRenderer.sortingOrder = this.afterInitOrderInLayer;
		this.meshRenderer.sortingLayerName = this.afterInitLayer;
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
		this.gameMode = GameMode.Instance.EMode;
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
		this.UpdateBossPosition();
		this.UpdateBossRotation();
		this.UpdateBullets();
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

	private void ChangeAttackFrequence()
	{
		if (this.HP > this.cacheEnemy.HP * 0.67f)
		{
			this.attackFrequency = 80;
		}
		else if (this.HP < this.cacheEnemy.HP * 0.67f && this.HP > this.cacheEnemy.HP * 0.33f)
		{
			this.attackFrequency = 90;
		}
		else
		{
			this.attackFrequency = 100;
		}
	}

	private void CreateMovePath(int pathCount, float xMin, float yMin, float xMax, float yMax, float z)
	{
		this.movePath = new Vector3[pathCount];
		for (int i = 0; i < pathCount; i++)
		{
			this.movePath[i] = new Vector3(UnityEngine.Random.Range(xMin, xMax), UnityEngine.Random.Range(yMin, yMax), z);
		}
	}

	private void CreateMoveLine(Vector3 start, Vector3 end)
	{
		this.movePath = new Vector3[2];
		this.movePath[0] = start;
		this.movePath[1] = end;
	}

	private void UpdateBossPosition()
	{
		if (this.bossState != Boss_Sunray.EState.ATTACK_MINI)
		{
			this.transform.position = Vector3.MoveTowards(this.transform.position, this.targetRun, this.moveSpeedPercent * Time.fixedDeltaTime * this.bossData.enemy[0].Speed);
		}
	}

	private void UpdateBossRotation()
	{
		this.transform.eulerAngles = new Vector3(0f, 0f, this.rotationAngle);
	}

	private void GetBone()
	{
		this.gunTipBones = new Bone[this.gunTipBoneNames.Length];
		for (int i = 0; i < this.gunTipBoneNames.Length; i++)
		{
			this.gunTipBones[i] = this.skeletonAnimation.skeleton.FindBone(this.gunTipBoneNames[i]);
		}
		this.boneCenter = this.skeletonAnimation.skeleton.FindBone(this.boneCenterName);
	}

	private void UpdateCachePosition()
	{
		if (GameManager.Instance.player.IsInVisible())
		{
			this.ramboPosition = new Vector3(UnityEngine.Random.Range(this.topLeftCameraPos.x, this.topRightCameraPos.x), this.ramboTransform.position.y, this.ramboTransform.position.z);
		}
		else
		{
			this.ramboPosition = this.ramboTransform.position;
		}
	}

	private void UpdateCameraCornerPosition()
	{
		this.bottomLeftCameraPos = this.mainCamera.ScreenToWorldPoint(new Vector3(0f, 0f, this.mainCamera.nearClipPlane));
		this.topLeftCameraPos = this.mainCamera.ScreenToWorldPoint(new Vector3(0f, (float)this.mainCamera.pixelHeight, this.mainCamera.nearClipPlane));
		this.bottomRightCameraPos = this.mainCamera.ScreenToWorldPoint(new Vector3((float)this.mainCamera.pixelWidth, 0f, this.mainCamera.nearClipPlane));
		this.topRightCameraPos = this.mainCamera.ScreenToWorldPoint(new Vector3((float)this.mainCamera.pixelWidth, (float)this.mainCamera.pixelHeight, this.mainCamera.nearClipPlane));
		this.bottomLeftCameraPos += new Vector3(1f, 1f, -this.bottomLeftCameraPos.z + this.transform.position.z);
		this.topLeftCameraPos += new Vector3(1f, -1f, -this.topLeftCameraPos.z + this.transform.position.z);
		this.bottomRightCameraPos += new Vector3(-1f, 1f, -this.bottomRightCameraPos.z + this.transform.position.z);
		this.topRightCameraPos += new Vector3(-1f, -1f, -this.topRightCameraPos.z + this.transform.position.z);
	}

	private void UpdateBossState()
	{
		if (this.State == ECharactor.DIE)
		{
			return;
		}
		switch (this.bossState)
		{
		case Boss_Sunray.EState.ATTACK_MINI:
			this.OnATTACK_MINI();
			break;
		case Boss_Sunray.EState.ATTACK_LAN:
			this.OnATTACK_LAN();
			break;
		case Boss_Sunray.EState.ATTACK_NHA_DAN:
			this.OnATTACK_NHA_DAN();
			break;
		case Boss_Sunray.EState.ATTACK_XOAY_1:
			this.OnATTACK_XOAY_1();
			break;
		case Boss_Sunray.EState.ATTACK_XOAY_2:
			this.OnATTACK_XOAY_2();
			break;
		case Boss_Sunray.EState.DIE:
			this.OnDIE();
			break;
		case Boss_Sunray.EState.HIT:
			this.OnHIT();
			break;
		case Boss_Sunray.EState.IDLE:
			this.OnIDLE();
			break;
		case Boss_Sunray.EState.START:
			this.OnSTART();
			break;
		case Boss_Sunray.EState.BIEN_MAT:
			this.OnBIEN_MAT();
			break;
		case Boss_Sunray.EState.XUAT_HIEN:
			this.OnXUAT_HIEN();
			break;
		case Boss_Sunray.EState.ATTACK_LAN_NGANG:
			this.OnATTACK_LAN_NGANG();
			break;
		case Boss_Sunray.EState.ATTACK_LAN_DOC:
			this.OnATTACK_LAN_DOC();
			break;
		case Boss_Sunray.EState.ATTACK_LAN_CHEO:
			this.OnATTACK_LAN_CHEO();
			break;
		default:
			this.OnIDLE();
			break;
		}
	}

	private void OnATTACK_MINI()
	{
		this.PlayAnim(Boss_Sunray.EState.ATTACK_MINI, false, 1f);
		this.PlaySound(this.ATTACK_MINI_Clip, this.ATTACK_MINI_Volume);
	}

	private void OnATTACK_LAN()
	{
		if (this.gameMode == GameMode.Mode.NORMAL)
		{
			float y = UnityEngine.Random.Range(this.ramboPosition.y + 1f, this.bottomLeftCameraPos.y);
			Vector3 vector = new Vector3(this.topLeftCameraPos.x, y, this.transform.position.z);
			Vector3 vector2 = new Vector3(this.topRightCameraPos.x, y, this.transform.position.z);
			if (Mathf.Abs(this.transform.position.x - this.topLeftCameraPos.x) < Mathf.Abs(this.transform.position.x - this.topRightCameraPos.x))
			{
				this.CreateMoveLine(vector, vector2);
			}
			else
			{
				this.CreateMoveLine(vector2, vector);
			}
		}
		else
		{
			this.CreateMovePath(this.moveStepCountATTACK_LAN, this.bottomLeftCameraPos.x, this.bottomLeftCameraPos.y, this.topRightCameraPos.x, this.topRightCameraPos.y, this.transform.position.z);
		}
		this.moveSpeedPercent = UnityEngine.Random.Range(1f, this.moveSpeedPercentATTACK_LAN);
		this.moveStep = 0;
		this.targetRun = this.movePath[this.moveStep];
		this.PlayAnim(Boss_Sunray.EState.ATTACK_LAN, true, 1f);
		this.PlayLoopSound(this.ATTACK_LAN_Clip, this.ATTACK_LAN_Volume, 1f);
		this.ActiveAllAttackBox(this.damagePercentATTACK_LAN);
	}

	private void OnATTACK_LAN_NGANG()
	{
		Vector3 end = new Vector3((this.transform.position.x >= this.ramboPosition.x) ? (this.topLeftCameraPos.x - 3f) : (this.topRightCameraPos.x + 3f), this.transform.position.y, this.transform.position.z);
		this.CreateMoveLine(this.transform.position, end);
		this.moveSpeedPercent = this.moveSpeedPercentATTACK_LAN_NGANG;
		this.moveStep = 0;
		this.targetRun = this.movePath[this.moveStep];
		this.PlayAnim(Boss_Sunray.EState.ATTACK_LAN, true, 1f);
		this.PlayLoopSound(this.ATTACK_LAN_Clip, this.ATTACK_LAN_Volume, 1f);
		this.ActiveAllAttackBox(this.damagePercentATTACK_LAN_NGANG);
	}

	private void OnATTACK_LAN_DOC()
	{
		Vector3 end = new Vector3(this.transform.position.x, (this.transform.position.y >= this.ramboPosition.y) ? (this.bottomLeftCameraPos.y - 3f) : (this.topLeftCameraPos.y + 3f), this.transform.position.z);
		this.CreateMoveLine(this.transform.position, end);
		this.moveSpeedPercent = this.moveSpeedPercentATTACK_LAN_DOC;
		this.moveStep = 0;
		this.targetRun = this.movePath[this.moveStep];
		this.PlayAnim(Boss_Sunray.EState.ATTACK_LAN, true, 1f);
		this.PlayLoopSound(this.ATTACK_LAN_Clip, this.ATTACK_LAN_Volume, 1f);
		this.ActiveAllAttackBox(this.damagePercentATTACK_LAN_DOC);
	}

	private void OnATTACK_LAN_CHEO()
	{
		this.CreateMoveLine(this.transform.position, this.endCheo);
		this.moveSpeedPercent = this.moveSpeedPercentATTACK_LAN_CHEO;
		this.moveStep = 0;
		this.targetRun = this.movePath[this.moveStep];
		this.PlayAnim(Boss_Sunray.EState.ATTACK_LAN, true, 1f);
		this.PlayLoopSound(this.ATTACK_LAN_Clip, this.ATTACK_LAN_Volume, 1f);
		this.ActiveAllAttackBox(this.damagePercentATTACK_LAN_CHEO);
	}

	private void OnATTACK_NHA_DAN()
	{
		this.CreateMovePath(this.moveStepCountATTACK_NHA_DAN, this.bottomLeftCameraPos.x, this.bottomLeftCameraPos.y, this.topRightCameraPos.x, this.topRightCameraPos.y, this.transform.position.z);
		this.moveSpeedPercent = UnityEngine.Random.Range(1f, this.moveSpeedPercentATTACK_NHA_DAN);
		this.moveStep = 0;
		this.targetRun = this.movePath[this.moveStep];
		this.PlayAnim(Boss_Sunray.EState.ATTACK_NHA_DAN, false, 1f);
		this.PlaySound(this.ATTACK_NHA_DAN_Clip, this.ATTACK_NHA_DAN_Volume);
	}

	private void OnATTACK_XOAY_1()
	{
		this.PlayAnim(Boss_Sunray.EState.ATTACK_XOAY_1, false, 1f);
		this.PlaySound(this.ATTACK_XOAY_1_Clip, this.ATTACK_XOAY_1_Volume);
		this.FX.triggerATTACK_XOAY = true;
		this.ActiveAllAttackBox(this.damagePercentATTACK_XOAY);
	}

	private void OnATTACK_XOAY_2()
	{
		if (this.gameMode == GameMode.Mode.NORMAL)
		{
			float y = UnityEngine.Random.Range(this.ramboPosition.y + 3f, this.topLeftCameraPos.y);
			Vector3 vector = new Vector3(this.topLeftCameraPos.x, y, this.transform.position.z);
			Vector3 vector2 = new Vector3(this.topRightCameraPos.x, y, this.transform.position.z);
			if (Mathf.Abs(this.transform.position.x - this.topLeftCameraPos.x) < Mathf.Abs(this.transform.position.x - this.topRightCameraPos.x))
			{
				this.CreateMoveLine(vector, vector2);
			}
			else
			{
				this.CreateMoveLine(vector2, vector);
			}
		}
		else
		{
			this.CreateMovePath(this.moveStepCountATTACK_XOAY, this.bottomLeftCameraPos.x, this.bottomLeftCameraPos.y, this.topRightCameraPos.x, this.topRightCameraPos.y, this.transform.position.z);
		}
		this.moveSpeedPercent = UnityEngine.Random.Range(1f, this.moveSpeedPercentATTACK_XOAY);
		this.moveStep = 0;
		this.targetRun = this.movePath[this.moveStep];
		float num = UnityEngine.Random.Range(0.5f, 1.5f);
		this.PlayAnim(Boss_Sunray.EState.ATTACK_XOAY_2, true, num);
		this.PlayLoopSound(this.ATTACK_XOAY_2_Clip, this.ATTACK_XOAY_2_Volume, num);
	}

	private void OnDIE()
	{
		this.bossState = Boss_Sunray.EState.DIE;
		this.PlayAnim(Boss_Sunray.EState.DIE, false, 1f);
		SingletonGame<AudioController>.Instance.StopSound();
		this.PlaySound(this.DIE_Clip, 1f);
	}

	private void OnHIT()
	{
		this.ChangeAttackFrequence();
		this.PlayAnim(Boss_Sunray.EState.HIT, 2);
	}

	private void OnIDLE()
	{
		this.rotationAngle = 0f;
		this.PlayAnim(Boss_Sunray.EState.IDLE, false, 1f);
		SingletonGame<AudioController>.Instance.StopSound();
		this.DeactiveAllAttackBox();
	}

	private void OnSTART()
	{
		this.PlayAnim(Boss_Sunray.EState.START, false, 1f);
		this.PlaySound(this.START_Clip, this.START_Volume);
	}

	private void OnBIEN_MAT()
	{
		this.FX.triggerBIEN_MAT = true;
		this.nextState = (Boss_Sunray.EState)this.afterDisappearArr[UnityEngine.Random.Range(0, 100) % this.afterDisappearArr.Length];
		this.CreateMovePath(this.moveStepCountBIEN_MAT, this.bottomLeftCameraPos.x, this.bottomLeftCameraPos.y, this.topRightCameraPos.x, this.topRightCameraPos.y, this.transform.position.z);
		if (this.nextState == Boss_Sunray.EState.ATTACK_LAN_NGANG)
		{
			this.movePath[this.movePath.Length - 1] = this.RandomXuatHienNgang();
		}
		else if (this.nextState == Boss_Sunray.EState.ATTACK_LAN_DOC)
		{
			this.movePath[this.movePath.Length - 1] = this.RandomXuatHienDoc();
		}
		else if (this.nextState == Boss_Sunray.EState.ATTACK_LAN_CHEO)
		{
			Vector3[] array = this.RandomXuatHienCheo();
			this.movePath[this.movePath.Length - 1] = array[0];
			this.endCheo = array[1];
			Vector3 vector = array[1] - array[0];
			this.nextRotationAngle = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			if (this.nextRotationAngle < -90f || this.nextRotationAngle > 90f)
			{
				this.nextRotationAngle += 180f;
			}
		}
		this.moveSpeedPercent = 0f;
		this.moveStep = 0;
		this.targetRun = this.movePath[this.moveStep];
		this.ActiveAllAttackBox(this.damagePercentBIEN_MAT);
	}

	private Vector3 RandomXuatHienNgang()
	{
		Vector3[] array = new Vector3[]
		{
			new Vector3(this.topLeftCameraPos.x, UnityEngine.Random.Range(this.bottomLeftCameraPos.y, this.topLeftCameraPos.y), this.transform.position.z),
			new Vector3(this.topRightCameraPos.x, UnityEngine.Random.Range(this.bottomLeftCameraPos.y, this.topLeftCameraPos.y), this.transform.position.z)
		};
		return array[UnityEngine.Random.Range(0, 100) % array.Length];
	}

	private Vector3 RandomXuatHienDoc()
	{
		Vector3[] array = new Vector3[]
		{
			new Vector3(this.ramboPosition.x + UnityEngine.Random.Range(-2f, 2f), this.bottomLeftCameraPos.y, this.transform.position.z),
			new Vector3(this.ramboPosition.x + UnityEngine.Random.Range(-2f, 2f), this.topLeftCameraPos.y, this.transform.position.z)
		};
		return array[UnityEngine.Random.Range(0, 100) % array.Length];
	}

	private Vector3 RandomXuatHienPosition()
	{
		Vector3[] array = new Vector3[]
		{
			this.RandomXuatHienNgang(),
			this.RandomXuatHienDoc()
		};
		return array[UnityEngine.Random.Range(0, 100) % array.Length];
	}

	private Vector3[] RandomXuatHienCheo()
	{
		Vector3[][] array = new Vector3[][]
		{
			new Vector3[]
			{
				this.topLeftCameraPos,
				this.bottomRightCameraPos
			},
			new Vector3[]
			{
				this.bottomLeftCameraPos,
				this.topRightCameraPos
			},
			new Vector3[]
			{
				this.topRightCameraPos,
				this.bottomLeftCameraPos
			},
			new Vector3[]
			{
				this.bottomRightCameraPos,
				this.topLeftCameraPos
			}
		};
		return array[UnityEngine.Random.Range(0, 100) % array.Length];
	}

	private void OnXUAT_HIEN()
	{
		this.FX.triggerXUAT_HIEN = true;
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
		case Boss_Sunray.EState.ATTACK_MINI:
			if (this.TriggerIDLE())
			{
				this.bossState = Boss_Sunray.EState.IDLE;
				flag = true;
			}
			goto IL_2AD;
		case Boss_Sunray.EState.ATTACK_LAN:
		case Boss_Sunray.EState.ATTACK_XOAY_2:
		case Boss_Sunray.EState.ATTACK_LAN_NGANG:
		case Boss_Sunray.EState.ATTACK_LAN_DOC:
		case Boss_Sunray.EState.ATTACK_LAN_CHEO:
			if (this.DoneMovePath())
			{
				this.bossState = Boss_Sunray.EState.IDLE;
				flag = true;
			}
			goto IL_2AD;
		case Boss_Sunray.EState.ATTACK_NHA_DAN:
			if (this.TriggerIDLE())
			{
				this.bossState = Boss_Sunray.EState.IDLE;
				flag = true;
			}
			goto IL_2AD;
		case Boss_Sunray.EState.ATTACK_XOAY_1:
			if (this.TriggerATTACK_XOAY_2())
			{
				this.bossState = Boss_Sunray.EState.ATTACK_XOAY_2;
				flag = true;
			}
			goto IL_2AD;
		case Boss_Sunray.EState.IDLE:
			if (this.TriggerIDLE())
			{
				if (UnityEngine.Random.Range(0, 100) < this.attackFrequency)
				{
					this.bossState = (Boss_Sunray.EState)this.attackArr[UnityEngine.Random.Range(0, 100) % this.attackArr.Length];
				}
				else
				{
					this.bossState = Boss_Sunray.EState.IDLE;
				}
				flag = true;
			}
			goto IL_2AD;
		case Boss_Sunray.EState.START:
			if (this.TriggerIDLE())
			{
				this.bossState = Boss_Sunray.EState.IDLE;
				flag = true;
			}
			goto IL_2AD;
		case Boss_Sunray.EState.BIEN_MAT:
			if (this.FX.eventBIEN_MAT)
			{
				this.meshRenderer.enabled = false;
				this.HideBoxBoss();
				this.FX.eventBIEN_MAT = false;
				this.PlayAnim(Boss_Sunray.EState.ATTACK_LAN, true, 1f);
			}
			if (this.FX.doneBIEN_MAT)
			{
				this.moveSpeedPercent = this.moveSpeedPercentBIEN_MAT;
				this.FX.doneBIEN_MAT = false;
				if (this.nextState == Boss_Sunray.EState.ATTACK_LAN_NGANG)
				{
					this.rotationAngle = 0f;
				}
				else if (this.nextState == Boss_Sunray.EState.ATTACK_LAN_DOC)
				{
					this.rotationAngle = 90f;
				}
				else if (this.nextState == Boss_Sunray.EState.ATTACK_LAN_CHEO)
				{
					this.rotationAngle = this.nextRotationAngle;
				}
			}
			if (this.ReachedToTarget(this.transform.position, this.targetRun))
			{
				this.moveStep++;
				if (this.moveStep < this.movePath.Length)
				{
					this.targetRun = this.movePath[this.moveStep];
				}
				else
				{
					this.bossState = Boss_Sunray.EState.XUAT_HIEN;
					flag = true;
				}
			}
			goto IL_2AD;
		case Boss_Sunray.EState.XUAT_HIEN:
			if (this.FX.eventXUAT_HIEN)
			{
				this.meshRenderer.enabled = true;
				this.FX.eventXUAT_HIEN = false;
				this.ShowBoxBoss();
			}
			if (this.FX.doneXUAT_HIEN)
			{
				this.bossState = this.nextState;
				this.FX.doneXUAT_HIEN = false;
				flag = true;
			}
			goto IL_2AD;
		}
		flag = false;
		IL_2AD:
		if (flag)
		{
			this.ClearDoneFlag();
		}
		return flag;
	}

	private bool DoneMovePath()
	{
		bool result = false;
		if (this.ReachedToTarget(this.transform.position, this.targetRun))
		{
			this.moveStep++;
			if (this.moveStep < this.movePath.Length)
			{
				this.targetRun = this.movePath[this.moveStep];
				result = false;
			}
			else
			{
				result = true;
			}
		}
		return result;
	}

	private bool TriggerIDLE()
	{
		return this.doneATTACK_MINI || this.doneATTACK_LAN || this.doneATTACK_NHA_DAN || this.doneATTACK_XOAY_2 || this.doneIDLE || this.doneSTART;
	}

	private bool TriggerATTACK_XOAY_2()
	{
		return this.doneATTACK_XOAY_1;
	}

	private bool ReachedToTarget(Vector3 curPos, Vector3 target)
	{
		return Vector2.Distance(curPos, target) < 0.1f;
	}

	private void ClearDoneFlag()
	{
		this.doneATTACK_MINI = false;
		this.doneATTACK_LAN = false;
		this.doneATTACK_NHA_DAN = false;
		this.doneATTACK_XOAY_1 = false;
		this.doneATTACK_XOAY_2 = false;
		this.doneSTART = false;
		this.doneIDLE = false;
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
			if (!(name == "attack-mini"))
			{
				if (name == "attack_nha_dan")
				{
					this.eventATTACK_NHA_DAN = true;
					for (int i = 0; i < this.gunTipBones.Length; i++)
					{
						Vector2 direction = new Vector2(this.gunTipBones[i].WorldX - this.boneCenter.WorldX, this.gunTipBones[i].WorldY - this.boneCenter.WorldY);
						Vector2 pos = new Vector2(this.gunTipBones[i].WorldX + this.transform.position.x, this.gunTipBones[i].WorldY + this.transform.position.y);
						this.CreateBullet(pos, direction);
					}
				}
			}
			else
			{
				this.FX.triggerATTACK_MINI = true;
				for (int j = 0; j < this.miniBoss.Length; j++)
				{
					this.miniBoss[j].triggerMiniAttack = true;
				}
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
		case "attack-mini":
			this.doneATTACK_MINI = true;
			break;
		case "attack_lan":
			this.doneATTACK_LAN = true;
			break;
		case "attack_nha_dan":
			this.doneATTACK_NHA_DAN = true;
			break;
		case "attack_xoay_1":
			this.doneATTACK_XOAY_1 = true;
			break;
		case "attack_xoay_2":
			this.doneATTACK_XOAY_2 = true;
			break;
		case "idle":
			this.doneIDLE = true;
			break;
		case "start":
			this.doneSTART = true;
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

	private void PlayAnim(Boss_Sunray.EState state, bool loop = false, float speedAnim = 1f)
	{
		this.skeletonAnimation.timeScale = speedAnim;
		this.skeletonAnimation.AnimationState.SetAnimation(0, this.animations[(int)state], loop);
	}

	private void PlayAnim(Boss_Sunray.EState state, int order)
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

	private void HideBoxBoss()
	{
	}

	private void ShowBoxBoss()
	{
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
				UIShowInforManager.Instance.ShowUnlock("Boss Sunray has been unlocked in BossMode!");
			}
			return;
		}
		this.OnHIT();
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
			UIShowInforManager.Instance.ShowUnlock("Boss Sunray has been unlocked in BossMode!");
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

	private void PlayLoopSound(AudioClip sound, float volume = 1f, float speed = 1f)
	{
		try
		{
			SingletonGame<AudioController>.Instance.PlayLoopSound(sound, volume, speed);
		}
		catch
		{
		}
	}

	private void InitBullets()
	{
		this.BulletPool = new ObjectPooling<BulletBoss4_1>(this.ListBullet.Count, null, null);
		this.ListBullet[0].gameObject.transform.parent.parent = null;
		for (int i = 0; i < this.ListBullet.Count; i++)
		{
			this.BulletPool.Store(this.ListBullet[i]);
		}
	}

	private void UpdateBullets()
	{
		for (int i = 0; i < this.ListBullet.Count; i++)
		{
			if (this.ListBullet[i] != null && this.ListBullet[i].gameObject.activeSelf)
			{
				this.ListBullet[i].UpdateObject();
			}
		}
	}

	private void CreateBullet(Vector2 pos, Vector2 direction)
	{
		BulletBoss4_1 bulletBoss4_ = this.BulletPool.New();
		if (bulletBoss4_ == null)
		{
			bulletBoss4_ = UnityEngine.Object.Instantiate<Transform>(this.ListBullet[0].transform).GetComponent<BulletBoss4_1>();
			bulletBoss4_.transform.parent = this.ListBullet[0].transform.parent;
			this.ListBullet.Add(bulletBoss4_);
		}
		bulletBoss4_.gameObject.SetActive(true);
		bulletBoss4_.Shoot(pos, direction, this.cacheEnemy.Damage, this.cacheEnemy.Speed_Bullet, delegate(BulletBoss4_1 b)
		{
			this.BulletPool.Store(b);
		});
	}

	private void ActiveAllAttackBox(float damagePercent)
	{
		this.Attack_Box_Body.Active(damagePercent * this.cacheEnemy.Damage, true, null);
		this.Attack_Box_Bua_1.Active(damagePercent * this.cacheEnemy.Damage, true, null);
		this.Attack_Box_Bua_2.Active(damagePercent * this.cacheEnemy.Damage, true, null);
	}

	private void DeactiveAllAttackBox()
	{
		this.Attack_Box_Body.Deactive();
		this.Attack_Box_Bua_1.Deactive();
		this.Attack_Box_Bua_2.Deactive();
	}

	[Header("**************** Boss_Sunray ***************")]
	[SerializeField]
	private TextAsset bossDataText;

	private EnemyCharactor bossData;

	private GameMode.Mode gameMode;

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

	private Camera mainCamera;

	private Vector3 topLeftCameraPos;

	private Vector3 topRightCameraPos;

	private Vector3 bottomLeftCameraPos;

	private Vector3 bottomRightCameraPos;

	public Boss_Sunray_FX FX;

	[Range(0f, 100f)]
	[SerializeField]
	[Header("============ ATTACK ===============")]
	private int attackFrequency;

	private int[] attackArr;

	private int[] afterDisappearArr;

	private Transform ramboTransform;

	private Vector3 ramboPosition;

	private float rotationAngle;

	private Vector3 targetRun;

	private Vector3[] movePath;

	private float moveSpeedPercent;

	private int moveStep;

	[Header("__________ ATTACK_BOX")]
	[SerializeField]
	private AttackBox Attack_Box_Body;

	[SerializeField]
	private AttackBox Attack_Box_Bua_1;

	[SerializeField]
	private AttackBox Attack_Box_Bua_2;

	[SerializeField]
	[Header("__________ START")]
	private AudioClip START_Clip;

	[SerializeField]
	private float START_Volume;

	[SerializeField]
	[Header("__________ DIE")]
	private AudioClip DIE_Clip;

	[SerializeField]
	private float DIE_Volume;

	[Header("__________ ATTACK_MINI")]
	public Boss_MiniSunray[] miniBoss;

	[SerializeField]
	private int moveStepCountATTACK_MINI;

	[SerializeField]
	private float moveSpeedPercentATTACK_MINI;

	[SerializeField]
	private AudioClip ATTACK_MINI_Clip;

	[SerializeField]
	private float ATTACK_MINI_Volume;

	[Header("__________ ATTACK_LAN")]
	[SerializeField]
	private int moveStepCountATTACK_LAN;

	[SerializeField]
	private float moveSpeedPercentATTACK_LAN;

	[SerializeField]
	private float damagePercentATTACK_LAN;

	[SerializeField]
	private AudioClip ATTACK_LAN_Clip;

	[SerializeField]
	private float ATTACK_LAN_Volume;

	[Header("__________ ATTACK_LAN_NGANG")]
	[SerializeField]
	private float moveSpeedPercentATTACK_LAN_NGANG;

	[SerializeField]
	private float damagePercentATTACK_LAN_NGANG;

	[SerializeField]
	[Header("__________ ATTACK_LAN_DOC")]
	private float moveSpeedPercentATTACK_LAN_DOC;

	[SerializeField]
	private float damagePercentATTACK_LAN_DOC;

	[SerializeField]
	[Header("__________ ATTACK_LAN_CHEO")]
	private float moveSpeedPercentATTACK_LAN_CHEO;

	[SerializeField]
	private float damagePercentATTACK_LAN_CHEO;

	private Vector3 endCheo;

	private float nextRotationAngle;

	[SerializeField]
	[Header("__________ ATTACK_NHA_DAN")]
	private int moveStepCountATTACK_NHA_DAN;

	[SerializeField]
	private float moveSpeedPercentATTACK_NHA_DAN;

	[SerializeField]
	private string[] gunTipBoneNames;

	[SerializeField]
	private string boneCenterName;

	private Bone[] gunTipBones;

	private Bone boneCenter;

	[Header("Bullet")]
	public List<BulletBoss4_1> ListBullet;

	public ObjectPooling<BulletBoss4_1> BulletPool;

	[SerializeField]
	private AudioClip ATTACK_NHA_DAN_Clip;

	[SerializeField]
	private float ATTACK_NHA_DAN_Volume;

	[SerializeField]
	[Header("__________ ATTACK_XOAY")]
	private int moveStepCountATTACK_XOAY;

	[SerializeField]
	private float moveSpeedPercentATTACK_XOAY;

	[SerializeField]
	private float damagePercentATTACK_XOAY;

	[SerializeField]
	private AudioClip ATTACK_XOAY_1_Clip;

	[SerializeField]
	private float ATTACK_XOAY_1_Volume;

	[SerializeField]
	private AudioClip ATTACK_XOAY_2_Clip;

	[SerializeField]
	private float ATTACK_XOAY_2_Volume;

	[SerializeField]
	[Header("__________ BIEN_MAT")]
	private int moveStepCountBIEN_MAT;

	[SerializeField]
	private float moveSpeedPercentBIEN_MAT;

	[SerializeField]
	private float damagePercentBIEN_MAT;

	[SerializeField]
	[Header("============ STATE ===============")]
	[Header("__________ XUAT_HIEN")]
	private Boss_Sunray.EState bossState;

	private Boss_Sunray.EState nextState;

	private bool changeState;

	private bool doneATTACK_MINI;

	private bool doneATTACK_LAN;

	private bool doneATTACK_NHA_DAN;

	private bool doneATTACK_XOAY_1;

	private bool doneATTACK_XOAY_2;

	private bool doneSTART;

	private bool doneIDLE;

	private bool eventATTACK_NHA_DAN;

	private int hit;

	private enum EState
	{
		ATTACK_MINI,
		ATTACK_LAN,
		ATTACK_NHA_DAN,
		ATTACK_XOAY_1,
		ATTACK_XOAY_2,
		DIE,
		HIT,
		IDLE,
		START,
		BIEN_MAT,
		XUAT_HIEN,
		ATTACK_LAN_NGANG,
		ATTACK_LAN_DOC,
		ATTACK_LAN_CHEO
	}
}
