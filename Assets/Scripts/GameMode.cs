using System;
using UnityEngine;

public class GameMode
{
	public GameMode()
	{
		this.MODE = GameMode.Mode.NORMAL;
	}

	public static GameMode Instance
	{
		get
		{
			if (GameMode.instance == null)
			{
				GameMode.instance = new GameMode();
			}
			return GameMode.instance;
		}
	}

	public GameMode.Mode EMode
	{
		get
		{
			GameMode.GameStyle style = this.Style;
			if (style != GameMode.GameStyle.SinglPlayer)
			{
				return GameMode.Mode.SUPER_HARD;
			}
			GameMode.ModePlay modePlay = this.modePlay;
			if (modePlay == GameMode.ModePlay.Boss_Mode)
			{
				return this.modeBossMode;
			}
			if (modePlay != GameMode.ModePlay.Campaign)
			{
				return GameMode.Mode.NORMAL;
			}
			return this.MODE;
		}
	}

	public float GetMode()
	{
		int num = 0;
		try
		{
			num = (int)GameManager.Instance.Level;
		}
		catch
		{
		}
		switch (this.modePlay)
		{
		case GameMode.ModePlay.Campaign:
			switch (this.MODE)
			{
			case GameMode.Mode.NORMAL:
				return 1f;
			case GameMode.Mode.HARD:
				return 3f * Mathf.Max(1f, Mathf.Log10((float)(60 - num)));
			case GameMode.Mode.SUPER_HARD:
				return 9f * Mathf.Max(1f, Mathf.Log10((float)(60 - num)));
			default:
				return 1f;
			}
			break;
		case GameMode.ModePlay.Boss_Mode:
			switch (this.MODE)
			{
			case GameMode.Mode.NORMAL:
				return 2f;
			case GameMode.Mode.HARD:
				return 5f;
			case GameMode.Mode.SUPER_HARD:
				return 10f;
			default:
				return 1f;
			}
			break;
		case GameMode.ModePlay.CoOpMode:
			return 5f;
		}
		return 1f;
	}

	public void CheckUnlockDifficulty()
	{
		for (int i = 0; i < 5; i++)
		{
			if (ProfileManager._CampaignProfile.MapsProfile[(i + 1) * 12 - 1].GetStar(GameMode.Mode.NORMAL) > 0)
			{
				ProfileManager._CampaignProfile.MapsProfile[i * 12].SetUnlocked(GameMode.Mode.HARD, true);
			}
			if (ProfileManager._CampaignProfile.MapsProfile[(i + 1) * 12 - 1].GetStar(GameMode.Mode.HARD) > 0)
			{
				ProfileManager._CampaignProfile.MapsProfile[i * 12].SetUnlocked(GameMode.Mode.SUPER_HARD, true);
			}
		}
	}

	private static GameMode instance;

	public GameMode.ModePlay modePlay;

	public GameMode.Mode modeBossMode;

	public GameMode.Mode MODE;

	public GameMode.GameStyle Style;

	public enum GameStyle
	{
		SinglPlayer,
		MultiPlayer
	}

	public enum ModePlay
	{
		Campaign,
		Boss_Mode,
		Endless,
		Special_Campaign,
		PvpMode,
		CoOpMode
	}

	public enum Mode
	{
		NORMAL,
		HARD,
		SUPER_HARD,
		TUTORIAL
	}
}
