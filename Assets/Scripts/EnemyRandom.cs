using System;
using UnityEngine;

public class EnemyRandom
{
	public void OnUpdate(float deltaTime)
	{
		if (!this.isInit)
		{
			return;
		}
		this.playerPos = GameManager.Instance.player.transform.position;
		if ((CameraController.Instance.orientaltion == CameraController.Orientation.HORIZONTAL && this.playerPos.x <= 10f) || GameManager.Instance.ListEnemy.Count > 12)
		{
			return;
		}
		this.TimeCreateEnemy += deltaTime;
		GameMode.ModePlay modePlay = GameMode.Instance.modePlay;
		if (modePlay != GameMode.ModePlay.Boss_Mode)
		{
			if (modePlay != GameMode.ModePlay.Campaign)
			{
				if (modePlay != GameMode.ModePlay.Endless)
				{
				}
			}
			else
			{
				switch (GameManager.Instance.Level)
				{
				case ELevel.LEVEL_9:
					if (this.playerPos.x > 107.09f)
					{
						return;
					}
					if (GameManager.Instance.player._controller.isGrounded && (this.playerPos.x <= 34f || this.playerPos.x >= 70f))
					{
						if (this.TimeCreateEnemy <= 12f)
						{
							return;
						}
						this.TimeCreateEnemy = 0f;
						this.CreateEnemyKnife(2, CameraController.Instance.GetPosEnemyRandomLeft());
						this.CreateEnemyKnife(2, CameraController.Instance.GetPosEnemyRandomRight());
					}
					else if (this.playerPos.x > 34f && this.playerPos.x < 70f)
					{
						if (this.TimeCreateEnemy <= 22f)
						{
							return;
						}
						this.TimeCreateEnemy = 0f;
						bool flag = UnityEngine.Random.Range(0f, 1f) >= 0.5f;
						this.posCameraX1 = new float[]
						{
							CameraController.Instance.camPos.x,
							CameraController.Instance.camPos.x + 3.2f,
							CameraController.Instance.camPos.x + 6.4f
						};
						this.posCameraX2 = new float[]
						{
							CameraController.Instance.camPos.x,
							CameraController.Instance.camPos.x + 4f
						};
						this.posCameraX = ((!flag) ? this.posCameraX2 : this.posCameraX1);
						this.posEnemy = Vector2.zero;
						this.posEnemy.y = CameraController.Instance.camPos.y + 5f;
						for (int i = 0; i < this.posCameraX.Length; i++)
						{
							this.posEnemy.x = this.posCameraX[i];
							this.CreateEnemyParachute(2, this.posEnemy);
						}
					}
					break;
				case ELevel.LEVEL_15:
				{
					if (this.TimeCreateEnemy <= 5f || this.CountEnemy > 2 || this.playerPos.x > 106f || !GameManager.Instance.player._controller.isGrounded)
					{
						return;
					}
					this.TimeCreateEnemy = 0f;
					bool flag = UnityEngine.Random.Range(0f, 1f) >= 0.5f;
					this.posCameraX1 = new float[]
					{
						CameraController.Instance.camPos.x,
						CameraController.Instance.camPos.x + 3.2f,
						CameraController.Instance.camPos.x + 6.4f
					};
					this.posCameraX2 = new float[]
					{
						CameraController.Instance.camPos.x,
						CameraController.Instance.camPos.x + 4f
					};
					this.posCameraX = ((!flag) ? this.posCameraX2 : this.posCameraX1);
					this.posEnemy = Vector2.zero;
					this.posEnemy.y = CameraController.Instance.camPos.y + 5f;
					for (int j = 0; j < this.posCameraX.Length; j++)
					{
						this.posEnemy.x = this.posCameraX[j];
						this.CreateEnemyParachute(2, this.posEnemy);
					}
					break;
				}
				case ELevel.LEVEL_19:
				{
					if (this.TimeCreateEnemy <= 5f || this.CountEnemy > 1 || this.playerPos.x > 180f || !GameManager.Instance.player._controller.isGrounded)
					{
						return;
					}
					this.TimeCreateEnemy = 0f;
					bool flag = UnityEngine.Random.Range(0f, 1f) >= 0.5f;
					if (flag)
					{
						this.CreateEnemyKnife(5, (UnityEngine.Random.Range(0f, 1f) < 0.5f) ? CameraController.Instance.GetPosEnemyRandomRight() : CameraController.Instance.GetPosEnemyRandomLeft());
					}
					else
					{
						this.CreateEnemyKnife(5, CameraController.Instance.GetPosEnemyRandomLeft());
						this.CreateEnemyKnife(5, CameraController.Instance.GetPosEnemyRandomRight());
					}
					break;
				}
				case ELevel.LEVEL_20:
				{
					if (this.TimeCreateEnemy <= 5f || this.CountEnemy > 2 || this.playerPos.x > 150f || !GameManager.Instance.player._controller.isGrounded)
					{
						return;
					}
					this.TimeCreateEnemy = 0f;
					bool flag = UnityEngine.Random.Range(0f, 1f) >= 0.5f;
					if (flag)
					{
						this.CreateEnemyKnife(6, (UnityEngine.Random.Range(0f, 1f) < 0.5f) ? CameraController.Instance.GetPosEnemyRandomRight() : CameraController.Instance.GetPosEnemyRandomLeft());
					}
					else
					{
						this.CreateEnemyKnife(6, CameraController.Instance.GetPosEnemyRandomLeft());
						this.CreateEnemyKnife(6, CameraController.Instance.GetPosEnemyRandomRight());
					}
					break;
				}
				case ELevel.LEVEL_21:
				{
					if (!this.run || this.TimeCreateEnemy < 4f)
					{
						return;
					}
					this.TimeCreateEnemy = 0f;
					bool flag = UnityEngine.Random.Range(0f, 1f) >= 0.5f;
					this.posCameraX1 = new float[]
					{
						CameraController.Instance.camPos.x,
						CameraController.Instance.camPos.x + 3.2f,
						CameraController.Instance.camPos.x + 6.4f
					};
					this.posCameraX2 = new float[]
					{
						CameraController.Instance.camPos.x,
						CameraController.Instance.camPos.x + 4f
					};
					this.posCameraX = ((!flag) ? this.posCameraX2 : this.posCameraX1);
					this.posEnemy = Vector2.zero;
					this.posEnemy.y = CameraController.Instance.camPos.y + 5f;
					for (int k = 0; k < this.posCameraX.Length; k++)
					{
						this.posEnemy.x = this.posCameraX[k];
						this.CreateEnemyParachute(2, this.posEnemy);
					}
					break;
				}
				case ELevel.LEVEL_22:
				{
					if (this.TimeCreateEnemy <= 3f || this.CountEnemy > 4 || this.playerPos.x > 145f || !GameManager.Instance.player._controller.isGrounded)
					{
						return;
					}
					this.TimeCreateEnemy = 0f;
					bool flag = UnityEngine.Random.Range(0f, 1f) >= 0.5f;
					if (flag)
					{
						this.CreateEnemyKnife(6, (UnityEngine.Random.Range(0f, 1f) < 0.5f) ? CameraController.Instance.GetPosEnemyRandomRight() : CameraController.Instance.GetPosEnemyRandomLeft());
					}
					else
					{
						this.CreateEnemyKnife(6, CameraController.Instance.GetPosEnemyRandomLeft());
						this.CreateEnemyKnife(6, CameraController.Instance.GetPosEnemyRandomRight());
					}
					break;
				}
				case ELevel.LEVEL_23:
				{
					if (!this.run || this.TimeCreateEnemy < 4f)
					{
						return;
					}
					this.TimeCreateEnemy = 0f;
					bool flag = UnityEngine.Random.Range(0f, 1f) >= 0.5f;
					this.posCameraX1 = new float[]
					{
						CameraController.Instance.camPos.x,
						CameraController.Instance.camPos.x + 3.2f,
						CameraController.Instance.camPos.x + 6.4f
					};
					this.posCameraX2 = new float[]
					{
						CameraController.Instance.camPos.x,
						CameraController.Instance.camPos.x + 4f
					};
					this.posCameraX = ((!flag) ? this.posCameraX2 : this.posCameraX1);
					this.posEnemy = Vector2.zero;
					this.posEnemy.y = CameraController.Instance.camPos.y + 5f;
					for (int l = 0; l < this.posCameraX.Length; l++)
					{
						this.posEnemy.x = this.posCameraX[l];
						this.CreateEnemyParachute(2, this.posEnemy);
					}
					break;
				}
				case ELevel.LEVEL_24:
				{
					if (this.TimeCreateEnemy <= 8f || this.CountEnemy > 8 || this.playerPos.x > 170f)
					{
						return;
					}
					this.TimeCreateEnemy = 0f;
					bool flag = UnityEngine.Random.Range(0f, 1f) >= 0.5f;
					this.posCameraX1 = new float[]
					{
						CameraController.Instance.camPos.x,
						CameraController.Instance.camPos.x + 3.2f,
						CameraController.Instance.camPos.x + 6.4f
					};
					this.posCameraX2 = new float[]
					{
						CameraController.Instance.camPos.x,
						CameraController.Instance.camPos.x + 4f
					};
					this.posCameraX = ((!flag) ? this.posCameraX2 : this.posCameraX1);
					this.posEnemy = Vector2.zero;
					this.posEnemy.y = CameraController.Instance.camPos.y + 5f;
					for (int m = 0; m < this.posCameraX.Length; m++)
					{
						this.posEnemy.x = this.posCameraX[m];
						this.CreateEnemyParachute(6, this.posEnemy);
					}
					break;
				}
				case ELevel.LEVEL_25:
				{
					if (this.TimeCreateEnemy <= 5f || this.CountEnemy > 8 || this.playerPos.x > 127.2f || !GameManager.Instance.player._controller.isGrounded)
					{
						return;
					}
					this.TimeCreateEnemy = 0f;
					bool flag = UnityEngine.Random.Range(0f, 1f) >= 0.5f;
					if (flag)
					{
						this.CreateEnemyKnife(7, (UnityEngine.Random.Range(0f, 1f) < 0.5f) ? CameraController.Instance.GetPosEnemyRandomRight() : CameraController.Instance.GetPosEnemyRandomLeft());
					}
					else
					{
						this.CreateEnemyKnife(7, CameraController.Instance.GetPosEnemyRandomLeft());
						this.CreateEnemyKnife(7, CameraController.Instance.GetPosEnemyRandomRight());
					}
					break;
				}
				case ELevel.LEVEL_26:
				{
					if (this.TimeCreateEnemy <= 4f || this.CountEnemy > 8 || this.playerPos.x > 115f || !GameManager.Instance.player._controller.isGrounded)
					{
						return;
					}
					this.TimeCreateEnemy = 0f;
					bool flag = UnityEngine.Random.Range(0f, 1f) >= 0.5f;
					if (flag)
					{
						this.CreateEnemyKnife(7, (UnityEngine.Random.Range(0f, 1f) < 0.5f) ? CameraController.Instance.GetPosEnemyRandomRight() : CameraController.Instance.GetPosEnemyRandomLeft());
					}
					else
					{
						this.CreateEnemyKnife(7, CameraController.Instance.GetPosEnemyRandomLeft());
						this.CreateEnemyKnife(7, CameraController.Instance.GetPosEnemyRandomRight());
					}
					break;
				}
				case ELevel.LEVEL_27:
				{
					if (this.TimeCreateEnemy <= 4f || this.CountEnemy > 8)
					{
						return;
					}
					this.TimeCreateEnemy = 0f;
					bool flag = UnityEngine.Random.Range(0f, 1f) >= 0.5f;
					this.posCameraX1 = new float[]
					{
						CameraController.Instance.camPos.x,
						CameraController.Instance.camPos.x + 3.2f,
						CameraController.Instance.camPos.x + 6.4f
					};
					this.posCameraX2 = new float[]
					{
						CameraController.Instance.camPos.x,
						CameraController.Instance.camPos.x + 4f
					};
					this.posCameraX = ((!flag) ? this.posCameraX2 : this.posCameraX1);
					this.posEnemy = Vector2.zero;
					this.posEnemy.y = CameraController.Instance.camPos.y + 5f;
					for (int n = 0; n < this.posCameraX.Length; n++)
					{
						this.posEnemy.x = this.posCameraX[n];
						this.CreateEnemyParachute(7, this.posEnemy);
					}
					break;
				}
				case ELevel.LEVEL_30:
				{
					if (this.TimeCreateEnemy <= 5f || this.CountEnemy > 2 || (this.playerPos.x > 64f && this.playerPos.x < 100f) || (this.playerPos.x > 120f && this.playerPos.x < 135f) || this.playerPos.x > 155f || !GameManager.Instance.player._controller.isGrounded)
					{
						return;
					}
					this.TimeCreateEnemy = 0f;
					bool flag = UnityEngine.Random.Range(0f, 1f) >= 0.5f;
					if (flag)
					{
						this.CreateEnemyKnife(8, (UnityEngine.Random.Range(0f, 1f) < 0.5f) ? CameraController.Instance.GetPosEnemyRandomRight() : CameraController.Instance.GetPosEnemyRandomLeft());
					}
					else
					{
						this.CreateEnemyKnife(8, CameraController.Instance.GetPosEnemyRandomLeft());
						this.CreateEnemyKnife(8, CameraController.Instance.GetPosEnemyRandomRight());
					}
					break;
				}
				}
			}
		}
	}

