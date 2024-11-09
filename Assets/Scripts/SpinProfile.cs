using System;
using System.Collections.Generic;
using CustomData;
using UnityEngine;

public class SpinProfile
{
	public SpinProfile()
	{
		this.freeSpin = new BoolProfileData("com.rambo.metal.squad.freespin", false);
		this.waitTime = new IntProfileData("com.rambo.spin.wait.time", GameConfig.DEFAULT_SPIN_WAITTIME);
		this.totalSpin = new IntProfileData("com.metal.squad.spin.total", 0);
		this.spinData = Resources.Load<SpinData>("Spin/SpinData");
		this.idGiftPageSpin = new IntProfileData("com.metal.squad.spin.page.id", 0);
		this.idItemSlot1 = new IntProfileData("random.item1", 51);
		this.idItemSlot4 = new IntProfileData("random.item4", 5);
		this.idItemSlot7 = new IntProfileData("random.item7", 42);
		this.idItemSlot8 = new IntProfileData("random.item8", 41);
		this.idGiftPageSpin.setValue(0);
	}

	public void OnRandomID()
	{
		List<Item> list = new List<Item>();
		list.Add(Item.John_D_Fragment);
		list.Add(Item.Yoo_na_Fragment);
		list.Add(Item.Dvornikov_Fragment);
		this.idItemSlot1.setValue((int)list[UnityEngine.Random.Range(0, list.Count)]);
		list.Clear();
		list.Add(Item.Thunder_Shot_Fragment);
		list.Add(Item.Rocket_Fragment);
		list.Add(Item.Medkit);
		list.Add(Item.MGL_140_Fragment);
		list.Add(Item.Spread_Gun_Fragment);
		this.idItemSlot4.setValue((int)list[UnityEngine.Random.Range(0, list.Count)]);
		list.Clear();
		list.Add(Item.Machine_Gun_Fragment);
		list.Add(Item.MGL_140_Fragment);
		list.Add(Item.Ice_Gun_Fragment);
		list.Add(Item.Fc10_Gun_Fragment);
		this.idItemSlot7.setValue((int)list[UnityEngine.Random.Range(0, list.Count)]);
		list.Clear();
		list.Add(Item.Sniper_Fragment);
		list.Add(Item.Laser_Gun_Fragment);
		list.Add(Item.Ct9_Gun_Fragment);
		list.Add(Item.Flame_Gun_Fragment);
		this.idItemSlot8.setValue((int)list[UnityEngine.Random.Range(0, list.Count)]);
		list.Clear();
	}

	public int TotalSpin
	{
		get
		{
			return this.totalSpin.Data.Value;
		}
		set
		{
			this.totalSpin.setValue(value);
		}
	}

	public bool Free
	{
		get
		{
			return this.freeSpin.Data;
		}
		set
		{
			this.freeSpin.setValue(value);
			if (value)
			{
				this.ResetWaitTime();
			}
		}
	}

	public int WaitTime
	{
		get
		{
			return this.waitTime.Data;
		}
		set
		{
			this.waitTime.setValue(value);
		}
	}

	public void ResetWaitTime()
	{
		VipLevel vipCurrent = VipManager.Instance.GetVipCurrent();
		int default_SPIN_WAITTIME = GameConfig.DEFAULT_SPIN_WAITTIME;
		this.waitTime.setValue(default_SPIN_WAITTIME);
	}

	private void ResetPageSpin()
	{
		int value = UnityEngine.Random.Range(0, this.spinData.giftPages.Length);
		this.idGiftPageSpin.setValue(value);
	}

	internal int GetIdPageSpin()
	{
		return this.idGiftPageSpin.Data.Value;
	}

	internal Item GetSpinGift(int idGift)
	{
		Item result;
		try
		{
			if (idGift == 1)
			{
				result = (Item)this.idItemSlot1.Data.Value;
			}
			else if (idGift == 4)
			{
				result = (Item)this.idItemSlot4.Data.Value;
			}
			else if (idGift == 7)
			{
				result = (Item)this.idItemSlot7.Data.Value;
			}
			else if (idGift == 8)
			{
				result = (Item)this.idItemSlot8.Data.Value;
			}
			else
			{
				result = this.spinData.giftPages[this.GetIdPageSpin()].datas[idGift].gift;
			}
		}
		catch
		{
			result = this.spinData.giftPages[this.GetIdPageSpin()].datas[0].gift;
		}
		return result;
	}

	internal int GetSpinAmount(int idGift)
	{
		int amount;
		try
		{
			amount = this.spinData.giftPages[this.GetIdPageSpin()].datas[idGift].amount;
		}
		catch
		{
			amount = this.spinData.giftPages[this.GetIdPageSpin()].datas[0].amount;
		}
		return amount;
	}

	internal float GetSpinRate(int idGift)
	{
		float rate;
		try
		{
			rate = this.spinData.giftPages[this.GetIdPageSpin()].datas[idGift].rate;
		}
		catch
		{
			rate = this.spinData.giftPages[this.GetIdPageSpin()].datas[0].rate;
		}
		return rate;
	}

	internal void ClampSpinGift(int idGift)
	{
		PopupManager.Instance.SaveReward(this.GetSpinGift(idGift), Mathf.Max(0, this.GetSpinAmount(idGift)), "Spin", null);
	}

	private BoolProfileData freeSpin;

	private IntProfileData totalSpin;

	private IntProfileData waitTime;

	private SpinData spinData;

	private IntProfileData idGiftPageSpin;

	private IntProfileData idItemSlot1;

	private IntProfileData idItemSlot4;

	private IntProfileData idItemSlot7;

	private IntProfileData idItemSlot8;
}
