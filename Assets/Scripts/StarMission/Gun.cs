using System;
using System.Collections.Generic;
using UnityEngine;

namespace StarMission
{
	public class Gun
	{
		public void AddEnemy(int id)
		{
			try
			{
				int value = this.GetTotalEnemy(id) + 1;
				this.EnemyContain[id] = value;
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.Log("Exeption: " + arg);
			}
		}

		public int GetTotalEnemy(int id)
		{
			int result = 0;
			try
			{
				this.EnemyContain.TryGetValue(id, out result);
			}
			catch
			{
			}
			return result;
		}

		public int GetTotalEnemy()
		{
			int num = 0;
			foreach (KeyValuePair<int, int> keyValuePair in this.EnemyContain)
			{
				num += keyValuePair.Value;
			}
			return num;
		}

		public bool isActived;

		private Dictionary<int, int> EnemyContain = new Dictionary<int, int>();

		public int TotalCombo;
	}
}
