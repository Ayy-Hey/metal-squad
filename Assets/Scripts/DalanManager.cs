using System;
using UnityEngine;

public class DalanManager : MonoBehaviour
{
	public void OnBegin()
	{
		if (this.isbegin)
		{
			return;
		}
		this.isbegin = true;
		this.timeCounter = float.MaxValue;
	}

	public void OnStop()
	{
		this.isbegin = false;
	}

	public void OnUpdate(float deltaTime)
	{
		if (!this.isbegin)
		{
			return;
		}
		this.timeCounter += deltaTime;
		if (this.timeCounter >= 4f)
		{
			TrapManager.Instance.CreateDalan(this.tfNguonPhat.position);
			this.timeCounter = 0f;
		}
	}

	[SerializeField]
	private Transform tfNguonPhat;

	private bool isbegin;

	private float timeCounter;
}
