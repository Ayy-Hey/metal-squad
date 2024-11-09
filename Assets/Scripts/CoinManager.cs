using System;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
	public void OnInit()
	{
		GameMode.GameStyle style = GameMode.Instance.Style;
		if (style != GameMode.GameStyle.MultiPlayer)
		{
			if (style == GameMode.GameStyle.SinglPlayer)
			{
				GameMode.ModePlay modePlay = GameMode.Instance.modePlay;
				if (modePlay == GameMode.ModePlay.Campaign || modePlay == GameMode.ModePlay.Special_Campaign)
				{
					this.SetEnemyDropCoinCampaign();
				}
			}
		}
		this.PoolingCoin = new ObjectPooling<Coin>(3, null, null);
		for (int i = 0; i < this.ListCoin.Count; i++)
		{
			this.PoolingCoin.Store(this.ListCoin[i]);
		}
		this.isInit = true;
		this.showGold.OnInit();
	}

	public void SetEnemyDropCoinCampaign()
	{
		UnityEngine.Debug.Log("COIN_DROP:" + GameManager.Instance.MAX_COIN_DROP);
		if (GameMode.Instance.MODE == GameMode.Mode.TUTORIAL)
		{
			return;
		}
		int num = 0;
		bool flag = true;
		int num2 = 0;
		for (int i = 0; i < DataLoader.LevelDataCurrent.points.Count; i++)
		{
			Points points = DataLoader.LevelDataCurrent.points[i];
			num += points.enemyData.enemyDataInfo.Count;
		}
		while (GameManager.Instance.MAX_COIN_DROP > 0 && flag)
		{
			for (int j = DataLoader.LevelDataCurrent.points.Count - 1; j >= 0; j--)
			{
				Points points2 = DataLoader.LevelDataCurrent.points[j];
				for (int k = 0; k < points2.enemyData.enemyDataInfo.Count; k++)
				{
					UnityEngine.Debug.Log("COIN_DROP:" + GameManager.Instance.MAX_COIN_DROP);
					if (GameManager.Instance.MAX_COIN_DROP <= 0)
					{
						return;
					}
					EnemyDataInfo enemyDataInfo = points2.enemyData.enemyDataInfo[k];
					float num3 = UnityEngine.Random.Range(0f, 1f);
					if (num3 > 0.7f && !enemyDataInfo.DropCoin)
					{
						int[] array = new int[]
						{
							1,
							1,
							1,
							2,
							2,
							2,
							3,
							3,
							3,
							3
						};
						int num4 = array[UnityEngine.Random.Range(0, array.Length)];
						GameManager.Instance.MAX_COIN_DROP -= num4 * 3;
						enemyDataInfo.DropCoin = true;
						enemyDataInfo.ValueCoin = num4;
					}
					if (enemyDataInfo.DropCoin)
					{
						num2++;
						if (num2 >= num)
						{
							return;
						}
					}
				}
			}
		}
	}

	public void SetEnemyDropCoin(int coinDrop)
	{
		int num = coinDrop;
		int num2 = 0;
		bool flag = true;
		int num3 = 0;
		for (int i = 0; i < DataLoader.LevelDataCurrent.points.Count; i++)
		{
			Points points = DataLoader.LevelDataCurrent.points[i];
			num2 += points.enemyData.enemyDataInfo.Count;
		}
		while (num > 0 && flag)
		{
			for (int j = 0; j < DataLoader.LevelDataCurrent.points.Count; j++)
			{
				Points points2 = DataLoader.LevelDataCurrent.points[j];
				for (int k = 0; k < points2.enemyData.enemyDataInfo.Count; k++)
				{
					if (num <= 0)
					{
						return;
					}
					EnemyDataInfo enemyDataInfo = points2.enemyData.enemyDataInfo[k];
					float num4 = UnityEngine.Random.Range(0f, 1f);
					if (num4 > 0.8f && !enemyDataInfo.DropCoin)
					{
						int[] array = new int[]
						{
							2,
							2,
							2,
							3,
							3,
							3,
							4,
							4,
							5,
							5
						};
						int num5 = array[UnityEngine.Random.Range(0, array.Length)];
						num -= num5 * 3;
						enemyDataInfo.DropCoin = true;
						enemyDataInfo.ValueCoin = num5;
					}
					if (enemyDataInfo.DropCoin)
					{
						num3++;
						if (num3 >= num2)
						{
							return;
						}
					}
				}
			}
		}
	}

	public void CreateCoin(int value, Vector2 pos)
	{
		for (int i = 0; i < value; i++)
		{
			Coin coin = this.CreateCoin(pos);
			coin.Show();
		}
	}

	public void OnUpdate(float deltaTime)
	{
		if (!this.isInit)
		{
			return;
		}
		for (int i = 0; i < this.ListCoin.Count; i++)
		{
			if (this.ListCoin[i] != null && this.ListCoin[i].gameObject.activeSelf)
			{
				this.ListCoin[i].OnUpdate(deltaTime);
			}
		}
	}

	private Coin CreateCoin(Vector3 pos)
	{
		Coin coin = this.PoolingCoin.New();
		if (coin == null)
		{
			coin = UnityEngine.Object.Instantiate<Transform>(this.ListCoin[0].transform).GetComponent<Coin>();
			coin.transform.parent = this.ListCoin[0].transform.parent;
			this.ListCoin.Add(coin);
		}
		coin.gameObject.SetActive(true);
		coin.transform.position = pos;
		return coin;
	}

	public void CoinCollected()
	{
		this.showGold.Show();
	}

	public void DestroyAll()
	{
		for (int i = 0; i < this.ListCoin.Count; i++)
		{
			if (this.ListCoin[i] != null && this.ListCoin[i].gameObject.activeSelf)
			{
				this.ListCoin[i].gameObject.SetActive(false);
			}
		}
	}

	public List<Coin> ListCoin;

	public ObjectPooling<Coin> PoolingCoin;

	[SerializeField]
	private ShowGoldUI showGold;

	private bool isInit;
}
