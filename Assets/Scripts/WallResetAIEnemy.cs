using System;
using System.Collections.Generic;
using UnityEngine;

public class WallResetAIEnemy : MonoBehaviour
{
	private void OnDisable()
	{
		for (int i = 0; i < this.ListBaseEnemy.Count; i++)
		{
			this.ListBaseEnemy[i].ResetAIEnemy(false);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		BaseEnemy baseEnemy = null;
		try
		{
			baseEnemy = other.transform.parent.GetComponent<BaseEnemy>();
		}
		catch
		{
		}
		if (baseEnemy != null && !this.ListBaseEnemy.Contains(baseEnemy) && !base.gameObject.CompareTag("Found_Jump"))
		{
			this.ListBaseEnemy.Add(baseEnemy);
			BaseEnemy baseEnemy2 = baseEnemy;
			baseEnemy2.OnEnemyDeaded = (Action)Delegate.Combine(baseEnemy2.OnEnemyDeaded, new Action(delegate()
			{
				this.ListBaseEnemy.Remove(baseEnemy);
			}));
		}
	}

	private List<BaseEnemy> ListBaseEnemy = new List<BaseEnemy>();
}
