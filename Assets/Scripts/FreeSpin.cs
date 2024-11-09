using System;
using UnityEngine;
using UnityEngine.UI;

public class FreeSpin : MonoBehaviour
{
	private void OnEnable()
	{
		this.ShowBoder(-1);
	}

	private void Update()
	{
		if (this.isSpin)
		{
			this.LoopSpin();
		}
	}

	public void Spin()
	{
		this.isSpin = true;
		this.oldTime = Time.time;
	}

	private void LoopSpin()
	{
		if (1f < this.deltaTime && this.spinId == this.lockSpinId)
		{
			this.isSpin = false;
			this.SpinDone(this.lockSpinId);
			return;
		}
		this.nowTime = Time.time;
		if (this.deltaTime <= this.nowTime - this.oldTime)
		{
			this.spinId = ((this.spinId != 7) ? (this.spinId + 1) : 0);
			this.oldTime = this.nowTime;
			this.deltaTime += Time.deltaTime / 3f;
		}
	}

	private void SpinDone(int itemId)
	{
	}

	private void ShowBoder(int id)
	{
		for (int i = 0; i < this.imgBoders.Length; i++)
		{
			this.imgBoders[i].enabled = (i == id);
		}
	}

	public Image[] imgBoders;

	private int lockSpinId;

	private int spinId;

	private float nowTime;

	private float oldTime;

	private float deltaTime;

	private bool isSpin;
}
