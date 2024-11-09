using System;
using UnityEngine;

public class Level17 : MonoBehaviour
{
	private void Start()
	{
		GateCampaign gateCampaign = this.gate;
		gateCampaign.OnOpenNewMapStarted = (Action)Delegate.Combine(gateCampaign.OnOpenNewMapStarted, new Action(delegate()
		{
			this.isInit = true;
			this.Layer1.gameObject.SetActive(false);
			this.Layer2.gameObject.SetActive(true);
		}));
	}

	private void Update()
	{
		if (!this.isInit)
		{
			return;
		}
		if (this.Layer2 != null && this.Layer2.tfChild != null)
		{
			this.Layer2.tfChild.localScale = Vector3.one * (CameraController.Instance.mCamera.orthographicSize / 3.6f);
		}
	}

	public GateCampaign gate;

	public ParallaxLayer Layer1;

	public ParallaxLayer Layer2;

	private bool isInit;
}
