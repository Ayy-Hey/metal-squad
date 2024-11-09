using System;
using Com.LuisPedroFonseca.ProCamera2D;
using UnityEngine;

public class StartGateCampaign : MonoBehaviour
{
	private void Start()
	{
		BaseTrigger baseTrigger = this.triggerCameraPro;
		baseTrigger.OnEnteredTrigger = (Action)Delegate.Combine(baseTrigger.OnEnteredTrigger, new Action(delegate()
		{
			if (this.Gate.isStartRun)
			{
				return;
			}
			for (int i = 0; i < GameManager.Instance.ListEnemy.Count; i++)
			{
				GameManager.Instance.ListEnemy[i].AddHealthPoint(-10000f, EWeapon.EXCEPTION);
			}
			LeanTween.cancel(CameraController.Instance.gameObject);
			CameraController.Instance.NumericBoundaries.RightBoundary = this.RightBoundaryCameraStop;
			this.Gate.isStartRun = true;
			GameManager.Instance.player.isAutoRun = true;
			GameManager.Instance.StateManager.EState = EGamePlay.PAUSE;
			GameManager.Instance.hudManager.HideControl();
			GameManager.Instance.player.GunCurrent.WeaponCurrent.OnRelease();
			GameManager.Instance.player._PlayerInput.OnRemoveInput(false);
		}));
	}

	[SerializeField]
	private GateCampaign Gate;

	[SerializeField]
	private float RightBoundaryCameraStop = 160f;

	[SerializeField]
	private BaseTrigger triggerCameraPro;
}
