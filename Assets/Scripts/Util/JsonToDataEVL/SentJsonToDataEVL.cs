using System;
using MyDataLoader;
using Newtonsoft.Json;
using UnityEngine;

namespace Util.JsonToDataEVL
{
	public class SentJsonToDataEVL : MonoBehaviour
	{
		private void OnValidate()
		{
			if (this.sentData)
			{
				this.sentData = false;
				if (!this.isArrayJson)
				{
					EnemyCharactor enemyCharactor = JsonConvert.DeserializeObject<EnemyCharactor>(this.txt.text);
					int num = enemyCharactor.enemy.Length;
					EVL[] array = new EVL[num];
					for (int i = 0; i < num; i++)
					{
						array[i].hp = enemyCharactor.enemy[i].HP;
						array[i].speed = enemyCharactor.enemy[i].Speed;
						array[i].maxVision = enemyCharactor.enemy[i].Vision_Max;
						if (this.lv2)
						{
							array[i].damage = enemyCharactor.enemy[i].DamageLv2;
							array[i].timeReload = enemyCharactor.enemy[i].Time_Reload_AttackLv2;
						}
						else
						{
							array[i].damage = enemyCharactor.enemy[i].Damage;
							array[i].timeReload = enemyCharactor.enemy[i].Time_Reload_Attack;
						}
					}
					this.data.datas = array;
				}
				else
				{
					ArrayDataEVL arrayDataEVL = JsonConvert.DeserializeObject<ArrayDataEVL>(this.txt.text);
					int num2 = arrayDataEVL.HP.Length;
					EVL[] array2 = new EVL[num2];
					for (int j = 0; j < num2; j++)
					{
						array2[j].hp = arrayDataEVL.HP[j];
						array2[j].speed = arrayDataEVL.Speed[j];
						array2[j].maxVision = arrayDataEVL.MaxVision[j];
						array2[j].damage = arrayDataEVL.Damage[j];
						array2[j].timeReload = arrayDataEVL.TimeReload[j];
					}
					this.data.datas = array2;
				}
			}
		}

		public bool isArrayJson;

		public TextAsset txt;

		public DataEVL data;

		public bool sentData;

		public bool lv2;
	}
}
