using System;
using System.Collections.Generic;
using SpawnEnemy;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	public static EnemyManager Instance
	{
		get
		{
			if (EnemyManager.instance == null)
			{
				EnemyManager.instance = UnityEngine.Object.FindObjectOfType<EnemyManager>();
			}
			return EnemyManager.instance;
		}
	}

	public void OnInit()
	{
		this.enemyRandom = new EnemyRandom();
		for (int i = 0; i < this.Spawns.Length; i++)
		{
			this.Spawns[i].OnInit();
			if (GameMode.Instance.modePlay == GameMode.ModePlay.Endless)
			{
				this.Spawns[i].SetLevelEnemy(0);
			}
		}
		this.ListDayDu = new List<DayDu>();
		this.ListSniper = new List<EnemySniper>();
		this.ListAkm = new List<EnemyAkm>();
		this.ListEnemyPistol = new List<EnemyPistol>();
		this.ListEnemyGrenade = new List<EnemyGrenade>();
		this.ListEnemyKnife = new List<EnemyKnife>();
		this.ListEnemyFire = new List<EnemyFire>();
		this.ListEnemyRocket = new List<EnemyRocket>();
		this.ListEnemyMiniGun = new List<EnemyMiniGun>();
		this.PoolEnemyMiniGun = new ObjectPooling<EnemyMiniGun>(0, null, null);
		this.ListTank = new List<Tank_1>();
		this.PoolTank = new ObjectPooling<Tank_1>(0, null, null);
		this.ListTank2 = new List<Tank_2>();
		this.PoolTank2 = new ObjectPooling<Tank_2>(0, null, null);
		this.ListTank3 = new List<Tank_3>();
		this.PoolTank3 = new ObjectPooling<Tank_3>(0, null, null);
		this.ListHelicopter = new List<HelicopterAttack>();
		this.ListAirplane = new List<Airplane>();
		this.PoolTrack = new ObjectPooling<EnemyTrack>(this.ListTrack.Count, null, null);
		for (int j = 0; j < this.ListTrack.Count; j++)
		{
			this.PoolTrack.Store(this.ListTrack[j]);
		}
		this.ListEnemy330 = new List<Enemy33>();
		this.ListEnemy350 = new List<Enemy35>();
		this.ListBee3 = new List<Bee3>();
		this.ListSungCoi = new List<EnemySungCoi>();
		this.ListEnemyDrone = new List<EnemyDrone>();
		this.ListEnemyFlyMan = new List<Enemy_FlyMan>();
		this.ListBeSung = new List<BeSung>();
		this.PoolBeSung = new ObjectPooling<BeSung>(0, null, null);
		this.ListBeSung2 = new List<BeSung2>();
		this.PoolBeSung2 = new ObjectPooling<BeSung2>(0, null, null);
		this.ListBugBomb = new List<BugBomb>();
		this.ListRocketMouse = new List<ChuotRocket>();
		this.ListEnemyThuyen = new List<EnemyThuyen>();
		this.ListUAV = new List<UAV>();
		this.ListNhenNhay = new List<NhenNhay>();
		this.ListEnemySau = new List<EnemySau>();
		this.ListSpaceShip = new List<SpaceShip>();
		this.ListMiniShip = new List<MiniBoss5_2>();
		this.ListMiniSpider = new List<NhenNho>();
		this.ListEnemyRiu = new List<EnemyRiu>();
		this.ListMiniGun2 = new List<EnemyMiniGun2>();
		this.PoolMiniGun2 = new ObjectPooling<EnemyMiniGun2>(0, null, null);
		this.ListZombieKnife = new List<ZombieKnife>();
		this.PoolZombieKnife = new ObjectPooling<ZombieKnife>(0, null, null);
		this.ListZombieRobot = new List<ZombieRobot>();
		this.PoolZombieRobot = new ObjectPooling<ZombieRobot>(0, null, null);
		this.IsInit = true;
	}

	public void OnUpdate(float deltaTime)
	{
		if (!this.IsInit)
		{
			return;
		}
		if (GameManager.Instance.StateManager.EState == EGamePlay.RUNNING)
		{
			this.enemyRandom.OnUpdate(deltaTime);
			for (int i = 0; i < this.Spawns.Length; i++)
			{
				this.Spawns[i].OnUpdate(deltaTime);
			}
		}
		if (GameManager.Instance.StateManager.EState != EGamePlay.NONE)
		{
			this.UpdateSniper(deltaTime);
			this.UpdateEnemyAkm(deltaTime);
			this.UpdateEnemyPistol(deltaTime);
			this.UpdateEnemyGrenade(deltaTime);
			this.UpdateEnemyKnife(deltaTime);
			this.UpdateEnemyFire(deltaTime);
			this.UpdateEnemyRocket(deltaTime);
			this.UpdateEnemyMiniGun(deltaTime);
			this.UpdateTank_1(deltaTime);
			this.UpdateTank2(deltaTime);
			this.UpdateTank3(deltaTime);
			this.UpdateHelicopter();
			this.UpdateAirplane();
			this.UpdateTrack();
			this.UpdateEnemy330();
			this.UpdateEnemy350();
			this.UpdateEnemySungCoi(deltaTime);
			this.UpdateEnemyDrone(deltaTime);
			this.UpdateBugBomb(deltaTime);
			this.UpdateRocketMouses(deltaTime);
			this.UpdateEnemyThuyen(deltaTime);
			this.UpdateUAV(deltaTime);
			this.UpdateBee3(deltaTime);
			this.UpdateNhenNhay(deltaTime);
			this.UpdateSau(deltaTime);
			this.UpdateSpaceShip(deltaTime);
			this.UpdateMiniShip();
			this.UpdateMiniSpider(deltaTime);
			this.UpdateDayDu(deltaTime);
			this.UpdateEnemyFlyMan(deltaTime);
			this.UpdateBeSung(deltaTime);
			this.UpdateBeSung2(deltaTime);
			this.UpdateEnemyRiu(deltaTime);
			this.UpdateEnemyMiniGun2(deltaTime);
			this.UpdateZombieKnife(deltaTime);
			this.UpdateZombieRobot(deltaTime);
		}
		GameMode.GameStyle style = GameMode.Instance.Style;
		if (style != GameMode.GameStyle.SinglPlayer)
		{
			if (style != GameMode.GameStyle.MultiPlayer)
			{
			}
		}
		else
		{
			GameMode.ModePlay modePlay = GameMode.Instance.modePlay;
			if (modePlay == GameMode.ModePlay.Campaign || modePlay == GameMode.ModePlay.Endless || modePlay == GameMode.ModePlay.Special_Campaign)
			{
				if (GameMode.Instance.MODE == GameMode.Mode.TUTORIAL)
				{
					return;
				}
				float x = GameManager.Instance.player.transform.position.x;
				float y = GameManager.Instance.player.transform.position.y;
				if (GameManager.Instance.StateManager.EState == EGamePlay.NONE || GameManager.Instance.StateManager.EState == EGamePlay.BEGIN)
				{
					x = CameraController.Instance.transform.position.x;
				}
				this.CreateEnemyWithFileJson(new Vector2(x, y));
				this.CheckLetGo(deltaTime);
			}
		}
	}

	private void OnDestroyObject()
	{
		this.DestroySniper();
		this.DestroyEnemyAkm();
		this.DestroyEnemyPistol();
		this.DestroyEnemyGrenade();
		this.DestroyEnemyKnife();
		this.DestroyEnemyFire();
		this.DestroyEnemyRocket();
		this.DestroyEnemyMiniGun();
		this.DestroyTank_1();
		this.DestroyTank2();
		this.DestroyTank3();
		this.DestroyHelicopter();
		this.DestroyAirplane();
		this.DestroyTrack();
		this.DestroyEnemy330();
		this.DestroyEnemy350();
		this.DestroyBee3();
		this.DestroyEnemySungCoi();
		this.DestroyEnemyDrone();
		this.DestroyEnemyFlyMan();
		this.DestroyBugBomb();
		this.DestroyRocketMouse();
		this.DestroyEnemyThuyen();
		this.DestroyUAV();
		this.DestroyNhenNhay();
		this.DestroySau();
		this.DestroySpaceShip();
		this.DestroyMiniShip();
		this.DestroyMiniSpider();
		this.DestroyDaydu();
		this.DestroyBeSung();
		this.DestroyBeSung2();
		this.DestroyEnemyRiu();
		this.DestroyEnemyMiniGun2();
		this.DestroyZombieKnife();
		this.DestroyZombieRobot();
		this.IsInit = false;
		EnemyManager.instance = null;
	}

	private void OnDestroy()
	{
		try
		{
			this.OnDestroyObject();
		}
		catch
		{
		}
	}

	public void OnClearEnemy()
	{
		this.DestroySniper();
		this.DestroyEnemyAkm();
		this.DestroyEnemyPistol();
		this.DestroyEnemyGrenade();
		this.DestroyEnemyKnife();
		this.DestroyEnemyFire();
		this.DestroyEnemyRocket();
		this.DestroyEnemyMiniGun();
		this.DestroyTank_1();
		this.DestroyTank2();
		this.DestroyTank3();
		this.DestroyHelicopter();
		this.DestroyAirplane();
		this.DestroyTrack();
		this.DestroyEnemy330();
		this.DestroyEnemy350();
		this.DestroyBee3();
		this.DestroyEnemySungCoi();
		this.DestroyEnemyDrone();
		this.DestroyEnemyFlyMan();
		this.DestroyBugBomb();
		this.DestroyRocketMouse();
		this.DestroyEnemyThuyen();
		this.DestroyUAV();
		this.DestroyNhenNhay();
		this.DestroySau();
		this.DestroySpaceShip();
		this.DestroyMiniSpider();
		this.DestroyDaydu();
		this.DestroyBeSung();
		this.DestroyBeSung2();
		this.DestroyEnemyRiu();
		this.DestroyEnemyMiniGun2();
		this.DestroyZombieKnife();
		this.DestroyZombieRobot();
		GameManager.Instance.ListEnemy.Clear();
	}

	public void OnPause()
	{
		this.PauseSniper(true);
		this.PauseEnemyAkm(true);
		this.PauseEnemyPistol(true);
		this.PauseEnemyGrenade(true);
		this.PauseEnemyKnife(true);
		this.PauseEnemyFire(true);
		this.PauseEnemyRocket(true);
		this.PauseEnemySungCoi(true);
		this.PauseEnemyMiniGun(true);
		this.PauseEnemyDrone(true);
		this.PauseEnemyFlyMan(true);
		this.PauseBugBomb(true);
		this.PauseRocketMouse(true);
		this.PauseEnemyThuyen(true);
		this.PauseUAV(true);
		this.PauseBee3(true);
		this.PauseNhenNhay(true);
		this.PauseSau(true);
		this.PauseSpaceShip(true);
		this.PauseMiniSpider(true);
		this.PauseTank_1(true);
		this.PauseTank_2(true);
		this.PauseTank_3(true);
		this.PauseBeSung(true);
		this.PauseBeSung2(true);
		this.PauseEnemyRiu(true);
		this.PauseEnemyMiniGun2(true);
		this.PauseZombieKnife(true);
		this.PauseZombieRobot(true);
	}

	public void OnResume()
	{
		this.PauseSniper(false);
		this.PauseEnemyAkm(false);
		this.PauseEnemyPistol(false);
		this.PauseEnemyGrenade(false);
		this.PauseEnemyKnife(false);
		this.PauseEnemyFire(false);
		this.PauseEnemyRocket(false);
		this.PauseEnemySungCoi(false);
		this.PauseEnemyMiniGun(false);
		this.PauseEnemyDrone(false);
		this.PauseEnemyFlyMan(false);
		this.PauseBugBomb(false);
		this.PauseRocketMouse(false);
		this.PauseEnemyThuyen(false);
		this.PauseUAV(false);
		this.PauseBee3(false);
		this.PauseNhenNhay(false);
		this.PauseSau(false);
		this.PauseSpaceShip(false);
		this.PauseMiniSpider(false);
		this.PauseTank_1(false);
		this.PauseTank_2(false);
		this.PauseTank_3(false);
		this.PauseBeSung(false);
		this.PauseBeSung2(false);
		this.PauseEnemyRiu(false);
		this.PauseEnemyMiniGun2(false);
		this.PauseZombieKnife(false);
		this.PauseZombieRobot(false);
	}

	private void CheckLetGo(float deltaTime)
	{
		if (this._coolDownLetGo > 0f)
		{
			this._coolDownLetGo -= deltaTime;
		}
		if (this._coolDownLetGo <= 0f)
		{
			bool flag = true;
			for (int i = 0; i < GameManager.Instance.ListEnemy.Count; i++)
			{
				flag = !GameManager.Instance.ListEnemy[i].isInCamera;
				if (!flag)
				{
					break;
				}
			}
			CameraController.Orientation orientaltion = CameraController.Instance.orientaltion;
			if (orientaltion != CameraController.Orientation.HORIZONTAL)
			{
				if (orientaltion == CameraController.Orientation.VERTICAL)
				{
					flag = (!CameraController.Instance.isVerticalDown && flag && CameraController.Instance.TopCamera() > CameraController.Instance.camPos.y + CameraController.Instance.Size().y + 4f);
				}
			}
			else
			{
				flag = (flag && !CameraController.Instance.StopMoveLeftBoundary && CameraController.Instance.RightCamera() > CameraController.Instance.camPos.x + CameraController.Instance.Size().x + 2f);
			}
			if (!flag)
			{
				this._coolDownLetGo = 2f;
			}
			else
			{
				if (GameManager.Instance.bossManager.Boss)
				{
					EBoss boss = GameManager.Instance.bossManager.Boss.boss;
					if (boss != EBoss.Super_Spider)
					{
						if (GameManager.Instance.bossManager.Boss.isInit)
						{
							return;
						}
					}
					else
					{
						bool fighting = GameManager.Instance.bossManager.Boss.GetComponent<Boss_SuperSpider>().fighting;
						if (fighting)
						{
							return;
						}
					}
				}
				this._coolDownLetGo = this.timeLetGo;
				MyMessage myMessage = new MyMessage();
				CameraController.Orientation orientaltion2 = CameraController.Instance.orientaltion;
				if (orientaltion2 != CameraController.Orientation.HORIZONTAL)
				{
					if (orientaltion2 == CameraController.Orientation.VERTICAL)
					{
						myMessage.IntValue = ((!CameraController.Instance.isVerticalDown) ? 0 : 1);
					}
				}
				else
				{
					myMessage.IntValue = 3;
				}
				EventDispatcher.PostEvent<MyMessage>("Show_Anim_Gogo", myMessage);
			}
		}
	}

	private void CheckStuck()
	{
		if (object.ReferenceEquals(GameManager.Instance.player, null) || GameManager.Instance.CheckPoint == -1)
		{
			return;
		}
		bool flag = false;
		for (int i = 0; i < GameManager.Instance.ListEnemy.Count; i++)
		{
			if (GameManager.Instance.ListEnemy[i].isInCamera)
			{
				flag = true;
				break;
			}
		}
		float num = 0f;
		float num2 = 0f;
		CameraController.Orientation orientaltion = CameraController.Instance.orientaltion;
		if (orientaltion != CameraController.Orientation.HORIZONTAL)
		{
			if (orientaltion == CameraController.Orientation.VERTICAL)
			{
				num = Mathf.Abs(DataLoader.LevelDataCurrent.points[GameManager.Instance.CheckPoint].checkpoint_pos_y - GameManager.Instance.player.GetPosition().y);
				num2 = CameraController.Instance.Size().y * 2f;
			}
		}
		else
		{
			num = Mathf.Abs(DataLoader.LevelDataCurrent.points[GameManager.Instance.CheckPoint].checkpoint_pos_x - GameManager.Instance.player.GetPosition().x);
			num2 = CameraController.Instance.Size().x * 2f;
		}
		if (num >= num2 || flag)
		{
			this.timeCheckStuck = 0f;
			return;
		}
		this.timeCheckStuck += Time.deltaTime;
		if (this.timeCheckStuck >= 3f)
		{
			this.timeCheckStuck = 0f;
			UnityEngine.Debug.Log("___________Clear Stuck_____________");
			DataLoader.LevelDataCurrent.points[GameManager.Instance.CheckPoint].totalEnemy = 0;
		}
	}

	private void CreateEnemyWithFileJson(Vector2 posRambo)
	{
		if (object.ReferenceEquals(DataLoader.LevelDataCurrent, null))
		{
			EventDispatcher.PostEvent("CompletedGame");
			return;
		}
		if (!DataLoader.LevelDataCurrent.isReaded)
		{
			EventDispatcher.PostEvent("CompletedGame");
			return;
		}
		this.CheckStuck();
		if (GameManager.Instance.CheckPoint != -1 && DataLoader.LevelDataCurrent.points[GameManager.Instance.CheckPoint].totalEnemy <= 0 && !DataLoader.LevelDataCurrent.points[GameManager.Instance.CheckPoint].isGo)
		{
			DataLoader.LevelDataCurrent.points[GameManager.Instance.CheckPoint].isGo = true;
			int num = GameManager.Instance.CheckPoint + 1;
			if (num < DataLoader.LevelDataCurrent.points.Count)
			{
				DataLoader.LevelDataCurrent.points[num].isUnlocked = true;
			}
		}
		for (int i = 0; i < DataLoader.LevelDataCurrent.points.Count; i++)
		{
			if (DataLoader.LevelDataCurrent.points[i].isUnlocked && !DataLoader.LevelDataCurrent.points[i].isGo)
			{
				if (GameManager.Instance.CheckPoint < i)
				{
					GameManager.Instance.CheckPoint = i;
					CameraController.Instance.NewCheckpoint(true, 15f);
				}
				EnemyData enemyData = DataLoader.LevelDataCurrent.points[i].enemyData;
				for (int j = 0; j < enemyData.enemyDataInfo.Count; j++)
				{
					EnemyDataInfo enemyDataInfo = enemyData.enemyDataInfo[j];
					if (!enemyDataInfo.created)
					{
						float num2 = Mathf.Max(6.4f, CameraController.Instance.Size().x + 4f);
						CameraController.Orientation orientaltion = CameraController.Instance.orientaltion;
						if (orientaltion != CameraController.Orientation.HORIZONTAL)
						{
							if (orientaltion == CameraController.Orientation.VERTICAL)
							{
								if (Mathf.Abs(enemyDataInfo.pos_y - posRambo.y) > CameraController.Instance.Size().y + 2.5f)
								{
									goto IL_249;
								}
							}
						}
						else if (Mathf.Abs(enemyDataInfo.pos_x - posRambo.x) > num2)
						{
							goto IL_249;
						}
						this.CreateNewEnemy(enemyDataInfo, true);
						enemyDataInfo.created = true;
					}
					IL_249:;
				}
			}
		}
	}

	public BaseEnemy CreateNewEnemy(EnemyDataInfo cacheEnemyData, bool isCreateWithJson = true)
	{
		
		ETypeEnemy type = (ETypeEnemy)cacheEnemyData.type;
		BaseEnemy   baseEnemy = null;
		switch (type)
		{
		case ETypeEnemy.SNIPER:
		{
			EnemySniper sniper = this.CreateSniper();
			sniper.Init(cacheEnemyData, delegate  
			{
				  PoolSniper.Store(sniper);
			});
			baseEnemy = sniper;
			baseEnemy.ReloadInfor(2f, 0.5f);
			break;
		}
		case ETypeEnemy.TANKER:
		{
			EnemyAkm akm = this.CreateEnemyAkm();
			akm.Init(cacheEnemyData, delegate
			{
				PoolAkm.Store(akm);
			});
			baseEnemy = akm;
			baseEnemy.ReloadInfor(2f, 0.5f);
			break;
		}
		case ETypeEnemy.GRENADES:
		{
			EnemyGrenade grenade = this.CreateEnemyGrenade();
			grenade.Init(cacheEnemyData, delegate
			{
				PoolEnemyGrenade.Store(grenade);
			});
			baseEnemy = grenade;
			baseEnemy.ReloadInfor(2f, 0.5f);
			break;
		}
		case ETypeEnemy.FIRE:
		{
			EnemyFire fire = this.CreateEnemyFire();
			fire.Init(cacheEnemyData, delegate
			{
				PoolEnemyfire.Store(fire);
			});
			baseEnemy = fire;
			baseEnemy.ReloadInfor(2f, 0.5f);
			break;
		}
		case ETypeEnemy.ROCKET:
		{
			EnemyRocket rocket = this.CreateEnemyRocket();
			rocket.Init(cacheEnemyData, delegate
			{
				PoolEnemyRocket.Store(rocket);
			});
			baseEnemy = rocket;
			baseEnemy.ReloadInfor(2f, 0.5f);
			break;
		}
		case ETypeEnemy.PISTOL:
		{
			EnemyPistol pistol = this.CreateEnemyPistol();
			pistol.Init(cacheEnemyData, delegate
			{
				PoolEnemyPistol.Store(pistol);
			});
			baseEnemy = pistol;
			baseEnemy.ReloadInfor(2f, 0.5f);
			break;
		}
		case ETypeEnemy.MINI_GUN:
		{
			EnemyMiniGun miniGun = this.CreateEnemyMiniGun();
			miniGun.Init(cacheEnemyData, delegate
			{
				PoolEnemyMiniGun.Store(miniGun);
			});
			baseEnemy = miniGun;
			baseEnemy.ReloadInfor(2f, 0.5f);
			break;
		}
		case ETypeEnemy.KNIFE:
		{
			EnemyKnife knife = this.CreateEnemyKnife();
			knife.Init(cacheEnemyData, delegate
			{
				PoolEnemyKnife.Store(knife);
			});
			baseEnemy = knife;
			baseEnemy.ReloadInfor(2f, 0.5f);
			break;
		}
		case ETypeEnemy.ENEMY_SUNG_COI:
		{
			EnemySungCoi coi = this.CreateEnemySungCoi();
			coi.Init(cacheEnemyData, delegate
			{
				PoolSungCoi.Store(coi);
			});
			baseEnemy = coi;
			baseEnemy.ReloadInfor(2f, 0.5f);
			break;
		}
		case ETypeEnemy.ENEMY_DRONE:
		{
			EnemyDrone drone = this.CreateEnemyDrone();
			drone.Init(cacheEnemyData, delegate
			{
				PoolEnemyDrone.Store(drone);
			});
			baseEnemy = drone;
			baseEnemy.ReloadInfor(2f, 0.5f);
			break;
		}
		case ETypeEnemy.BUG_BOMB:
		{
			BugBomb bugBomb = this.CreateBugBomb();
			bugBomb.Init(cacheEnemyData);
			if (GameMode.Instance.Style != GameMode.GameStyle.MultiPlayer)
			{
				if (isCreateWithJson)
				{
					DataLoader.LevelDataCurrent.points[Mathf.Max(0, GameManager.Instance.CheckPoint)].totalEnemy--;
				}
				GameManager.Instance.TotalEnemyKilled++;
			}
			return null;
		}
		case ETypeEnemy.ROCKET_MOUSE:
		{
			ChuotRocket chuotRocket = this.CreateRocketMouse();
			chuotRocket.Init(cacheEnemyData);
			if (GameMode.Instance.Style != GameMode.GameStyle.MultiPlayer)
			{
				if (isCreateWithJson)
				{
					DataLoader.LevelDataCurrent.points[Mathf.Max(0, GameManager.Instance.CheckPoint)].totalEnemy--;
				}
				GameManager.Instance.TotalEnemyKilled++;
			}
			return null;
		}
		case ETypeEnemy.THUYEN_1:
		case ETypeEnemy.THUYEN_2:
		{
			EnemyThuyen enemyThuyen = this.CreateThuyen();
			cacheEnemyData.DropCoin = false;
			enemyThuyen.InitEnemy(cacheEnemyData);
			enemyThuyen.CheckPoint = Mathf.Max(0, GameManager.Instance.CheckPoint);
			enemyThuyen.ReloadInfor(1f, 0.2f);
			return null;
		}
		case ETypeEnemy.NHEN_NHAY:
		{
			NhenNhay nhenNhay = this.CreateNhenNhay();
			nhenNhay.InitEnemy(cacheEnemyData, delegate(NhenNhay _nhen)
			{
				PoolNhenNhay.Store(_nhen);
			});
			baseEnemy = nhenNhay;
			baseEnemy.ReloadInfor(1f, 0.2f);
			break;
		}
		case ETypeEnemy.SAU:
			this.CreateSau().InitEnemy(cacheEnemyData, isCreateWithJson, delegate(EnemySau sau)
			{
				PoolEnemySau.Store(sau);
			});
			return null;
		case ETypeEnemy.MINI_SPIDER:
		{
			NhenNho nhenNho = this.CreateMiniSpider();
			nhenNho.InitEnemy(cacheEnemyData, delegate(NhenNho spider)
			{
				PoolMiniSpider.Store(spider);
			});
			baseEnemy = nhenNho;
			baseEnemy.ReloadInfor(0.5f, 0.2f);
			break;
		}
		case ETypeEnemy.ENEMY_FLYMAN:
		{
			Enemy_FlyMan fly = this.CreateEnemyFlyMan();
			fly.Init(cacheEnemyData, delegate
			{
				PoolEnemyFlyMan.Store(fly);
			});
			baseEnemy = fly;
			baseEnemy.ReloadInfor(0.5f, 0.2f);
			break;
		}
		case ETypeEnemy.ENEMY_RIU:
		{
			EnemyRiu riu = this.CreateEnemyRiu();
			riu.Init(cacheEnemyData, delegate
			{
				PoolEnemyRiu.Store(riu);
			});
			baseEnemy = riu;
			break;
		}
		case ETypeEnemy.MINI_GUN_2:
		{
			EnemyMiniGun2 mg = this.CreateEnemyMiniGun2();
			mg.Init(cacheEnemyData, delegate
			{
				PoolMiniGun2.Store(mg);
			});
			baseEnemy = mg;
			break;
		}
		case ETypeEnemy.TANK_1:
		{
			Tank_1 tank = this.CreateTank_1();
			tank.Init(cacheEnemyData, delegate
			{
				PoolTank.Store(tank);
			});
			baseEnemy = tank;
			baseEnemy.ReloadInfor(0.2f, 0.3f);
			break;
		}
		case ETypeEnemy.TANK_2:
		{
			Tank_2 tank2 = this.CreateTank2();
			tank2.Init(cacheEnemyData, delegate
			{
				PoolTank2.Store(tank2);
			});
			baseEnemy = tank2;
			baseEnemy.ReloadInfor(0.2f, 0.3f);
			break;
		}
		case ETypeEnemy.TANK_3:
		{
			Tank_3 tank3 = this.CreateTank3();
			tank3.Init(cacheEnemyData, delegate
			{
				PoolTank3.Store(tank3);
			});
			baseEnemy = tank3;
			baseEnemy.ReloadInfor(0.2f, 0.3f);
			break;
		}
		default:
			switch (type)
			{
			case ETypeEnemy.HELICOPTER:
			{
				HelicopterAttack helicopterAttack = this.CreateHelicopter();
				helicopterAttack.cacheEnemyData = cacheEnemyData;
				helicopterAttack.InitEnemy(cacheEnemyData.level);
				baseEnemy = helicopterAttack;
				baseEnemy.transform.position = cacheEnemyData.Vt2;
				baseEnemy.ReloadInfor(0.2f, 0.2f);
				break;
			}
			case ETypeEnemy.AIRPLANE_BOMB:
			{
				Airplane airplane = this.CreateAirplane();
				airplane.Init(cacheEnemyData.level);
				airplane.transform.position = cacheEnemyData.Vt2;
				airplane.CheckPoint = Mathf.Max(0, GameManager.Instance.CheckPoint);
				airplane.isCreateWithJson = isCreateWithJson;
				return null;
			}
			case ETypeEnemy.BEE:
			{
				Bee3 bee = this.CreateBee3();
				if (bee != null)
				{
					bee.cacheEnemyData = cacheEnemyData;
					bee.Init(delegate(Bee3 _bee)
					{
						PoolBee3.Store(_bee);
					});
					baseEnemy = bee;
					baseEnemy.ReloadInfor(0.2f, 0.2f);
				}
				break;
			}
			case ETypeEnemy.UAV:
			{
				UAV uav = this.CreateUAV();
				uav.Init(cacheEnemyData, delegate(UAV _uav)
				{
					poolUAV.Store(_uav);
				});
				baseEnemy = uav;
				baseEnemy.ReloadInfor(1f, 0.2f);
				break;
			}
			case ETypeEnemy.SPACE_SHIP:
			{
				SpaceShip spaceShip = this.CreateSpaceShip();
				spaceShip.cacheEnemyData = cacheEnemyData;
				spaceShip.Init(delegate(SpaceShip s)
				{
					PoolSpaceShip.Store(s);
				}, false);
				baseEnemy = spaceShip;
				baseEnemy.ReloadInfor(0.5f, 0.2f);
				break;
			}
			case ETypeEnemy.MINI_SHIP:
			{
				MiniBoss5_2 miniBoss5_ = this.CreateMiniShip();
				miniBoss5_.cacheEnemyData = cacheEnemyData;
				miniBoss5_.InitEnemy(false, delegate(MiniBoss5_2 _ship)
				{
					PoolMiniShip.Store(_ship);
				});
				baseEnemy = miniBoss5_;
				baseEnemy.ReloadInfor(0.5f, 0.2f);
				break;
			}
			default:
				if (type != ETypeEnemy.ENEMY_330)
				{
					if (type != ETypeEnemy.ENEMY_350)
					{
						if (type == ETypeEnemy.ROCKET_JETPACK)
						{
							JetpackManager.Instance.CreateWarning(cacheEnemyData.Vt2.y * 100f);
							return null;
						}
					}
					else
					{
						Enemy35 enemy = this.CreateEnemy350();
						enemy.cacheEnemyData = cacheEnemyData;
						enemy.OnInitEnemy(cacheEnemyData.level);
						baseEnemy = enemy;
						baseEnemy.transform.position = cacheEnemyData.Vt2;
						baseEnemy.ReloadInfor(2f, 0.5f);
					}
				}
				else
				{
					Enemy33 enemy2 = this.CreateEnemy330();
					enemy2.cacheEnemyData = cacheEnemyData;
					enemy2.OnInitEnemy(cacheEnemyData.level, cacheEnemyData.Vt2);
					baseEnemy = enemy2;
					baseEnemy.ReloadInfor(2f, 0.5f);
				}
				break;
			}
			break;
		case ETypeEnemy.ENEMY_BESUNG:
		{
			BeSung beSung = this.CreateEnemyBeSung();
			beSung.Init(cacheEnemyData, delegate
			{
				PoolBeSung.Store(beSung);
			});
			baseEnemy = beSung;
			break;
		}
		case ETypeEnemy.ENEMY_BESUNG_2:
		{
			BeSung2 beSung2 = this.CreateBeSung2();
			beSung2.Init(cacheEnemyData, delegate
			{
				PoolBeSung2.Store(beSung2);
			});
		    baseEnemy = beSung2;
			break;
		}
		case ETypeEnemy.ZOMBIE_KNIFE:
		{
			ZombieKnife zombie = this.CreateZombieKnife();
			zombie.Init(cacheEnemyData, delegate
			{
				PoolZombieKnife.Store(zombie);
			});
			baseEnemy = zombie;
			break;
		}
		case ETypeEnemy.ZOMBIE_ROBOT:
		{
			ZombieRobot robot = this.CreateZombieRobot();
			robot.Init(cacheEnemyData, delegate
			{
				PoolZombieRobot.Store(robot);
			});
			baseEnemy = robot;
			break;
		}
		}
		GameManager.Instance.ListEnemy.Add(baseEnemy);
		baseEnemy.CheckPoint = Mathf.Max(0, GameManager.Instance.CheckPoint);
		baseEnemy.isCreateWithJson = isCreateWithJson;
		baseEnemy.ResetIfStuck();
		if (isCreateWithJson)
		{
			
			baseEnemy.OnEnemyDeaded = (Action)Delegate.Combine(baseEnemy.OnEnemyDeaded, new Action(delegate()
			{
				baseEnemy.isCreateWithJson = false;
				isCreateWithJson = false;
				if (GameMode.Instance.Style != GameMode.GameStyle.MultiPlayer)
				{
					DataLoader.LevelDataCurrent.points[baseEnemy.CheckPoint].totalEnemy--;
					GameManager.Instance.giftManager.CreateItemWeapon(baseEnemy.Origin());
					GameManager.Instance.TotalEnemyKilled++;
				}
			}));
		}
		return baseEnemy;
	}

	public DayDu CreateDayDu(Vector3 pos)
	{
		if (!this.tfParentDaydu)
		{
			this.tfParentDaydu = new GameObject("Group_DayDu").transform;
			this.tfParentDaydu.parent = base.transform;
			this.PoolDayDu = new ObjectPooling<DayDu>(0, null, null);
		}
		DayDu dayDu = this.PoolDayDu.New();
		if (!dayDu)
		{
			dayDu = UnityEngine.Object.Instantiate<DayDu>(this.day);
			dayDu.gameObject.transform.parent = this.tfParentDaydu;
			this.ListDayDu.Add(dayDu);
		}
		dayDu.Init(pos);
		return dayDu;
	}

	private void UpdateDayDu(float deltaTime)
	{
		for (int i = 0; i < this.ListDayDu.Count; i++)
		{
			if (this.ListDayDu[i] && this.ListDayDu[i].isInit)
			{
				this.ListDayDu[i].OnUpdate(deltaTime);
			}
		}
	}

	private void DestroyDaydu()
	{
		for (int i = 0; i < this.ListDayDu.Count; i++)
		{
			if (this.ListDayDu[i] && this.ListDayDu[i].isInit)
			{
				this.ListDayDu[i].gameObject.SetActive(false);
			}
		}
	}

	public EnemyTrack CreateTrack()
	{
		EnemyTrack enemyTrack = this.PoolTrack.New();
		if (enemyTrack == null)
		{
			enemyTrack = UnityEngine.Object.Instantiate<Transform>(this.ListTrack[0].transform).GetComponent<EnemyTrack>();
			enemyTrack.transform.parent = this.ListTrack[0].transform.parent;
			this.ListTrack.Add(enemyTrack);
		}
		enemyTrack.gameObject.SetActive(true);
		return enemyTrack;
	}

	private void UpdateTrack()
	{
		for (int i = 0; i < this.ListTrack.Count; i++)
		{
			if (this.ListTrack[i] != null && this.ListTrack[i].gameObject.activeSelf)
			{
				this.ListTrack[i].OnUpdate();
			}
		}
	}

	private void DestroyTrack()
	{
		for (int i = 0; i < this.ListTrack.Count; i++)
		{
			if (this.ListTrack[i] != null && this.ListTrack[i].gameObject.activeSelf)
			{
				this.ListTrack[i].gameObject.SetActive(false);
			}
		}
	}

	public Airplane CreateAirplane()
	{
		if (!this.tfParentAirplane)
		{
			this.tfParentAirplane = new GameObject("Group_Airplane").transform;
			this.tfParentAirplane.parent = base.transform;
			this.PoolAirplane = new ObjectPooling<Airplane>(0, null, null);
		}
		Airplane airplane = this.PoolAirplane.New();
		if (airplane == null)
		{
			airplane = UnityEngine.Object.Instantiate<Airplane>(this.airplane);
			airplane.gameObject.transform.parent = this.tfParentAirplane;
			this.ListAirplane.Add(airplane);
		}
		airplane.gameObject.SetActive(true);
		return airplane;
	}

	private void UpdateAirplane()
	{
		for (int i = 0; i < this.ListAirplane.Count; i++)
		{
			if (this.ListAirplane[i] != null && this.ListAirplane[i].gameObject.activeSelf)
			{
				this.ListAirplane[i].UpdateObject();
			}
		}
	}

	private void DestroyAirplane()
	{
		for (int i = 0; i < this.ListAirplane.Count; i++)
		{
			if (this.ListAirplane[i] != null && this.ListAirplane[i].gameObject.activeSelf)
			{
				this.ListAirplane[i].gameObject.SetActive(false);
			}
		}
	}

	public HelicopterAttack CreateHelicopter()
	{
		if (!this.tfParentHelicopter)
		{
			this.tfParentHelicopter = new GameObject("Group_HelioCopter").transform;
			this.tfParentHelicopter.parent = base.transform;
			this.PoolHelicopter = new ObjectPooling<HelicopterAttack>(0, null, null);
		}
		HelicopterAttack helicopterAttack = this.PoolHelicopter.New();
		if (helicopterAttack == null)
		{
			helicopterAttack = UnityEngine.Object.Instantiate<HelicopterAttack>(this.helicopterAttack);
			helicopterAttack.gameObject.transform.parent = this.tfParentHelicopter;
			this.ListHelicopter.Add(helicopterAttack);
		}
		helicopterAttack.gameObject.SetActive(true);
		return helicopterAttack;
	}

	private void UpdateHelicopter()
	{
		for (int i = 0; i < this.ListHelicopter.Count; i++)
		{
			if (this.ListHelicopter[i] != null && this.ListHelicopter[i].gameObject.activeSelf)
			{
				this.ListHelicopter[i].UpdateObject();
			}
		}
	}

	private void DestroyHelicopter()
	{
		for (int i = 0; i < this.ListHelicopter.Count; i++)
		{
			if (this.ListHelicopter[i] != null && this.ListHelicopter[i].gameObject.activeSelf)
			{
				this.ListHelicopter[i].gameObject.SetActive(false);
			}
		}
	}

	public Tank_1 CreateTank_1()
	{
		if (!this.tfParentTank)
		{
			this.tfParentTank = new GameObject("Group_Tank").transform;
			this.tfParentTank.parent = base.transform;
		}
		Tank_1 tank_ = this.PoolTank.New();
		if (tank_ == null)
		{
			tank_ = UnityEngine.Object.Instantiate<Tank_1>(this.tank);
			tank_.gameObject.transform.parent = this.tfParentTank;
			this.ListTank.Add(tank_);
		}
		return tank_;
	}

	private void UpdateTank_1(float deltaTime)
	{
		for (int i = 0; i < this.ListTank.Count; i++)
		{
			if (this.ListTank[i] && this.ListTank[i].isInit)
			{
				this.ListTank[i].OnUpdate(deltaTime);
			}
		}
	}

	private void PauseTank_1(bool pause)
	{
		for (int i = 0; i < this.ListTank.Count; i++)
		{
			if (this.ListTank[i] && this.ListTank[i].isInit)
			{
				this.ListTank[i].Pause(pause);
			}
		}
	}

	private void DestroyTank_1()
	{
		for (int i = 0; i < this.ListTank.Count; i++)
		{
			if (this.ListTank[i] && this.ListTank[i].isInit)
			{
				this.ListTank[i].gameObject.SetActive(false);
			}
		}
	}

	public Tank_2 CreateTank2()
	{
		if (!this.tfParentTank)
		{
			this.tfParentTank = new GameObject("Group_Tank").transform;
			this.tfParentTank.parent = base.transform;
		}
		Tank_2 tank_ = this.PoolTank2.New();
		if (tank_ == null)
		{
			tank_ = UnityEngine.Object.Instantiate<Tank_2>(this.tank2);
			tank_.gameObject.transform.parent = this.tfParentTank;
			this.ListTank2.Add(tank_);
		}
		return tank_;
	}

	private void UpdateTank2(float deltaTime)
	{
		for (int i = 0; i < this.ListTank2.Count; i++)
		{
			if (this.ListTank2[i] && this.ListTank2[i].isInit)
			{
				this.ListTank2[i].OnUpdate(deltaTime);
			}
		}
	}

	private void PauseTank_2(bool pause)
	{
		for (int i = 0; i < this.ListTank2.Count; i++)
		{
			if (this.ListTank2[i] && this.ListTank2[i].isInit)
			{
				this.ListTank2[i].Pause(pause);
			}
		}
	}

	private void DestroyTank2()
	{
		for (int i = 0; i < this.ListTank2.Count; i++)
		{
			if (this.ListTank2[i] && this.ListTank2[i].isInit)
			{
				this.ListTank2[i].gameObject.SetActive(false);
			}
		}
	}

	public Tank_3 CreateTank3()
	{
		if (!this.tfParentTank)
		{
			this.tfParentTank = new GameObject("Group_Tank").transform;
			this.tfParentTank.parent = base.transform;
		}
		Tank_3 tank_ = this.PoolTank3.New();
		if (tank_ == null)
		{
			tank_ = UnityEngine.Object.Instantiate<Tank_3>(this.tank3);
			tank_.gameObject.transform.parent = this.tfParentTank;
			this.ListTank3.Add(tank_);
		}
		return tank_;
	}

	private void UpdateTank3(float deltaTime)
	{
		for (int i = 0; i < this.ListTank3.Count; i++)
		{
			if (this.ListTank3[i] && this.ListTank3[i].isInit)
			{
				this.ListTank3[i].OnUpdate(deltaTime);
			}
		}
	}

	private void PauseTank_3(bool pause)
	{
		for (int i = 0; i < this.ListTank3.Count; i++)
		{
			if (this.ListTank3[i] && this.ListTank3[i].isInit)
			{
				this.ListTank3[i].Pause(pause);
			}
		}
	}

	private void DestroyTank3()
	{
		for (int i = 0; i < this.ListTank3.Count; i++)
		{
			if (this.ListTank3[i] && this.ListTank3[i].isInit)
			{
				this.ListTank3[i].gameObject.SetActive(false);
			}
		}
	}

	public EnemySniper CreateSniper()
	{
		if (!this.tfParentSniper)
		{
			this.PoolSniper = new ObjectPooling<EnemySniper>(0, null, null);
			this.tfParentSniper = new GameObject("GroupSniper").transform;
			this.tfParentSniper.parent = base.transform;
		}
		EnemySniper enemySniper = this.PoolSniper.New();
		if (!enemySniper)
		{
			enemySniper = UnityEngine.Object.Instantiate<EnemySniper>(this.enemySniper);
			this.ListSniper.Add(enemySniper);
			enemySniper.gameObject.transform.parent = this.tfParentSniper;
		}
		return enemySniper;
	}

	public void UpdateSniper(float deltaTime)
	{
		for (int i = 0; i < this.ListSniper.Count; i++)
		{
			if (this.ListSniper[i] && this.ListSniper[i].isInit)
			{
				this.ListSniper[i].OnUpdate(deltaTime);
			}
		}
	}

	private void PauseSniper(bool pause)
	{
		for (int i = 0; i < this.ListSniper.Count; i++)
		{
			if (this.ListSniper[i] && this.ListSniper[i].isInit)
			{
				this.ListSniper[i].Pause(pause);
			}
		}
	}

	private void DestroySniper()
	{
		for (int i = 0; i < this.ListSniper.Count; i++)
		{
			if (this.ListSniper[i] && this.ListSniper[i].isInit)
			{
				this.ListSniper[i].gameObject.SetActive(false);
			}
		}
	}

	public EnemyAkm CreateEnemyAkm()
	{
		if (!this.tfParenAkm)
		{
			this.PoolAkm = new ObjectPooling<EnemyAkm>(0, null, null);
			this.tfParenAkm = new GameObject("GroupAkm").transform;
			this.tfParenAkm.parent = base.transform;
		}
		EnemyAkm enemyAkm = this.PoolAkm.New();
		if (!enemyAkm)
		{
			enemyAkm = UnityEngine.Object.Instantiate<EnemyAkm>(this.enemyAkm);
			enemyAkm.gameObject.transform.parent = this.tfParenAkm;
			this.ListAkm.Add(enemyAkm);
		}
		return enemyAkm;
	}

	private void UpdateEnemyAkm(float deltaTime)
	{
		for (int i = 0; i < this.ListAkm.Count; i++)
		{
			if (this.ListAkm[i] && this.ListAkm[i].isInit)
			{
				this.ListAkm[i].OnUpdate(deltaTime);
			}
		}
	}

	private void PauseEnemyAkm(bool pause)
	{
		for (int i = 0; i < this.ListAkm.Count; i++)
		{
			if (this.ListAkm[i] && this.ListAkm[i].isInit)
			{
				this.ListAkm[i].Pause(pause);
			}
		}
	}

	private void DestroyEnemyAkm()
	{
		for (int i = 0; i < this.ListAkm.Count; i++)
		{
			if (this.ListAkm[i] && this.ListAkm[i].isInit)
			{
				this.ListAkm[i].gameObject.SetActive(false);
			}
		}
	}

	public EnemyPistol CreateEnemyPistol()
	{
		if (!this.tfParentPistol)
		{
			this.PoolEnemyPistol = new ObjectPooling<EnemyPistol>(0, null, null);
			this.tfParentPistol = new GameObject("GroupPistol").transform;
			this.tfParentPistol.parent = base.transform;
		}
		EnemyPistol enemyPistol = this.PoolEnemyPistol.New();
		if (!enemyPistol)
		{
			enemyPistol = UnityEngine.Object.Instantiate<EnemyPistol>(this.enemyPistol);
			this.ListEnemyPistol.Add(enemyPistol);
			enemyPistol.gameObject.transform.parent = this.tfParentPistol;
		}
		return enemyPistol;
	}

	private void UpdateEnemyPistol(float deltaTime)
	{
		for (int i = 0; i < this.ListEnemyPistol.Count; i++)
		{
			if (this.ListEnemyPistol[i] && this.ListEnemyPistol[i].isInit)
			{
				this.ListEnemyPistol[i].OnUpdate(deltaTime);
			}
		}
	}

	private void PauseEnemyPistol(bool pause)
	{
		for (int i = 0; i < this.ListEnemyPistol.Count; i++)
		{
			if (this.ListEnemyPistol[i] && this.ListEnemyPistol[i].isInit)
			{
				this.ListEnemyPistol[i].Pause(pause);
			}
		}
	}

	private void DestroyEnemyPistol()
	{
		for (int i = 0; i < this.ListEnemyPistol.Count; i++)
		{
			if (this.ListEnemyPistol[i] && this.ListEnemyPistol[i].isInit)
			{
				this.ListEnemyPistol[i].gameObject.SetActive(false);
			}
		}
	}

	public EnemyGrenade CreateEnemyGrenade()
	{
		if (!this.tfParentGrenade)
		{
			this.PoolEnemyGrenade = new ObjectPooling<EnemyGrenade>(0, null, null);
			this.tfParentGrenade = new GameObject("GroupGrenade").transform;
			this.tfParentGrenade.parent = base.transform;
		}
		EnemyGrenade enemyGrenade = this.PoolEnemyGrenade.New();
		if (!enemyGrenade)
		{
			enemyGrenade = UnityEngine.Object.Instantiate<EnemyGrenade>(this.enemyGrenade);
			this.ListEnemyGrenade.Add(enemyGrenade);
			enemyGrenade.gameObject.transform.parent = this.tfParentGrenade;
		}
		return enemyGrenade;
	}

	private void UpdateEnemyGrenade(float deltaTime)
	{
		for (int i = 0; i < this.ListEnemyGrenade.Count; i++)
		{
			if (this.ListEnemyGrenade[i] && this.ListEnemyGrenade[i].isInit)
			{
				this.ListEnemyGrenade[i].OnUpdate(deltaTime);
			}
		}
	}

	private void PauseEnemyGrenade(bool pause)
	{
		for (int i = 0; i < this.ListEnemyGrenade.Count; i++)
		{
			if (this.ListEnemyGrenade[i] && this.ListEnemyGrenade[i].isInit)
			{
				this.ListEnemyGrenade[i].Pause(pause);
			}
		}
	}

	private void DestroyEnemyGrenade()
	{
		for (int i = 0; i < this.ListEnemyGrenade.Count; i++)
		{
			if (this.ListEnemyGrenade[i] && this.ListEnemyGrenade[i].isInit)
			{
				this.ListEnemyGrenade[i].gameObject.SetActive(false);
			}
		}
	}

	public EnemyKnife CreateEnemyKnife()
	{
		if (!this.tfParentKnife)
		{
			this.PoolEnemyKnife = new ObjectPooling<EnemyKnife>(0, null, null);
			this.tfParentKnife = new GameObject("GroupKnife").transform;
			this.tfParentKnife.parent = base.transform;
		}
		EnemyKnife enemyKnife = this.PoolEnemyKnife.New();
		if (!enemyKnife)
		{
			enemyKnife = UnityEngine.Object.Instantiate<EnemyKnife>(this.enemyKnife);
			this.ListEnemyKnife.Add(enemyKnife);
			enemyKnife.gameObject.transform.parent = this.tfParentKnife;
		}
		return enemyKnife;
	}

	private void UpdateEnemyKnife(float deltaTime)
	{
		for (int i = 0; i < this.ListEnemyKnife.Count; i++)
		{
			if (this.ListEnemyKnife[i] && this.ListEnemyKnife[i].isInit)
			{
				this.ListEnemyKnife[i].OnUpdate(deltaTime);
			}
		}
	}

	private void PauseEnemyKnife(bool pause)
	{
		for (int i = 0; i < this.ListEnemyKnife.Count; i++)
		{
			if (this.ListEnemyKnife[i] && this.ListEnemyKnife[i].isInit)
			{
				this.ListEnemyKnife[i].Pause(pause);
			}
		}
	}

	private void DestroyEnemyKnife()
	{
		for (int i = 0; i < this.ListEnemyKnife.Count; i++)
		{
			if (this.ListEnemyKnife[i] && this.ListEnemyKnife[i].isInit)
			{
				this.ListEnemyKnife[i].gameObject.SetActive(false);
			}
		}
	}

	public EnemyFire CreateEnemyFire()
	{
		if (!this.tfParentFire)
		{
			this.PoolEnemyfire = new ObjectPooling<EnemyFire>(0, null, null);
			this.tfParentFire = new GameObject("GroupFire").transform;
			this.tfParentFire.parent = base.transform;
		}
		EnemyFire enemyFire = this.PoolEnemyfire.New();
		if (!enemyFire)
		{
			enemyFire = UnityEngine.Object.Instantiate<EnemyFire>(this.enemyFire);
			this.ListEnemyFire.Add(enemyFire);
			enemyFire.gameObject.transform.parent = this.tfParentFire;
		}
		return enemyFire;
	}

	private void UpdateEnemyFire(float deltaTime)
	{
		for (int i = 0; i < this.ListEnemyFire.Count; i++)
		{
			if (this.ListEnemyFire[i] && this.ListEnemyFire[i].isInit)
			{
				this.ListEnemyFire[i].OnUpdate(deltaTime);
			}
		}
	}

	private void PauseEnemyFire(bool pause)
	{
		for (int i = 0; i < this.ListEnemyFire.Count; i++)
		{
			if (this.ListEnemyFire[i] && this.ListEnemyFire[i].isInit)
			{
				this.ListEnemyFire[i].Pause(pause);
			}
		}
	}

	private void DestroyEnemyFire()
	{
		for (int i = 0; i < this.ListEnemyFire.Count; i++)
		{
			if (this.ListEnemyFire[i] && this.ListEnemyFire[i].isInit)
			{
				this.ListEnemyFire[i].gameObject.SetActive(false);
			}
		}
	}

	public EnemyRocket CreateEnemyRocket()
	{
		if (!this.tfParentRocket)
		{
			this.PoolEnemyRocket = new ObjectPooling<EnemyRocket>(0, null, null);
			this.tfParentRocket = new GameObject("GroupRocket").transform;
			this.tfParentRocket.parent = base.transform;
		}
		EnemyRocket enemyRocket = this.PoolEnemyRocket.New();
		if (!enemyRocket)
		{
			enemyRocket = UnityEngine.Object.Instantiate<EnemyRocket>(this.enemyRocket);
			this.ListEnemyRocket.Add(enemyRocket);
			enemyRocket.gameObject.transform.parent = this.tfParentRocket;
		}
		return enemyRocket;
	}

	private void UpdateEnemyRocket(float deltaTime)
	{
		for (int i = 0; i < this.ListEnemyRocket.Count; i++)
		{
			if (this.ListEnemyRocket[i] && this.ListEnemyRocket[i].isInit)
			{
				this.ListEnemyRocket[i].OnUpdate(deltaTime);
			}
		}
	}

	private void PauseEnemyRocket(bool pause)
	{
		for (int i = 0; i < this.ListEnemyRocket.Count; i++)
		{
			if (this.ListEnemyRocket[i] && this.ListEnemyRocket[i].isInit)
			{
				this.ListEnemyRocket[i].Pause(pause);
			}
		}
	}

	private void DestroyEnemyRocket()
	{
		for (int i = 0; i < this.ListEnemyRocket.Count; i++)
		{
			if (this.ListEnemyRocket[i] && this.ListEnemyRocket[i].isInit)
			{
				this.ListEnemyRocket[i].gameObject.SetActive(false);
			}
		}
	}

	public EnemyMiniGun CreateEnemyMiniGun()
	{
		if (!this.tfParentMiniGun)
		{
			this.tfParentMiniGun = new GameObject("Group_MiniGun").transform;
			this.tfParentMiniGun.parent = base.transform;
		}
		EnemyMiniGun enemyMiniGun = this.PoolEnemyMiniGun.New();
		if (!enemyMiniGun)
		{
			enemyMiniGun = UnityEngine.Object.Instantiate<EnemyMiniGun>(this.enemyMiniGun);
			this.ListEnemyMiniGun.Add(enemyMiniGun);
			enemyMiniGun.gameObject.transform.parent = this.tfParentMiniGun;
		}
		return enemyMiniGun;
	}

	private void UpdateEnemyMiniGun(float deltaTime)
	{
		for (int i = 0; i < this.ListEnemyMiniGun.Count; i++)
		{
			if (this.ListEnemyMiniGun[i] && this.ListEnemyMiniGun[i].isInit)
			{
				this.ListEnemyMiniGun[i].OnUpdate(deltaTime);
			}
		}
	}

	private void PauseEnemyMiniGun(bool pause)
	{
		for (int i = 0; i < this.ListEnemyMiniGun.Count; i++)
		{
			if (this.ListEnemyMiniGun[i] && this.ListEnemyMiniGun[i].isInit)
			{
				this.ListEnemyMiniGun[i].Pause(pause);
			}
		}
	}

	private void DestroyEnemyMiniGun()
	{
		for (int i = 0; i < this.ListEnemyMiniGun.Count; i++)
		{
			if (this.ListEnemyMiniGun[i] && this.ListEnemyMiniGun[i].isInit)
			{
				this.ListEnemyMiniGun[i].gameObject.SetActive(false);
			}
		}
	}

	private Enemy33 CreateEnemy330()
	{
		if (!this.tfParentEnemy33)
		{
			this.tfParentEnemy33 = new GameObject("Group_Enemy33").transform;
			this.tfParentEnemy33.parent = base.transform;
			this.EnemyPool330 = new ObjectPooling<Enemy33>(0, null, null);
		}
		Enemy33 enemy = this.EnemyPool330.New();
		if (enemy == null)
		{
			enemy = UnityEngine.Object.Instantiate<Enemy33>(this.enemy33);
			enemy.gameObject.transform.parent = this.tfParentEnemy33;
			this.ListEnemy330.Add(enemy);
		}
		enemy.gameObject.SetActive(true);
		return enemy;
	}

	private void UpdateEnemy330()
	{
		for (int i = 0; i < this.ListEnemy330.Count; i++)
		{
			if (this.ListEnemy330[i] != null && this.ListEnemy330[i].gameObject.activeSelf)
			{
				this.ListEnemy330[i].OnUpdate();
			}
		}
	}

	private void DestroyEnemy330()
	{
		for (int i = 0; i < this.ListEnemy330.Count; i++)
		{
			if (this.ListEnemy330[i] != null && this.ListEnemy330[i].gameObject.activeSelf)
			{
				this.ListEnemy330[i].gameObject.SetActive(false);
			}
		}
	}

	private Enemy35 CreateEnemy350()
	{
		if (!this.tfParentEnemy35)
		{
			this.tfParentEnemy35 = new GameObject("Group_Enemy35").transform;
			this.tfParentEnemy35.parent = base.transform;
			this.EnemyPool350 = new ObjectPooling<Enemy35>(0, null, null);
		}
		Enemy35 enemy = this.EnemyPool350.New();
		if (enemy == null)
		{
			enemy = UnityEngine.Object.Instantiate<Enemy35>(this.enemy35);
			enemy.gameObject.transform.parent = this.tfParentEnemy35;
			this.ListEnemy350.Add(enemy);
		}
		enemy.gameObject.SetActive(true);
		return enemy;
	}

	private void UpdateEnemy350()
	{
		for (int i = 0; i < this.ListEnemy350.Count; i++)
		{
			if (this.ListEnemy350[i] != null && this.ListEnemy350[i].gameObject.activeSelf)
			{
				this.ListEnemy350[i].OnUpdate();
			}
		}
	}

	private void DestroyEnemy350()
	{
		for (int i = 0; i < this.ListEnemy350.Count; i++)
		{
			if (this.ListEnemy350[i] != null && this.ListEnemy350[i].gameObject.activeSelf)
			{
				this.ListEnemy350[i].gameObject.SetActive(false);
			}
		}
	}

	public Bee3 CreateBee3()
	{
		if (!this.tfParentBee3)
		{
			this.tfParentBee3 = new GameObject("Group_Bee3").transform;
			this.tfParentBee3.parent = base.transform;
			this.PoolBee3 = new ObjectPooling<Bee3>(0, null, null);
		}
		Bee3 bee = this.PoolBee3.New();
		if (!bee)
		{
			bee = UnityEngine.Object.Instantiate<Bee3>(this.bee3);
			this.ListBee3.Add(bee);
			bee.gameObject.transform.parent = this.tfParentBee3;
		}
		return bee;
	}

	private void UpdateBee3(float deltaTime)
	{
		for (int i = 0; i < this.ListBee3.Count; i++)
		{
			if (this.ListBee3[i] && this.ListBee3[i].isInit)
			{
				this.ListBee3[i].OnUpdate(deltaTime);
			}
		}
	}

	private void PauseBee3(bool pause)
	{
		for (int i = 0; i < this.ListBee3.Count; i++)
		{
			if (this.ListBee3[i] && this.ListBee3[i].isInit)
			{
				this.ListBee3[i].Pause(pause);
			}
		}
	}

	private void DestroyBee3()
	{
		for (int i = 0; i < this.ListBee3.Count; i++)
		{
			if (this.ListBee3[i] && this.ListBee3[i].isInit)
			{
				this.ListBee3[i].gameObject.SetActive(false);
			}
		}
	}

	public EnemySungCoi CreateEnemySungCoi()
	{
		if (!this.tfParentSungCoi)
		{
			this.tfParentSungCoi = new GameObject("GroupSungCoi").transform;
			this.tfParentSungCoi.parent = base.transform;
			this.PoolSungCoi = new ObjectPooling<EnemySungCoi>(0, null, null);
		}
		EnemySungCoi enemySungCoi = this.PoolSungCoi.New();
		if (!enemySungCoi)
		{
			enemySungCoi = UnityEngine.Object.Instantiate<EnemySungCoi>(this.sungCoi);
			enemySungCoi.gameObject.transform.parent = this.tfParentSungCoi;
			this.ListSungCoi.Add(enemySungCoi);
		}
		return enemySungCoi;
	}

	private void UpdateEnemySungCoi(float deltaTime)
	{
		for (int i = 0; i < this.ListSungCoi.Count; i++)
		{
			if (this.ListSungCoi[i] != null && this.ListSungCoi[i].isInit)
			{
				this.ListSungCoi[i].OnUpdate(deltaTime);
			}
		}
	}

	private void DestroyEnemySungCoi()
	{
		for (int i = 0; i < this.ListSungCoi.Count; i++)
		{
			if (this.ListSungCoi[i] && this.ListSungCoi[i].isInit)
			{
				this.ListSungCoi[i].gameObject.SetActive(false);
			}
		}
	}

	public void PauseEnemySungCoi(bool pause)
	{
		for (int i = 0; i < this.ListSungCoi.Count; i++)
		{
			if (this.ListSungCoi[i] && this.ListSungCoi[i].isInit)
			{
				this.ListSungCoi[i].Pause(pause);
			}
		}
	}

	public EnemyDrone CreateEnemyDrone()
	{
		if (!this.tfParentEnemyDrone)
		{
			this.tfParentEnemyDrone = new GameObject("GroupDrone").transform;
			this.tfParentEnemyDrone.parent = base.transform;
			this.PoolEnemyDrone = new ObjectPooling<EnemyDrone>(0, null, null);
		}
		EnemyDrone enemyDrone = this.PoolEnemyDrone.New();
		if (!enemyDrone)
		{
			enemyDrone = UnityEngine.Object.Instantiate<EnemyDrone>(this.enemyDrone);
			enemyDrone.gameObject.transform.parent = this.tfParentEnemyDrone;
			this.ListEnemyDrone.Add(enemyDrone);
		}
		return enemyDrone;
	}

	private void UpdateEnemyDrone(float deltaTime)
	{
		for (int i = 0; i < this.ListEnemyDrone.Count; i++)
		{
			if (this.ListEnemyDrone[i] && this.ListEnemyDrone[i].isInit)
			{
				this.ListEnemyDrone[i].OnUpdate(deltaTime);
			}
		}
	}

	private void DestroyEnemyDrone()
	{
		for (int i = 0; i < this.ListEnemyDrone.Count; i++)
		{
			if (this.ListEnemyDrone[i] && this.ListEnemyDrone[i].isInit)
			{
				this.ListEnemyDrone[i].gameObject.SetActive(false);
			}
		}
	}

	private void PauseEnemyDrone(bool pause)
	{
		for (int i = 0; i < this.ListEnemyDrone.Count; i++)
		{
			if (this.ListEnemyDrone[i] && this.ListEnemyDrone[i].isInit)
			{
				this.ListEnemyDrone[i].Pause(pause);
			}
		}
	}

	public Enemy_FlyMan CreateEnemyFlyMan()
	{
		if (!this.tfParentEnemyFlyMan)
		{
			this.tfParentEnemyFlyMan = new GameObject("Group_FlyMan").transform;
			this.tfParentEnemyFlyMan.SetParent(base.transform);
			this.PoolEnemyFlyMan = new ObjectPooling<Enemy_FlyMan>(0, null, null);
		}
		Enemy_FlyMan enemy_FlyMan = this.PoolEnemyFlyMan.New();
		if (!enemy_FlyMan)
		{
			enemy_FlyMan = UnityEngine.Object.Instantiate<Enemy_FlyMan>(this.enemyFlyMan);
			this.ListEnemyFlyMan.Add(enemy_FlyMan);
			enemy_FlyMan.gameObject.transform.parent = this.tfParentEnemyFlyMan;
		}
		return enemy_FlyMan;
	}

	private void UpdateEnemyFlyMan(float deltaTime)
	{
		for (int i = 0; i < this.ListEnemyFlyMan.Count; i++)
		{
			if (this.ListEnemyFlyMan[i] && this.ListEnemyFlyMan[i].isInit)
			{
				this.ListEnemyFlyMan[i].OnUpdate(deltaTime);
			}
		}
	}

	private void PauseEnemyFlyMan(bool pause)
	{
		for (int i = 0; i < this.ListEnemyFlyMan.Count; i++)
		{
			if (this.ListEnemyFlyMan[i] && this.ListEnemyFlyMan[i].isInit)
			{
				this.ListEnemyFlyMan[i].Pause(pause);
			}
		}
	}

	private void DestroyEnemyFlyMan()
	{
		for (int i = 0; i < this.ListEnemyFlyMan.Count; i++)
		{
			if (this.ListEnemyFlyMan[i] && this.ListEnemyFlyMan[i].isInit)
			{
				this.ListEnemyFlyMan[i].gameObject.SetActive(false);
			}
		}
	}

	public BeSung CreateEnemyBeSung()
	{
		if (!this.tfParentBeSung)
		{
			this.tfParentBeSung = new GameObject("Group_EnemyBeSung").transform;
			this.tfParentBeSung.parent = base.transform;
		}
		BeSung beSung = this.PoolBeSung.New();
		if (!beSung)
		{
			beSung = UnityEngine.Object.Instantiate<BeSung>(this.beSung);
			beSung.gameObject.transform.parent = this.tfParentBeSung;
			this.ListBeSung.Add(beSung);
		}
		return beSung;
	}

	private void UpdateBeSung(float deltaTime)
	{
		for (int i = 0; i < this.ListBeSung.Count; i++)
		{
			if (this.ListBeSung[i] && this.ListBeSung[i].isInit)
			{
				this.ListBeSung[i].OnUpdate(deltaTime);
			}
		}
	}

	private void PauseBeSung(bool pause)
	{
		for (int i = 0; i < this.ListBeSung.Count; i++)
		{
			if (this.ListBeSung[i] && this.ListBeSung[i].isInit)
			{
				this.ListBeSung[i].Pause(pause);
			}
		}
	}

	private void DestroyBeSung()
	{
		for (int i = 0; i < this.ListBeSung.Count; i++)
		{
			if (this.ListBeSung[i] && this.ListBeSung[i].isInit)
			{
				this.ListBeSung[i].gameObject.SetActive(false);
			}
		}
	}

	public BeSung2 CreateBeSung2()
	{
		if (!this.tfParentBeSung)
		{
			this.tfParentBeSung = new GameObject("Group_EnemyBeSung").transform;
			this.tfParentBeSung.parent = base.transform;
		}
		BeSung2 beSung = this.PoolBeSung2.New();
		if (!beSung)
		{
			beSung = UnityEngine.Object.Instantiate<BeSung2>(this.beSung2);
			this.ListBeSung2.Add(beSung);
			beSung.gameObject.transform.SetParent(this.tfParentBeSung);
		}
		return beSung;
	}

	private void UpdateBeSung2(float deltaTime)
	{
		for (int i = 0; i < this.ListBeSung2.Count; i++)
		{
			if (this.ListBeSung2[i] && this.ListBeSung2[i].isInit)
			{
				this.ListBeSung2[i].OnUpdate(deltaTime);
			}
		}
	}

	private void PauseBeSung2(bool pause)
	{
		for (int i = 0; i < this.ListBeSung2.Count; i++)
		{
			if (this.ListBeSung2[i] && this.ListBeSung2[i].isInit)
			{
				this.ListBeSung2[i].Pause(pause);
			}
		}
	}

	private void DestroyBeSung2()
	{
		for (int i = 0; i < this.ListBeSung2.Count; i++)
		{
			if (this.ListBeSung2[i] && this.ListBeSung2[i].isInit)
			{
				this.ListBeSung2[i].gameObject.SetActive(false);
			}
		}
	}

	public BugBomb CreateBugBomb()
	{
		if (!this.tfParentBugBomb)
		{
			this.tfParentBugBomb = new GameObject("Group_BugBomb").transform;
			this.tfParentBugBomb.parent = base.transform;
			this.BugBombPool = new ObjectPooling<BugBomb>(0, null, null);
		}
		BugBomb bugBomb = this.BugBombPool.New();
		if (!bugBomb)
		{
			bugBomb = UnityEngine.Object.Instantiate<BugBomb>(this.bugBomb);
			bugBomb.gameObject.transform.parent = this.tfParentBugBomb;
			this.ListBugBomb.Add(bugBomb);
		}
		return bugBomb;
	}

	private void UpdateBugBomb(float deltaTime)
	{
		for (int i = 0; i < this.ListBugBomb.Count; i++)
		{
			if (this.ListBugBomb[i] && this.ListBugBomb[i].isInit)
			{
				this.ListBugBomb[i].OnUpdate(deltaTime);
			}
		}
	}

	private void DestroyBugBomb()
	{
		for (int i = 0; i < this.ListBugBomb.Count; i++)
		{
			if (this.ListBugBomb[i] && this.ListBugBomb[i].isInit)
			{
				this.ListBugBomb[i].gameObject.SetActive(false);
			}
		}
	}

	private void PauseBugBomb(bool stop)
	{
		for (int i = 0; i < this.ListBugBomb.Count; i++)
		{
			if (this.ListBugBomb[i] && this.ListBugBomb[i].isInit)
			{
				this.ListBugBomb[i].Pause(stop);
			}
		}
	}

	public ChuotRocket CreateRocketMouse()
	{
		if (!this.tfParentMouseRocket)
		{
			this.tfParentMouseRocket = new GameObject("Group_MouseRocket").transform;
			this.tfParentMouseRocket.parent = base.transform;
			this.RocketMousePool = new ObjectPooling<ChuotRocket>(0, null, null);
		}
		ChuotRocket chuotRocket = this.RocketMousePool.New();
		if (!chuotRocket)
		{
			chuotRocket = UnityEngine.Object.Instantiate<ChuotRocket>(this.mouseRocket);
			chuotRocket.gameObject.transform.parent = this.tfParentMouseRocket;
			this.ListRocketMouse.Add(chuotRocket);
		}
		return chuotRocket;
	}

	private void UpdateRocketMouses(float deltaTime)
	{
		for (int i = 0; i < this.ListRocketMouse.Count; i++)
		{
			if (this.ListRocketMouse[i] && this.ListRocketMouse[i].isInit)
			{
				this.ListRocketMouse[i].OnUpdate(deltaTime);
			}
		}
	}

	private void DestroyRocketMouse()
	{
		for (int i = 0; i < this.ListRocketMouse.Count; i++)
		{
			if (this.ListRocketMouse[i] && this.ListRocketMouse[i].isInit)
			{
				this.ListRocketMouse[i].gameObject.SetActive(false);
			}
		}
	}

	private void PauseRocketMouse(bool stop)
	{
		for (int i = 0; i < this.ListRocketMouse.Count; i++)
		{
			if (this.ListRocketMouse[i] && this.ListRocketMouse[i].isInit)
			{
				this.ListRocketMouse[i].Pause(stop);
			}
		}
	}

	public EnemyThuyen CreateThuyen()
	{
		if (!this.tfParentThuyen)
		{
			this.tfParentThuyen = new GameObject("Group_Thuyen").transform;
			this.tfParentThuyen.parent = base.transform;
			this.EnemyThuyenPool = new ObjectPooling<EnemyThuyen>(0, null, null);
		}
		EnemyThuyen enemyThuyen = this.EnemyThuyenPool.New();
		if (!enemyThuyen)
		{
			enemyThuyen = UnityEngine.Object.Instantiate<EnemyThuyen>(this.enemyThuyen);
			enemyThuyen.gameObject.transform.parent = this.tfParentThuyen;
			this.ListEnemyThuyen.Add(enemyThuyen);
		}
		return enemyThuyen;
	}

	public void UpdateEnemyThuyen(float deltaTime)
	{
		for (int i = 0; i < this.ListEnemyThuyen.Count; i++)
		{
			if (this.ListEnemyThuyen[i].isInit)
			{
				this.ListEnemyThuyen[i].UpdateEnemy(deltaTime);
			}
		}
	}

	private void DestroyEnemyThuyen()
	{
		for (int i = 0; i < this.ListEnemyThuyen.Count; i++)
		{
			if (this.ListEnemyThuyen[i].isInit)
			{
				this.ListEnemyThuyen[i].gameObject.SetActive(false);
			}
		}
	}

	private void PauseEnemyThuyen(bool paused)
	{
		for (int i = 0; i < this.ListEnemyThuyen.Count; i++)
		{
			if (this.ListEnemyThuyen[i].isInit)
			{
				this.ListEnemyThuyen[i].PauseEnemy(paused);
			}
		}
	}

	public UAV CreateUAV()
	{
		if (!this.tfParentUAV)
		{
			this.tfParentUAV = new GameObject("GroupUAV").transform;
			this.tfParentUAV.parent = base.transform;
			this.poolUAV = new ObjectPooling<UAV>(0, null, null);
		}
		UAV uav = this.poolUAV.New();
		if (!uav)
		{
			uav = UnityEngine.Object.Instantiate<UAV>(this.uav);
			this.ListUAV.Add(uav);
			uav.gameObject.transform.parent = this.tfParentUAV;
		}
		return uav;
	}

	public void UpdateUAV(float deltaTime)
	{
		for (int i = 0; i < this.ListUAV.Count; i++)
		{
			if (this.ListUAV[i] && this.ListUAV[i].isInit)
			{
				this.ListUAV[i].OnUpdate(deltaTime);
			}
		}
	}

	public void PauseUAV(bool pause)
	{
		for (int i = 0; i < this.ListUAV.Count; i++)
		{
			if (this.ListUAV[i] && this.ListUAV[i].isInit)
			{
				this.ListUAV[i].Pause(pause);
			}
		}
	}

	public void DestroyUAV()
	{
		for (int i = 0; i < this.ListUAV.Count; i++)
		{
			if (this.ListUAV[i] && this.ListUAV[i].isInit)
			{
				this.ListUAV[i].gameObject.SetActive(false);
			}
		}
	}

	public NhenNhay CreateNhenNhay()
	{
		if (!this.tfParentNhenNhay)
		{
			this.tfParentNhenNhay = new GameObject("Group_NhenNhay").transform;
			this.tfParentNhenNhay.parent = base.transform;
			this.PoolNhenNhay = new ObjectPooling<NhenNhay>(0, null, null);
		}
		NhenNhay nhenNhay = this.PoolNhenNhay.New();
		if (!nhenNhay)
		{
			nhenNhay = UnityEngine.Object.Instantiate<NhenNhay>(this.nhenNhay);
			this.ListNhenNhay.Add(nhenNhay);
			nhenNhay.gameObject.transform.parent = this.tfParentNhenNhay;
		}
		return nhenNhay;
	}

	public void UpdateNhenNhay(float deltaTime)
	{
		for (int i = 0; i < this.ListNhenNhay.Count; i++)
		{
			if (this.ListNhenNhay[i] && this.ListNhenNhay[i].isInit)
			{
				this.ListNhenNhay[i].OnUpdate(deltaTime);
			}
		}
	}

	public void PauseNhenNhay(bool pause)
	{
		for (int i = 0; i < this.ListNhenNhay.Count; i++)
		{
			if (this.ListNhenNhay[i] && this.ListNhenNhay[i].isInit)
			{
				this.ListNhenNhay[i].Pause(pause);
			}
		}
	}

	public void DestroyNhenNhay()
	{
		for (int i = 0; i < this.ListNhenNhay.Count; i++)
		{
			if (this.ListNhenNhay[i] && this.ListNhenNhay[i].isInit)
			{
				this.ListNhenNhay[i].gameObject.SetActive(false);
			}
		}
	}

	public EnemySau CreateSau()
	{
		if (!this.tfParentEnemySau)
		{
			this.PoolEnemySau = new ObjectPooling<EnemySau>(0, null, null);
			this.tfParentEnemySau = new GameObject("Group_Sau").transform;
			this.tfParentEnemySau.parent = base.transform;
		}
		EnemySau enemySau = this.PoolEnemySau.New();
		if (!enemySau)
		{
			enemySau = UnityEngine.Object.Instantiate<EnemySau>(this.enemySau);
			this.ListEnemySau.Add(enemySau);
			enemySau.gameObject.transform.parent = this.tfParentEnemySau;
		}
		return enemySau;
	}

	public void UpdateSau(float deltaTime)
	{
		for (int i = 0; i < this.ListEnemySau.Count; i++)
		{
			if (this.ListEnemySau[i] && this.ListEnemySau[i].isInit)
			{
				this.ListEnemySau[i].OnUpdate(deltaTime);
			}
		}
	}

	public void PauseSau(bool pause)
	{
		for (int i = 0; i < this.ListEnemySau.Count; i++)
		{
			if (this.ListEnemySau[i] && this.ListEnemySau[i].isInit)
			{
				this.ListEnemySau[i].Pause(pause);
			}
		}
	}

	public void DestroySau()
	{
		for (int i = 0; i < this.ListEnemySau.Count; i++)
		{
			if (this.ListEnemySau[i] && this.ListEnemySau[i].isInit)
			{
				this.ListEnemySau[i].gameObject.SetActive(false);
			}
		}
	}

	public SpaceShip CreateSpaceShip()
	{
		if (!this.tfParentSpaceShip)
		{
			this.PoolSpaceShip = new ObjectPooling<SpaceShip>(0, null, null);
			this.tfParentSpaceShip = new GameObject("Group_SpaceShip").transform;
			this.tfParentSpaceShip.parent = base.transform;
		}
		SpaceShip spaceShip = this.PoolSpaceShip.New();
		if (!spaceShip)
		{
			spaceShip = UnityEngine.Object.Instantiate<SpaceShip>(this.spaceShip);
			this.ListSpaceShip.Add(spaceShip);
			spaceShip.gameObject.transform.parent = this.tfParentSpaceShip;
		}
		return spaceShip;
	}

	public void UpdateSpaceShip(float deltaTime)
	{
		for (int i = 0; i < this.ListSpaceShip.Count; i++)
		{
			if (this.ListSpaceShip[i] && this.ListSpaceShip[i].isInit)
			{
				this.ListSpaceShip[i].OnUpdate(deltaTime);
			}
		}
	}

	public void PauseSpaceShip(bool pause)
	{
		for (int i = 0; i < this.ListSpaceShip.Count; i++)
		{
			if (this.ListSpaceShip[i] && this.ListSpaceShip[i].isInit)
			{
				this.ListSpaceShip[i].Pause(pause);
			}
		}
	}

	public void DestroySpaceShip()
	{
		for (int i = 0; i < this.ListSpaceShip.Count; i++)
		{
			if (this.ListSpaceShip[i] && this.ListSpaceShip[i].isInit)
			{
				this.ListSpaceShip[i].gameObject.SetActive(false);
			}
		}
	}

	public MiniBoss5_2 CreateMiniShip()
	{
		if (!this.tfParentMiniShip)
		{
			this.PoolMiniShip = new ObjectPooling<MiniBoss5_2>(0, null, null);
			this.tfParentMiniShip = new GameObject("Group_MiniShip").transform;
			this.tfParentMiniShip.parent = base.transform;
		}
		MiniBoss5_2 miniBoss5_ = this.PoolMiniShip.New();
		if (!miniBoss5_)
		{
			miniBoss5_ = UnityEngine.Object.Instantiate<MiniBoss5_2>(this.miniShip);
			miniBoss5_.gameObject.transform.parent = this.tfParentMiniShip;
			this.ListMiniShip.Add(miniBoss5_);
		}
		return miniBoss5_;
	}

	public void UpdateMiniShip()
	{
		for (int i = 0; i < this.ListMiniShip.Count; i++)
		{
			if (this.ListMiniShip[i] && this.ListMiniShip[i].isInit)
			{
				this.ListMiniShip[i].UpdateObject();
			}
		}
	}

	public void DestroyMiniShip()
	{
		for (int i = 0; i < this.ListMiniShip.Count; i++)
		{
			if (this.ListMiniShip[i] && this.ListMiniShip[i].isInit)
			{
				this.ListMiniShip[i].gameObject.SetActive(false);
			}
		}
	}

	public NhenNho CreateMiniSpider(float damage, float hp, float speed, Vector3 pos)
	{
		NhenNho nhenNho = this.CreateMiniSpider();
		nhenNho.InitObject(damage, hp, speed, pos, delegate(NhenNho s)
		{
			this.PoolMiniSpider.Store(s);
		}, true);
		GameManager.Instance.ListEnemy.Add(nhenNho);
		return nhenNho;
	}

	public NhenNho CreateMiniSpider()
	{
		if (!this.tfParentMiniSpider)
		{
			this.PoolMiniSpider = new ObjectPooling<NhenNho>(0, null, null);
			this.tfParentMiniSpider = new GameObject("Group_MiniSpider").transform;
			this.tfParentMiniSpider.parent = base.transform;
		}
		NhenNho nhenNho = this.PoolMiniSpider.New();
		if (!nhenNho)
		{
			nhenNho = UnityEngine.Object.Instantiate<NhenNho>(this.miniSpider);
			nhenNho.gameObject.transform.parent = this.tfParentMiniSpider;
			this.ListMiniSpider.Add(nhenNho);
		}
		return nhenNho;
	}

	private void UpdateMiniSpider(float deltaTime)
	{
		for (int i = 0; i < this.ListMiniSpider.Count; i++)
		{
			if (this.ListMiniSpider[i] && this.ListMiniSpider[i].isInit)
			{
				this.ListMiniSpider[i].UpdateObject(deltaTime);
			}
		}
	}

	private void PauseMiniSpider(bool pause)
	{
		for (int i = 0; i < this.ListMiniSpider.Count; i++)
		{
			if (this.ListMiniSpider[i] && this.ListMiniSpider[i].isInit)
			{
				this.ListMiniSpider[i].PauseObject(pause);
			}
		}
	}

	private void DestroyMiniSpider()
	{
		for (int i = 0; i < this.ListMiniSpider.Count; i++)
		{
			if (this.ListMiniSpider[i] && this.ListMiniSpider[i].isInit)
			{
				this.ListMiniSpider[i].gameObject.SetActive(false);
			}
		}
	}

	public EnemyRiu CreateEnemyRiu()
	{
		if (!this.tfParentEnemyRiu)
		{
			this.tfParentEnemyRiu = new GameObject("Group_EnemyRiu").transform;
			this.tfParentEnemyRiu.SetParent(base.transform);
			this.PoolEnemyRiu = new ObjectPooling<EnemyRiu>(0, null, null);
		}
		EnemyRiu enemyRiu = this.PoolEnemyRiu.New();
		if (!enemyRiu)
		{
			enemyRiu = UnityEngine.Object.Instantiate<EnemyRiu>(this.enemyRiu);
			this.ListEnemyRiu.Add(enemyRiu);
			enemyRiu.gameObject.transform.SetParent(this.tfParentEnemyRiu);
		}
		return enemyRiu;
	}

	private void UpdateEnemyRiu(float deltaTime)
	{
		for (int i = 0; i < this.ListEnemyRiu.Count; i++)
		{
			if (this.ListEnemyRiu[i] && this.ListEnemyRiu[i].isInit)
			{
				this.ListEnemyRiu[i].OnUpdate(deltaTime);
			}
		}
	}

	private void PauseEnemyRiu(bool pause)
	{
		for (int i = 0; i < this.ListEnemyRiu.Count; i++)
		{
			if (this.ListEnemyRiu[i] && this.ListEnemyRiu[i].isInit)
			{
				this.ListEnemyRiu[i].Pause(pause);
			}
		}
	}

	private void DestroyEnemyRiu()
	{
		for (int i = 0; i < this.ListEnemyRiu.Count; i++)
		{
			if (this.ListEnemyRiu[i] && this.ListEnemyRiu[i].isInit)
			{
				this.ListEnemyRiu[i].gameObject.SetActive(false);
			}
		}
	}

	public EnemyMiniGun2 CreateEnemyMiniGun2()
	{
		if (!this.tfParentMiniGun)
		{
			this.tfParentMiniGun = new GameObject("Group_MiniGun").transform;
			this.tfParentMiniGun.parent = base.transform;
		}
		EnemyMiniGun2 enemyMiniGun = this.PoolMiniGun2.New();
		if (!enemyMiniGun)
		{
			enemyMiniGun = UnityEngine.Object.Instantiate<EnemyMiniGun2>(this.enemyMiniGun2);
			this.ListMiniGun2.Add(enemyMiniGun);
			enemyMiniGun.gameObject.transform.SetParent(this.tfParentMiniGun);
		}
		return enemyMiniGun;
	}

	private void UpdateEnemyMiniGun2(float deltaTime)
	{
		for (int i = 0; i < this.ListMiniGun2.Count; i++)
		{
			if (this.ListMiniGun2[i] && this.ListMiniGun2[i].isInit)
			{
				this.ListMiniGun2[i].OnUpdate(deltaTime);
			}
		}
	}

	private void PauseEnemyMiniGun2(bool pause)
	{
		for (int i = 0; i < this.ListMiniGun2.Count; i++)
		{
			if (this.ListMiniGun2[i] && this.ListMiniGun2[i].isInit)
			{
				this.ListMiniGun2[i].Pause(pause);
			}
		}
	}

	private void DestroyEnemyMiniGun2()
	{
		for (int i = 0; i < this.ListMiniGun2.Count; i++)
		{
			if (this.ListMiniGun2[i] && this.ListMiniGun2[i].isInit)
			{
				this.ListMiniGun2[i].gameObject.SetActive(false);
			}
		}
	}

	public ZombieKnife CreateZombieKnife()
	{
		if (!this.tfParentZombie)
		{
			this.tfParentZombie = new GameObject("Group_Zombie").transform;
			this.tfParentZombie.SetParent(base.transform);
		}
		ZombieKnife zombieKnife = this.PoolZombieKnife.New();
		if (!zombieKnife)
		{
			zombieKnife = UnityEngine.Object.Instantiate<ZombieKnife>(this.zombieKnife);
			this.ListZombieKnife.Add(zombieKnife);
			zombieKnife.gameObject.transform.SetParent(this.tfParentZombie);
		}
		return zombieKnife;
	}

	private void UpdateZombieKnife(float deltaTime)
	{
		int count = this.ListZombieKnife.Count;
		for (int i = 0; i < count; i++)
		{
			if (this.ListZombieKnife[i] && this.ListZombieKnife[i].isInit)
			{
				this.ListZombieKnife[i].OnUpdate(deltaTime);
			}
		}
	}

	private void PauseZombieKnife(bool pause)
	{
		int count = this.ListZombieKnife.Count;
		for (int i = 0; i < count; i++)
		{
			if (this.ListZombieKnife[i] && this.ListZombieKnife[i].isInit)
			{
				this.ListZombieKnife[i].Pause(pause);
			}
		}
	}

	private void DestroyZombieKnife()
	{
		int count = this.ListZombieKnife.Count;
		for (int i = 0; i < count; i++)
		{
			if (this.ListZombieKnife[i] && this.ListZombieKnife[i].isInit)
			{
				this.ListZombieKnife[i].gameObject.SetActive(false);
			}
		}
	}

	public ZombieRobot CreateZombieRobot()
	{
		if (!this.tfParentZombie)
		{
			this.tfParentZombie = new GameObject("Group_Zombie").transform;
			this.tfParentZombie.SetParent(base.transform);
		}
		ZombieRobot zombieRobot = this.PoolZombieRobot.New();
		if (!zombieRobot)
		{
			zombieRobot = UnityEngine.Object.Instantiate<ZombieRobot>(this.zombieRobot);
			this.ListZombieRobot.Add(zombieRobot);
			zombieRobot.gameObject.transform.SetParent(this.tfParentZombie);
		}
		return zombieRobot;
	}

	private void UpdateZombieRobot(float deltaTime)
	{
		int count = this.ListZombieRobot.Count;
		for (int i = 0; i < count; i++)
		{
			if (this.ListZombieRobot[i] && this.ListZombieRobot[i].isInit)
			{
				this.ListZombieRobot[i].OnUpdate(deltaTime);
			}
		}
	}

	private void PauseZombieRobot(bool pause)
	{
		int count = this.ListZombieRobot.Count;
		for (int i = 0; i < count; i++)
		{
			if (this.ListZombieRobot[i] && this.ListZombieRobot[i].isInit)
			{
				this.ListZombieRobot[i].Pause(pause);
			}
		}
	}

	private void DestroyZombieRobot()
	{
		int count = this.ListZombieRobot.Count;
		for (int i = 0; i < count; i++)
		{
			if (this.ListZombieRobot[i] && this.ListZombieRobot[i].isInit)
			{
				this.ListZombieRobot[i].gameObject.SetActive(false);
			}
		}
	}

	private static EnemyManager instance;

	private const int LENGTH_LIST = 10;

	private float timeCheckStuck;

	[SerializeField]
	[Header("_____________Day du__________________")]
	private DayDu day;

	private Transform tfParentDaydu;

	private List<DayDu> ListDayDu;

	public ObjectPooling<DayDu> PoolDayDu;

	[Header("_____________SNIPER_________________")]
	[SerializeField]
	private EnemySniper enemySniper;

	private Transform tfParentSniper;

	private List<EnemySniper> ListSniper;

	public ObjectPooling<EnemySniper> PoolSniper;

	[Header("_____________ AKM ___________________")]
	[SerializeField]
	private EnemyAkm enemyAkm;

	private Transform tfParenAkm;

	private List<EnemyAkm> ListAkm;

	public ObjectPooling<EnemyAkm> PoolAkm;

	[SerializeField]
	[Header("_____________PISTOL_________________")]
	private EnemyPistol enemyPistol;

	private Transform tfParentPistol;

	private List<EnemyPistol> ListEnemyPistol;

	public ObjectPooling<EnemyPistol> PoolEnemyPistol;

	[Header("____________GRENADES______________")]
	[SerializeField]
	private EnemyGrenade enemyGrenade;

	private Transform tfParentGrenade;

	private List<EnemyGrenade> ListEnemyGrenade;

	public ObjectPooling<EnemyGrenade> PoolEnemyGrenade;

	[SerializeField]
	[Header("_____________KNIFE_________________")]
	private EnemyKnife enemyKnife;

	private Transform tfParentKnife;

	private List<EnemyKnife> ListEnemyKnife;

	public ObjectPooling<EnemyKnife> PoolEnemyKnife;

	[SerializeField]
	[Header("______________FIRE_________________")]
	private EnemyFire enemyFire;

	private Transform tfParentFire;

	private List<EnemyFire> ListEnemyFire;

	public ObjectPooling<EnemyFire> PoolEnemyfire;

	[SerializeField]
	[Header("_____________ROCKET______________")]
	private EnemyRocket enemyRocket;

	private Transform tfParentRocket;

	private List<EnemyRocket> ListEnemyRocket;

	public ObjectPooling<EnemyRocket> PoolEnemyRocket;

	[SerializeField]
	[Header("_____________MINI GUN_______________")]
	private EnemyMiniGun enemyMiniGun;

	private Transform tfParentMiniGun;

	private List<EnemyMiniGun> ListEnemyMiniGun;

	public ObjectPooling<EnemyMiniGun> PoolEnemyMiniGun;

	[SerializeField]
	[Header("_____________TANK 1________________")]
	private Tank_1 tank;

	private Transform tfParentTank;

	private List<Tank_1> ListTank;

	public ObjectPooling<Tank_1> PoolTank;

	[SerializeField]
	[Header("_____________TANK 2________________")]
	private Tank_2 tank2;

	private List<Tank_2> ListTank2;

	public ObjectPooling<Tank_2> PoolTank2;

	[SerializeField]
	[Header("_____________TANK 3________________")]
	private Tank_3 tank3;

	private List<Tank_3> ListTank3;

	public ObjectPooling<Tank_3> PoolTank3;

	[Header("_____________Helicopter______________")]
	[SerializeField]
	private HelicopterAttack helicopterAttack;

	private Transform tfParentHelicopter;

	private List<HelicopterAttack> ListHelicopter;

	public ObjectPooling<HelicopterAttack> PoolHelicopter;

	[SerializeField]
	[Header("_____________Airplane_______________")]
	private Airplane airplane;

	private Transform tfParentAirplane;

	private List<Airplane> ListAirplane;

	public ObjectPooling<Airplane> PoolAirplane;

	[SerializeField]
	[Header("_____________Track_________________")]
	private List<EnemyTrack> ListTrack;

	public ObjectPooling<EnemyTrack> PoolTrack;

	[Header("___________ Enemy 330______________")]
	[SerializeField]
	private Enemy33 enemy33;

	private Transform tfParentEnemy33;

	private List<Enemy33> ListEnemy330;

	public ObjectPooling<Enemy33> EnemyPool330;

	[Header("___________Enemy 350____________")]
	[SerializeField]
	private Enemy35 enemy35;

	private Transform tfParentEnemy35;

	private List<Enemy35> ListEnemy350;

	public ObjectPooling<Enemy35> EnemyPool350;

	[Header("______________Bee3_______________")]
	[SerializeField]
	private Bee3 bee3;

	private Transform tfParentBee3;

	private List<Bee3> ListBee3;

	public ObjectPooling<Bee3> PoolBee3;

	[SerializeField]
	[Header("____________Enemy Sung Coi___________")]
	private EnemySungCoi sungCoi;

	private Transform tfParentSungCoi;

	private List<EnemySungCoi> ListSungCoi;

	public ObjectPooling<EnemySungCoi> PoolSungCoi;

	[SerializeField]
	[Header("____________Enemy Drone_____________")]
	private EnemyDrone enemyDrone;

	private Transform tfParentEnemyDrone;

	private List<EnemyDrone> ListEnemyDrone;

	public ObjectPooling<EnemyDrone> PoolEnemyDrone;

	[Header("____________Enemy Fly Man_____________")]
	[SerializeField]
	private Enemy_FlyMan enemyFlyMan;

	private Transform tfParentEnemyFlyMan;

	private List<Enemy_FlyMan> ListEnemyFlyMan;

	public ObjectPooling<Enemy_FlyMan> PoolEnemyFlyMan;

	[SerializeField]
	[Header("___________Enemy Be Sung______________")]
	private BeSung beSung;

	private Transform tfParentBeSung;

	private List<BeSung> ListBeSung;

	public ObjectPooling<BeSung> PoolBeSung;

	[SerializeField]
	private BeSung2 beSung2;

	private List<BeSung2> ListBeSung2;

	public ObjectPooling<BeSung2> PoolBeSung2;

	[SerializeField]
	[Header("_____________Bug Bomb_____________")]
	private BugBomb bugBomb;

	private Transform tfParentBugBomb;

	private List<BugBomb> ListBugBomb;

	public ObjectPooling<BugBomb> BugBombPool;

	[SerializeField]
	[Header("___________Mouse Rocket_____________")]
	private ChuotRocket mouseRocket;

	private Transform tfParentMouseRocket;

	private List<ChuotRocket> ListRocketMouse;

	public ObjectPooling<ChuotRocket> RocketMousePool;

	[SerializeField]
	[Header("_____________ Thuyen ______________")]
	private EnemyThuyen enemyThuyen;

	private Transform tfParentThuyen;

	private List<EnemyThuyen> ListEnemyThuyen;

	public ObjectPooling<EnemyThuyen> EnemyThuyenPool;

	[SerializeField]
	[Header("_______________UAV______________")]
	private UAV uav;

	private Transform tfParentUAV;

	private List<UAV> ListUAV;

	public ObjectPooling<UAV> poolUAV;

	[Header("_____________NhenNhay______________")]
	[SerializeField]
	private NhenNhay nhenNhay;

	private Transform tfParentNhenNhay;

	private List<NhenNhay> ListNhenNhay;

	public ObjectPooling<NhenNhay> PoolNhenNhay;

	[Header(" _______________ Sau _________________")]
	[SerializeField]
	private EnemySau enemySau;

	private Transform tfParentEnemySau;

	private List<EnemySau> ListEnemySau;

	public ObjectPooling<EnemySau> PoolEnemySau;

	[SerializeField]
	[Header("______________ Space Ship ________________")]
	private SpaceShip spaceShip;

	private Transform tfParentSpaceShip;

	private List<SpaceShip> ListSpaceShip;

	public ObjectPooling<SpaceShip> PoolSpaceShip;

	[Header("________________ Mini Ship________________")]
	[SerializeField]
	private MiniBoss5_2 miniShip;

	private Transform tfParentMiniShip;

	private List<MiniBoss5_2> ListMiniShip;

	public ObjectPooling<MiniBoss5_2> PoolMiniShip;

	[SerializeField]
	[Header("____________ Nhen Nho-Boss Supper Spider___________")]
	private NhenNho miniSpider;

	private Transform tfParentMiniSpider;

	private List<NhenNho> ListMiniSpider;

	public ObjectPooling<NhenNho> PoolMiniSpider;

	[Header("__________________ Enemy Riu_________________")]
	[SerializeField]
	private EnemyRiu enemyRiu;

	private Transform tfParentEnemyRiu;

	private List<EnemyRiu> ListEnemyRiu;

	public ObjectPooling<EnemyRiu> PoolEnemyRiu;

	[SerializeField]
	[Header("__________________ Enemy MiniGun 2_________________")]
	private EnemyMiniGun2 enemyMiniGun2;

	private List<EnemyMiniGun2> ListMiniGun2;

	public ObjectPooling<EnemyMiniGun2> PoolMiniGun2;

	private Transform tfParentZombie;

	[Header(" __________________ Zombie Knife ____________________")]
	[SerializeField]
	private ZombieKnife zombieKnife;

	private List<ZombieKnife> ListZombieKnife;

	public ObjectPooling<ZombieKnife> PoolZombieKnife;

	[Header("__________________ Zombie Robot_____________________")]
	[SerializeField]
	private ZombieRobot zombieRobot;

	private List<ZombieRobot> ListZombieRobot;

	private ObjectPooling<ZombieRobot> PoolZombieRobot;

	[Header("___________________ Heliocopter__________________")]
	public EnemyHelicopter enemy_helicopter;

	public bool IsInit;

	[HideInInspector]
	public EnemyRandom enemyRandom;

	[NonSerialized]
	public int CountEnemyPreloadDataSuccess;

	public AutoSpawnEnemies[] Spawns;

	private float _coolDownLetGo;

	private float timeLetGo = 10f;
}