	private void OnUpdateInBossMode(float deltaTime)
	{
		if (!this.isInit)
		{
			return;
		}
		EBoss bossCurrent = ProfileManager.bossCurrent;
		if (bossCurrent == EBoss.Heroic_Mech || bossCurrent == EBoss.T_REX)
		{
			Vector2 position = GameManager.Instance.player.GetPosition();
			CameraController instance = CameraController.Instance;
			Vector3 position2 = CameraController.Instance.transform.position;
			this.TimeCreateEnemy += deltaTime;
			if (GameManager.Instance.ListEnemy.Count > 5 || this.TimeCreateEnemy <= 5f || !GameManager.Instance.player._controller.isGrounded)
			{
				return;
			}
			this.TimeCreateEnemy = 0f;
			bool flag = UnityEngine.Random.Range(0f, 1f) >= 0.5f;
			if (flag)
			{
				this.CreateEnemyKnife(3, (UnityEngine.Random.Range(0f, 1f) < 0.5f) ? instance.GetPosEnemyRandomRight() : instance.GetPosEnemyRandomLeft());
			}
			else
			{
				this.CreateEnemyKnife(3, instance.GetPosEnemyRandomLeft());
				this.CreateEnemyKnife(3, instance.GetPosEnemyRandomRight());
			}
		}
	}

