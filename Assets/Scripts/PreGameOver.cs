using System;
using Com.LuisPedroFonseca.ProCamera2D;
using UnityEngine;
using UnityEngine.Events;

public class PreGameOver : MonoBehaviour
{
	public static PreGameOver Instance
	{
		get
		{
			if (PreGameOver.instance == null)
			{
				PreGameOver.instance = UnityEngine.Object.FindObjectOfType<PreGameOver>();
			}
			return PreGameOver.instance;
		}
	}

	public bool isStart { get; set; }

	private void OnValidate()
	{
		if (this.hasBoss)
		{
			Log.Info("--------------> level boss:" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
		}
	}

	public void OnInit()
	{
		if (this.isinit)
		{
			return;
		}
		this.isinit = true;
		this.OnStarted = (Action)Delegate.Combine(this.OnStarted, new Action(delegate()
		{
			this.isStart = true;
			GameManager.Instance.hudManager.HideControl();
			GameManager.Instance.StateManager.SetPreview();
			if (this.hasBoss)
			{
				GameManager.Instance.audioManager.Boss_Background();
				GameManager.Instance.StateManager.SetWaitingBoss();
			}
			if (this.isPreviewCamera)
			{
				GameManager.Instance.StateManager.SetPreview();
				this.cinematics.Play();
				GameManager.Instance.bossManager.CreateBoss();
				GameManager.Instance.player.EMovement = BaseCharactor.EMovementBasic.Release;
				GameManager.Instance.hudManager.ShowSkipPreviewBoss(new UnityAction(this.StopPreviewCamera));
				this.cinematics.OnCinematicFinished.AddListener(delegate()
				{
					GameManager.Instance.hudManager.HideSkipPreviewBoss();
					GameManager.Instance.player.isAutoRun = true;
				});
			}
			else
			{
				GameManager.Instance.player.GunCurrent.WeaponCurrent.OnRelease();
				GameManager.Instance.player._PlayerSpine.ResetAnimTarget();
				GameManager.Instance.player.isAutoRun = true;
				if (this.hasBoss)
				{
					GameManager.Instance.hudManager.WarningBoss.SetActive(true);
				}
			}
		}));
		this.OnEnded = (Action)Delegate.Combine(this.OnEnded, new Action(delegate()
		{
			this.isStart = false;
			CameraController.Instance.isMapEnded = true;
			GameManager.Instance.player._PlayerInput.OnRemoveInput(false);
			GameManager.Instance.player._PlayerSpine.OnIdle(true);
			GameManager.Instance.player.isAutoRun = false;
			if (!this.hasBoss)
			{
				GameManager.Instance.player._PlayerSpine.OnVictory();
				PlayerManagerStory.Instance.OnRunGameOver();
			}
			else
			{
				GameManager.Instance.hudManager.WarningBoss.SetActive(false);
				GameManager.Instance.StateManager.SetGameRuning();
				GameManager.Instance.hudManager.ShowControl(1.1f);
				if (!this.isPreviewCamera)
				{
					GameManager.Instance.bossManager.CreateBoss();
				}
			}
		}));
	}

	public void EndMap()
	{
		if (GameManager.Instance.bossManager.Boss)
		{
			GameManager.Instance.bossManager.CreateBoss();
		}
		else
		{
			GameManager.Instance.hudManager.HideControl();
			PlayerManagerStory.Instance.OnRunGameOver();
		}
	}

	public void StopPreviewCamera()
	{
		this.cinematics.Stop();
		if (!object.ReferenceEquals(this.skipPreviewEvent, null))
		{
			this.skipPreviewEvent();
		}
	}

	private void Update()
	{
		if (!this.isinit || !this.isStart || !GameManager.Instance.player.isAutoRun)
		{
			return;
		}
		GameManager.Instance.player.OnMovement(BaseCharactor.EMovementBasic.Right);
	}

	private static PreGameOver instance;

	private bool isinit;

	[SerializeField]
	private bool hasBoss;

	[SerializeField]
	private bool isPreviewCamera;

	public ProCamera2DCinematics cinematics;

	public Action skipPreviewEvent;

	public Action OnStarted;

	public Action OnEnded;
}
