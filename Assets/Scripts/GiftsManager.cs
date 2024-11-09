using System;
using System.Collections.Generic;
using UnityEngine;

public class GiftsManager : MonoBehaviour
{
	public bool IsInit { get; set; }

	public void Init()
	{
		if (this.IsInit)
		{
			return;
		}
		this.IsInit = true;
		this.ListItemGun = new List<ItemGun>();
		this.PoolItemGun = new ObjectPooling<ItemGun>(0, null, null);
		this.GiftPool = new ObjectPooling<Gift>(5, new Action<Gift>(this.ResetGift), new Action<Gift>(this.InitGift));
		for (int i = 0; i < this.ListGift.Count; i++)
		{
			this.GiftPool.Store(this.ListGift[i]);
		}
		this.listIDGunSpecial = new List<int>();
		for (int j = 0; j < 6; j++)
		{
			if (ProfileManager.weaponsSpecial[j].GetGunBuy())
			{
				this.TotalGunReady++;
				this.listIDGunSpecial.Add(j);
			}
		}
	}

	public void OnUpdate(float deltaTime)
	{
		for (int i = 0; i < this.ListItemGun.Count; i++)
		{
			if (this.ListItemGun[i] != null && this.ListItemGun[i].gameObject.activeSelf)
			{
				this.ListItemGun[i].OnUpdate(deltaTime);
			}
		}
	}

	public void Create(Vector3 pos, int gift_value)
	{
		pos.y += 1f;
		Gift gift = this.GiftPool.New();
		if (gift == null)
		{
			gift = UnityEngine.Object.Instantiate<Transform>(this.ListGift[0].transform).GetComponent<Gift>();
			gift.transform.parent = this.ListGift[0].transform.parent;
			this.ListGift.Add(gift);
		}
		gift.Show((Gift.ETYPE)gift_value, pos);
	}

	public void CreateItemWeapon(Vector3 pos)
	{
		if (this.TotalGunReady <= 0)
		{
			return;
		}
		float num = UnityEngine.Random.Range(0f, 1f);
		float num2 = 0.03f;
		if (GameMode.Instance.modePlay == GameMode.ModePlay.Boss_Mode)
		{
			if (!this.isDropGun && GameManager.Instance.hudManager.arrLineBloodBoss[1].fillAmount <= 0.5f)
			{
				num2 = 0.5f;
			}
		}
		else if (!this.isDropGun && GameManager.Instance.StateManager.popupEndMission.miniMap.percent_distance_completed >= 0.5f)
		{
			num2 = 0.5f;
		}
		if (num > num2)
		{
			return;
		}
		ItemGun itemGun = this.PoolItemGun.New();
		if (itemGun == null)
		{
			string path = "GameObject/Items/ItemGun";
			itemGun = (UnityEngine.Object.Instantiate(Resources.Load(path, typeof(ItemGun)), base.transform) as ItemGun);
			this.ListItemGun.Add(itemGun);
		}
		int num3 = UnityEngine.Random.Range(0, this.listIDGunSpecial.Count);
		if (num3 >= this.listIDGunSpecial.Count)
		{
			num3 = 0;
		}
		int id = this.listIDGunSpecial[num3];
		itemGun.Show(id, pos);
		this.isDropGun = true;
	}

	public void DestroyObject()
	{
		this.IsInit = false;
		for (int i = 0; i < this.ListGift.Count; i++)
		{
			if (this.ListGift[i] != null && this.ListGift[i].gameObject.activeSelf)
			{
				this.ListGift[i].gameObject.SetActive(false);
			}
		}
		for (int j = 0; j < this.ListItemGun.Count; j++)
		{
			if (this.ListItemGun[j] != null && this.ListItemGun[j].gameObject.activeSelf)
			{
				this.ListItemGun[j].gameObject.SetActive(false);
			}
		}
	}

	private void ResetGift(Gift gift)
	{
	}

	private void InitGift(Gift gift)
	{
	}

	public List<Gift> ListGift;

	public ObjectPooling<Gift> GiftPool;

	private int TotalGunReady;

	private List<int> listIDGunSpecial;

	private List<ItemGun> ListItemGun;

	public ObjectPooling<ItemGun> PoolItemGun;

	private bool isDropGun;
}