	private void CreateEnemyKnife(int Level, Vector2 pos)
	{
		this.CountEnemy++;
		EnemyKnife enemy = EnemyManager.Instance.CreateEnemyKnife();
		EnemyDataInfo enemyDataInfo = new EnemyDataInfo();
		enemyDataInfo.pos_x = pos.x;
		enemyDataInfo.pos_y = pos.y;
		enemyDataInfo.ismove = true;
		enemyDataInfo.type = 7;
		enemy.Init(enemyDataInfo, delegate
		{
			EnemyManager.Instance.PoolEnemyKnife.Store(enemy);
		});
		EnemyKnife enemy2 = enemy;
		enemy2.OnEnemyDeaded = (Action)Delegate.Combine(enemy2.OnEnemyDeaded, new Action(delegate()
		{
			this.CountEnemy--;
		}));
		enemy.cacheEnemy.Vision_Min = 0.2f;
		enemy.cacheEnemy.Vision_Max = 8f;
		GameManager.Instance.ListEnemy.Add(enemy);
	}

	private void CreateEnemyParachute(int Level, Vector2 pos)
	{
		RaycastHit2D raycastHit2D = Physics2D.Raycast(pos, Vector2.down, float.PositiveInfinity, LayerMask.GetMask(this.arrLayerMask));
		if (raycastHit2D.collider == null || !raycastHit2D.collider.CompareTag("Ground") || raycastHit2D.collider.gameObject.layer == LayerMask.GetMask(new string[]
		{
			this.arrLayerMask[1]
		}))
		{
			return;
		}
		int num = UnityEngine.Random.Range(0, 4);
		this.CountEnemy++;
		switch (num)
		{
		case 0:
		{
			EnemyFire enemy1 = EnemyManager.Instance.CreateEnemyFire();
			EnemyDataInfo enemyDataInfo = new EnemyDataInfo();
			enemyDataInfo.ismove = true;
			enemyDataInfo.type = 3;
			enemyDataInfo.pos_x = pos.x;
			enemyDataInfo.pos_y = pos.y;
			enemy1.Init(enemyDataInfo, delegate
			{
				EnemyManager.Instance.PoolEnemyfire.Store(enemy1);
			});
			EnemyFire enemy = enemy1;
			enemy.OnEnemyDeaded = (Action)Delegate.Combine(enemy.OnEnemyDeaded, new Action(delegate()
			{
				this.CountEnemy--;
			}));
			enemy1.cacheEnemy.Vision_Min = 0.5f;
			enemy1.cacheEnemy.Vision_Max = 8f;
			GameManager.Instance.ListEnemy.Add(enemy1);
			enemy1.SetParachuter(0.3f);
			break;
		}
		case 1:
		case 2:
		{
			EnemyAkm enemy2 = EnemyManager.Instance.CreateEnemyAkm();
			EnemyDataInfo enemyDataInfo2 = new EnemyDataInfo();
			enemyDataInfo2.pos_x = pos.x;
			enemyDataInfo2.pos_y = pos.y;
			enemyDataInfo2.ismove = true;
			enemyDataInfo2.type = 1;
			enemy2.Init(enemyDataInfo2, delegate
			{
				EnemyManager.Instance.PoolAkm.Store(enemy2);
			});
			EnemyAkm enemy7 = enemy2;
			enemy7.OnEnemyDeaded = (Action)Delegate.Combine(enemy7.OnEnemyDeaded, new Action(delegate()
			{
				this.CountEnemy--;
			}));
			enemy2.cacheEnemyData.ismove = true;
			enemy2.cacheEnemy.Vision_Min = 3f;
			enemy2.cacheEnemy.Vision_Max = 9f;
			GameManager.Instance.ListEnemy.Add(enemy2);
			enemy2.SetParachuter(0.3f);
			break;
		}
		case 3:
		{
			EnemyGrenade enemy4 = EnemyManager.Instance.CreateEnemyGrenade();
			EnemyDataInfo enemyDataInfo = new EnemyDataInfo();
			enemyDataInfo.ismove = true;
			enemyDataInfo.type = 2;
			enemyDataInfo.pos_x = pos.x;
			enemyDataInfo.pos_y = pos.y;
			enemy4.Init(enemyDataInfo, delegate
			{
				EnemyManager.Instance.PoolEnemyGrenade.Store(enemy4);
			});
			EnemyGrenade enemy3 = enemy4;
			enemy3.OnEnemyDeaded = (Action)Delegate.Combine(enemy3.OnEnemyDeaded, new Action(delegate()
			{
				this.CountEnemy--;
			}));
			enemy4.cacheEnemy.Vision_Min = 3f;
			enemy4.cacheEnemy.Vision_Max = 9f;
			GameManager.Instance.ListEnemy.Add(enemy4);
			enemy4.SetParachuter(0.3f);
			break;
		}
		case 4:
		{
			EnemyPistol enemy5 = EnemyManager.Instance.CreateEnemyPistol();
			EnemyDataInfo enemyDataInfo = new EnemyDataInfo();
			enemyDataInfo.ismove = true;
			enemyDataInfo.type = 5;
			enemyDataInfo.pos_x = pos.x;
			enemyDataInfo.pos_y = pos.y;
			enemy5.Init(enemyDataInfo, delegate
			{
				EnemyManager.Instance.PoolEnemyPistol.Store(enemy5);
			});
			EnemyPistol enemy6 = enemy5;
			enemy6.OnEnemyDeaded = (Action)Delegate.Combine(enemy6.OnEnemyDeaded, new Action(delegate()
			{
				this.CountEnemy--;
			}));
			enemy5.cacheEnemy.Vision_Min = 3f;
			enemy5.cacheEnemy.Vision_Max = 9f;
			GameManager.Instance.ListEnemy.Add(enemy5);
			enemy5.SetParachuter(0.3f);
			break;
		}
		}
	}

	private bool isInit;

	private float TimeCreateEnemy;

	public int CountEnemy;

	private string[] arrLayerMask;

	public bool run;

	private int num;

	private float[] posCameraX1 = new float[3];

	private float[] posCameraX2 = new float[2];

	private float[] posCameraX;

	private Vector2 posEnemy;

	private Vector2 playerPos;
}
