using System;
using System.Collections;
using Com.LuisPedroFonseca.ProCamera2D;
using PlayerStory;
using PVPManager;
using Spine;
using Spine.Unity;
using UnityEngine;

public class PlayerManagerStory : MonoBehaviour
{
	public static PlayerManagerStory Instance
	{
		get
		{
			if (PlayerManagerStory.instance == null)
			{
				PlayerManagerStory.instance = UnityEngine.Object.FindObjectOfType<PlayerManagerStory>();
			}
			return PlayerManagerStory.instance;
		}
	}

	private IEnumerator Start()
	{
		yield return new WaitUntil(() => GameManager.Instance.StateManager.EState == EGamePlay.BEGIN);
		yield return new WaitUntil(() => GameManager.Instance.player != null);
		yield return new WaitUntil(() => GameManager.Instance.player.IsInit);
		this.OnStart();
		yield break;
	}

	public void OnStart()
	{
		this.isPreGameOver = false;
		PlayerManagerStory.TypeBegin typeBegin = this.typeBegin;
		if (typeBegin != PlayerManagerStory.TypeBegin.RUN)
		{
			if (typeBegin == PlayerManagerStory.TypeBegin.JUMP)
			{
				this.StartAirPlain();
			}
		}
		else
		{
			this.OnRunBegin();
		}
	}

	public void StartAirPlain()
	{
		if (GameMode.Instance.Style != GameMode.GameStyle.MultiPlayer)
		{
			ProCamera2D.Instance.Zoom(1.8f, 0f, EaseType.EaseInOut);
		}
		this.objJump.SetActive(true);
		this.skeletonAirPlane.gameObject.SetActive(true);
		this.skeletonAirPlane.state.SetAnimation(0, "animation", false);
		this.skeletonAirPlane.state.Complete -= this.HandleComplete;
		this.skeletonAirPlane.state.Complete += this.HandleComplete;
		this.skeletonAirPlane.state.Event -= this.HandleEvent;
		this.skeletonAirPlane.state.Event += this.HandleEvent;
		if (ProfileManager.settingProfile.IsSound)
		{
			this._audioAirplane.Play();
			this._audioAirplane.loop = true;
		}
	}

	public void SetPosEffectAir(Vector3 pos)
	{
		this.skeletonEffectAirPlane.transform.position = pos;
	}

	private void HandleComplete(TrackEntry entry)
	{
		if (entry == null)
		{
			return;
		}
		this.skeletonAirPlane.gameObject.SetActive(false);
		this.skeletonAirPlane.state.ClearTracks();
		if (this.OnAirPlainStartGameEnded != null)
		{
			this.OnAirPlainStartGameEnded();
		}
	}

	private void HandleEvent(TrackEntry entry, Spine.Event e)
	{
		if (entry == null)
		{
			return;
		}
		string name = e.Data.Name;
		if (name != null)
		{
			if (!(name == "bui"))
			{
				if (name == "nhay")
				{
					Bone bone = this.skeletonAirPlane.skeleton.FindBone("NV");
					Vector3 position = this.skeletonAirPlane.transform.position;
					position.x += bone.WorldX;
					position.y += bone.WorldY;
					this.OnJumpBegin(position);
				}
			}
			else
			{
				this.skeletonEffectAirPlane.gameObject.SetActive(true);
				this.skeletonEffectAirPlane.state.SetAnimation(0, "nhay", false);
			}
		}
	}

	public void OnRunGameOver()
	{
		this.isPreGameOver = true;
		EnemyManager.Instance.IsInit = false;
		bool flag = true;
		bool flag2 = false;
		if (GameMode.Instance.Style != GameMode.GameStyle.MultiPlayer)
		{
			if (GameMode.Instance.modePlay == GameMode.ModePlay.Campaign)
			{
				string key = "com.sora.metal.squad.first.finish.map." + GameManager.Instance.Level;
				if ((int)GameManager.Instance.Level % (int)ELevel.LEVEL_7 == (int)ELevel.LEVEL_6 && PlayerPrefs.GetInt(key, 0) == 0)
				{
					flag = false;
					PlayerPrefs.SetInt(key, 1);
				}
			}
			if (GameMode.Instance.EMode == GameMode.Mode.TUTORIAL)
			{
				flag = false;
			}
			if (this.typeGameEnd == PlayerManagerStory.TypeGameEnd.None || flag)
			{
				this.isPreGameOver = false;
				EventDispatcher.PostEvent("CompletedGame");
				return;
			}
		}
		else
		{
			if (GameMode.Instance.modePlay == GameMode.ModePlay.CoOpMode)
			{
				return;
			}
			if (GameMode.Instance.modePlay == GameMode.ModePlay.PvpMode)
			{
				flag2 = PVPManager.PVPManager.Instance.OnCompleteMap();
			}
		}
		if (flag2)
		{
			return;
		}
		this.playerGameOver.OnBegin();
		PlayerSetupGameOver playerSetupGameOver = this.playerGameOver;
		playerSetupGameOver.OnEnded = (Action)Delegate.Combine(playerSetupGameOver.OnEnded, new Action(delegate()
		{
			this.isPreGameOver = false;
			if (GameMode.Instance.Style != GameMode.GameStyle.MultiPlayer)
			{
				EventDispatcher.PostEvent("CompletedGame");
			}
		}));
	}

	public void OnRunBegin()
	{
		if (this.typeBegin == PlayerManagerStory.TypeBegin.JUMP)
		{
			return;
		}
		this.playerRun.OnRun(delegate
		{
			GameMode.Mode emode = GameMode.Instance.EMode;
			if (emode != GameMode.Mode.TUTORIAL)
			{
				if (!object.ReferenceEquals(EnemyManager.Instance.enemy_helicopter, null))
				{
					EnemyManager.Instance.enemy_helicopter.gameObject.SetActive(true);
					EnemyManager.Instance.enemy_helicopter.Init();
				}
				GameManager.Instance.skillManager.Init();
				GameManager.Instance.StateManager.SetGameRuning();
				CameraController.Instance.AddPlayer(GameManager.Instance.player.transform, 1.5f);
			}
		});
		GameManager.Instance.player._PlayerSpine.OnIdle(true);
		SingletonGame<AudioController>.Instance.PlaySound(GameManager.Instance.player._audioGoGo, 1f);
	}

	public void OnJumpBegin(Vector2 posJump)
	{
		if (this.typeBegin == PlayerManagerStory.TypeBegin.RUN)
		{
			return;
		}
		if (GameMode.Instance.Style == GameMode.GameStyle.MultiPlayer)
		{
			GameManager.Instance.player.gameObject.SetActive(true);
			GameManager.Instance.player._PlayerInput.OnRemoveInput(true);
		}
		GameManager.Instance.player._PlayerSpine.SetParachute(posJump);
		CameraController.Instance.AddPlayer(GameManager.Instance.player.transform, 1.5f);
	}

	private static PlayerManagerStory instance;

	public PlayerManagerStory.TypeBegin typeBegin;

	public PlayerManagerStory.TypeGameEnd typeGameEnd;

	public GameObject objJump;

	public PlayerSetupGameBegin playerRun;

	public PlayerSetupGameOver playerGameOver;

	public Vector2 posPlayerStart;

	public GameObject objSkip;

	public bool isPreGameOver;

	public SkeletonAnimation skeletonAirPlane;

	public SkeletonAnimation skeletonEffectAirPlane;

	public AudioSource _audioAirplane;

	public Action OnAirPlainStartGameEnded;

	public enum TypeBegin
	{
		RUN,
		JUMP
	}

	public enum TypeGameEnd
	{
		None,
		AirPlane
	}
}
