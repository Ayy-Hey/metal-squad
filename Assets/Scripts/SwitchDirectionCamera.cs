using System;
using Com.LuisPedroFonseca.ProCamera2D;
using UnityEngine;

public class SwitchDirectionCamera : MonoBehaviour
{
	private void Start()
	{
		this.isStart = false;
		if (this.CameraCinematics != null)
		{
			this.CameraCinematics.OnCinematicFinished.AddListener(delegate()
			{
				GameManager.Instance.StateManager.EState = EGamePlay.RUNNING;
				GameManager.Instance.hudManager.ShowControl(1.1f);
				CameraController.Instance.NewCheckpoint(true, 15f);
			});
		}
		if (this.gate != null)
		{
			GateCampaign gateCampaign = this.gate;
			gateCampaign.OnOpenNewMapStarted = (Action)Delegate.Combine(gateCampaign.OnOpenNewMapStarted, new Action(delegate()
			{
				CameraController.Instance.NumericBoundaries.RightBoundary = this.RightBoundary;
				CameraController.Instance.NumericBoundaries.LeftBoundary = this.LeftBoundary;
				CameraController.Instance.NumericBoundaries.TopBoundary = 100f;
				CameraController.Instance.orientaltion = this.oriental;
			}));
			GateCampaign gateCampaign2 = this.gate;
			gateCampaign2.OnOpenNewMapEnded = (Action)Delegate.Combine(gateCampaign2.OnOpenNewMapEnded, new Action(delegate()
			{
				this.PlayCinematics();
				if (this.objClear != null)
				{
					this.objClear.SetActive(true);
				}
			}));
		}
	}

	private void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.CompareTag("Rambo"))
		{
			CameraController.Instance.orientaltion = this.oriental;
			CameraController.Instance.NewCheckpoint(true, 15f);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Rambo"))
		{
			Log.Info("change orientation to" + this.oriental);
			CameraController.Instance.orientaltion = this.oriental;
			CameraController.Instance.NewCheckpoint(true, 15f);
		}
	}

	private void PlayCinematics()
	{
		if (this.isStart)
		{
			return;
		}
		this.isStart = true;
		GameManager.Instance.player._PlayerInput.OnRemoveInput(false);
		GameManager.Instance.player._PlayerSpine.OnIdle(true);
		GameManager.Instance.StateManager.EState = EGamePlay.PAUSE;
		CameraController.Instance.orientaltion = this.oriental;
		GameManager.Instance.hudManager.HideControl();
		this.CameraCinematics.Play();
	}

	[SerializeField]
	private ProCamera2DCinematics CameraCinematics;

	[SerializeField]
	private CameraController.Orientation oriental = CameraController.Orientation.HORIZONTAL;

	[SerializeField]
	private float LeftBoundary = 160f;

	[SerializeField]
	private float RightBoundary = 173f;

	[SerializeField]
	private GateCampaign gate;

	private bool isStart;

	[SerializeField]
	private GameObject objClear;
}
