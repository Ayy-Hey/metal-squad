using System;
using System.Collections;
using UnityEngine;

public class ULaser : MonoBehaviour
{
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(2f);
		if (this.line1 != null)
		{
			this.line1.sortingLayerName = "Gameplay";
		}
		if (this.line2 != null)
		{
			this.line2.sortingLayerName = "Gameplay";
		}
		this.objLaser.SetActive(false);
		this.Step = 0;
		this.isInit = true;
		yield break;
	}

	private void LateUpdate()
	{
		if (!this.isInit)
		{
			return;
		}
		if (!CameraController.Instance.IsInView(base.transform))
		{
			this.objLaser.SetActive(false);
			return;
		}
		if (GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			this.objLaser.SetActive(false);
			if (this.objSprite != null)
			{
				this.objSprite.SetActive(false);
			}
			return;
		}
		if (this.objSprite != null)
		{
			this.objSprite.SetActive(true);
		}
		this.Time_Reload += Time.deltaTime;
		int step = this.Step;
		if (step != 0)
		{
			if (step == 1)
			{
				if (this.Time_Reload >= this.TimeDisActive)
				{
					this.objLaser.SetActive(false);
					this.Step = 0;
					this.Time_Reload = 0f;
				}
			}
		}
		else if (this.Time_Reload >= this.TimeActive)
		{
			this.objLaser.SetActive(true);
			this.Step = 1;
			this.Time_Reload = 0f;
		}
	}

	[SerializeField]
	private LineRenderer line1;

	[SerializeField]
	private LineRenderer line2;

	[SerializeField]
	private GameObject objLaser;

	[SerializeField]
	private GameObject objSprite;

	private float Time_Reload;

	private int Step;

	[SerializeField]
	private float TimeActive = 3f;

	[SerializeField]
	private float TimeDisActive = 3f;

	private bool isInit;
}
