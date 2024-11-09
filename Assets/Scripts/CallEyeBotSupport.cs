using System;
using UnityEngine;

public class CallEyeBotSupport : MonoBehaviour
{
	public void CallSupport(PlayerMain player)
	{
		if (this.support != null && this.isLive && GameMode.Instance.modePlay != GameMode.ModePlay.CoOpMode)
		{
			this.support.AddHealthPoint(this.support.MAXHPCurrent, EWeapon.NONE);
		}
		else
		{
			this.support = (UnityEngine.Object.Instantiate(Resources.Load("GameObject/EyeBot", typeof(EyeBotSupporter))) as EyeBotSupporter);
			this.support.gameObject.SetActive(true);
			this.support.transform.localScale = Vector3.one * 0.7f;
			this.support.transform.position = this.posRambo.position;
			this.support.OnBegin(player);
			EyeBotSupporter eyeBotSupporter = this.support;
			eyeBotSupporter.dieCallBack = (Action<bool>)Delegate.Combine(eyeBotSupporter.dieCallBack, new Action<bool>(delegate(bool live)
			{
				this.isLive = live;
			}));
			GameManager.Instance.ListRambo.Add(this.support);
			Vector2 v = new Vector2(CameraController.Instance.transform.position.x - 12.6f, CameraController.Instance.transform.position.y + 1f);
			this.support.transform.position = v;
			this.isLive = true;
		}
	}

	private bool isLive;

	public Transform posRambo;

	public LayerMask layerGround;

	private EyeBotSupporter support;
}
