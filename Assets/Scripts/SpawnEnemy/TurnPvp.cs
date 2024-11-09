using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpawnEnemy
{
	public class TurnPvp : Turn
	{
		public void Init()
		{
			this.lstEnemy = new List<Enemy>();
			this.CreatAllEnemy();
			this.enemies = this.lstEnemy.ToArray();
			foreach (Enemy obj in this.lstEnemy)
			{
				MonoSingleton<EnemyPoolPvp>.Instance.EnemyPool.Store(obj);
			}
			this.vtBoder = Camera.main.ScreenToWorldPoint(new Vector3((float)Screen.width, (float)Screen.height, 0f));
			for (int i = 0; i < this.noParachuteSpawnAreas.Length; i++)
			{
				Vector3 position = this.noParachuteSpawnAreas[i].transform.position;
				if (position.x < 0f)
				{
					position.x = -this.vtBoder.x;
				}
				else
				{
					position.x = this.vtBoder.x;
				}
				this.noParachuteSpawnAreas[i].transform.position = position;
			}
			for (int j = 0; j < this.noParachuteGroundSpawnAreas.Length; j++)
			{
				Vector3 position2 = this.noParachuteGroundSpawnAreas[j].transform.position;
				if (position2.x < 0f)
				{
					position2.x = -this.vtBoder.x;
				}
				else
				{
					position2.x = this.vtBoder.x;
				}
				this.noParachuteGroundSpawnAreas[j].transform.position = position2;
			}
			for (int k = 0; k < this.parachuteSpawnAreas.Length; k++)
			{
				Vector3 position3 = this.parachuteSpawnAreas[k].transform.position;
				position3.y = this.vtBoder.y;
				this.parachuteSpawnAreas[k].transform.position = position3;
			}
			for (int l = 0; l < this.MaybaySpawnAreas.Length; l++)
			{
				Vector3 position4 = this.MaybaySpawnAreas[l].transform.position;
				position4.y = this.vtBoder.y;
				position4.x = this.vtBoder.x;
				this.MaybaySpawnAreas[l].transform.position = position4;
			}
		}

		private void CreatAllEnemy()
		{
			for (int i = 0; i < this.enemyCount; i++)
			{
				this.CreateRandomEnemy(this.Types[UnityEngine.Random.Range(0, 100) % this.Types.Length]);
			}
		}

		private void CreateRandomEnemy(ETypeEnemy type)
		{
			TurnPvp.TypeEnemyStart isParachute = this.GetIsParachute(type);
			SpawnArea spawnArea;
			switch (isParachute)
			{
			case TurnPvp.TypeEnemyStart.NO_PARACHUTE:
				spawnArea = this.noParachuteSpawnAreas[UnityEngine.Random.Range(0, 100) % this.noParachuteSpawnAreas.Length];
				break;
			case TurnPvp.TypeEnemyStart.NO_PARACHUTE_GROUND:
				spawnArea = this.noParachuteGroundSpawnAreas[UnityEngine.Random.Range(0, 100) % this.noParachuteGroundSpawnAreas.Length];
				break;
			case TurnPvp.TypeEnemyStart.PARACHUTE:
				spawnArea = this.parachuteSpawnAreas[UnityEngine.Random.Range(0, 100) % this.parachuteSpawnAreas.Length];
				break;
			case TurnPvp.TypeEnemyStart.MAYBAY:
				spawnArea = this.MaybaySpawnAreas[UnityEngine.Random.Range(0, 100) % this.MaybaySpawnAreas.Length];
				break;
			default:
				spawnArea = this.noParachuteSpawnAreas[UnityEngine.Random.Range(0, 100) % this.noParachuteSpawnAreas.Length];
				break;
			}
			this.CreateEnemyGameObject(type, spawnArea, isParachute);
		}

		private void CreateEnemyGameObject(ETypeEnemy type, SpawnArea spawnArea, TurnPvp.TypeEnemyStart typeEnemyStart)
		{
			Enemy enemy = MonoSingleton<EnemyPoolPvp>.Instance.CreateEnemy();
			enemy.gameObject.name = type.ToString();
			enemy.transform.position = new Vector3(UnityEngine.Random.Range(spawnArea.min.x, spawnArea.max.x), UnityEngine.Random.Range(spawnArea.min.y, spawnArea.max.y), 0f);
			enemy.TimeDelay = 0f;
			enemy.isParachute = (typeEnemyStart == TurnPvp.TypeEnemyStart.PARACHUTE);
			enemy.isMove = true;
			enemy.Type = type;
			enemy.TimeDelay = UnityEngine.Random.Range(0f, 2f);
			this.lstEnemy.Add(enemy);
		}

		private TurnPvp.TypeEnemyStart GetIsParachute(ETypeEnemy type)
		{
			TurnPvp.TypeEnemyStart result;
			switch (type)
			{
			case ETypeEnemy.SNIPER:
			case ETypeEnemy.TANKER:
			case ETypeEnemy.GRENADES:
			case ETypeEnemy.FIRE:
			case ETypeEnemy.ROCKET:
			case ETypeEnemy.PISTOL:
			case ETypeEnemy.KNIFE:
				result = ((UnityEngine.Random.Range(0f, 1f) <= 0.5f) ? TurnPvp.TypeEnemyStart.PARACHUTE : TurnPvp.TypeEnemyStart.NO_PARACHUTE);
				break;
			default:
				if (type != ETypeEnemy.HELICOPTER && type != ETypeEnemy.AIRPLANE_BOMB)
				{
					result = TurnPvp.TypeEnemyStart.NO_PARACHUTE;
				}
				else
				{
					result = TurnPvp.TypeEnemyStart.MAYBAY;
				}
				break;
			case ETypeEnemy.BUG_BOMB:
			case ETypeEnemy.NHEN_NHAY:
				result = ((UnityEngine.Random.Range(0, 2) != 1) ? TurnPvp.TypeEnemyStart.NO_PARACHUTE_GROUND : TurnPvp.TypeEnemyStart.NO_PARACHUTE);
				break;
			case ETypeEnemy.ROCKET_MOUSE:
			case ETypeEnemy.SAU:
			case ETypeEnemy.MINI_SPIDER:
			case ETypeEnemy.TANK_1:
			case ETypeEnemy.TANK_2:
			case ETypeEnemy.TANK_3:
				result = TurnPvp.TypeEnemyStart.NO_PARACHUTE_GROUND;
				break;
			}
			return result;
		}

		public SpawnArea[] MaybaySpawnAreas;

		public SpawnArea[] parachuteSpawnAreas;

		public SpawnArea[] noParachuteSpawnAreas;

		public SpawnArea[] noParachuteGroundSpawnAreas;

		public ETypeEnemy[] Types;

		public int enemyCount;

		private List<Enemy> lstEnemy;

		private Vector3 vtBoder;

		public enum TypeEnemyStart
		{
			NO_PARACHUTE,
			NO_PARACHUTE_GROUND,
			PARACHUTE,
			MAYBAY
		}
	}
}
