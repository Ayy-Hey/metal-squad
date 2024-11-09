using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
	public void Init()
	{
		if (GameMode.Instance.MODE == GameMode.Mode.TUTORIAL)
		{
			this.IDSKILL = 2;
		}
		else
		{
			this.IDSKILL = ProfileManager.settingProfile.IDChar;
		}
		float cooldown = DataLoader.characterData[this.IDSKILL].skills[0].Cooldown[ProfileManager.rambos[this.IDSKILL].LevelUpgrade];
		this.buttonSkill[this.IDSKILL].OnInit(cooldown);
		this.isInit = true;
		this.ListRainBombSkill = new List<RainBombSkill>();
		this.PoolRainBombSkill = new ObjectPooling<RainBombSkill>(0, null, null);
	}

	public void UpdateSkill(float deltaTime)
	{
		if (this.buttonSkill[this.IDSKILL] != null)
		{
			this.buttonSkill[this.IDSKILL].OnUpdate(deltaTime);
		}
		for (int i = 0; i < this.ListRainBombSkill.Count; i++)
		{
			if (this.ListRainBombSkill[i] != null && this.ListRainBombSkill[i].gameObject.activeSelf)
			{
				this.ListRainBombSkill[i].UpdateObject();
			}
		}
	}

	public void OnClickedSkill()
	{
		if (!this.isInit)
		{
			return;
		}
		if (SplashScreen._isBuildMarketing)
		{
			this.ShowSkill(GameManager.Instance.player, this.IDSKILL);
			return;
		}
		if (this.buttonSkill[this.IDSKILL].IsActive)
		{
			GameManager.Instance.mMission.CountUseSkills++;
			if (GameMode.Instance.MODE != GameMode.Mode.TUTORIAL)
			{
				GameManager.Instance.mMission.StartCheck();
			}
			this.ShowSkill(GameManager.Instance.player, this.IDSKILL);
			this.buttonSkill[this.IDSKILL].Reset();
		}
	}

	public void ShowSkill(PlayerMain player, int IDSKILL)
	{
		if (IDSKILL != 0)
		{
			if (IDSKILL != 2)
			{
				if (IDSKILL == 1)
				{
					float duration = DataLoader.characterData[IDSKILL].skills[0].ActiveDuration[ProfileManager.rambos[IDSKILL].LevelUpgrade];
					if (!player.IsRemotePlayer && player.syncRamboState != null)
					{
						player.syncRamboState.SendRpc_OnInvisible();
					}
					player._PlayerSkill.OnInvisible(duration);
				}
			}
			else
			{
				player._PlayerSkill.OnSkillActiveRocket();
			}
		}
		else
		{
			if (!player.IsRemotePlayer && player.syncRamboState != null)
			{
				player.syncRamboState.SendRpc_CallEyeBotSupport();
			}
			this.callEyeBotSupport.gameObject.SetActive(true);
			this.callEyeBotSupport.CallSupport(player);
		}
	}

	public void DestroyAll()
	{
		for (int i = 0; i < this.ListRainBombSkill.Count; i++)
		{
			if (this.ListRainBombSkill[i] != null && this.ListRainBombSkill[i].gameObject.activeSelf)
			{
				this.ListRainBombSkill[i].gameObject.SetActive(false);
			}
		}
	}

	public void OnPause()
	{
		for (int i = 0; i < this.ListRainBombSkill.Count; i++)
		{
			if (this.ListRainBombSkill[i] != null && this.ListRainBombSkill[i].gameObject.activeSelf)
			{
				this.ListRainBombSkill[i].SetPause();
			}
		}
	}

	public void OnResume()
	{
		for (int i = 0; i < this.ListRainBombSkill.Count; i++)
		{
			if (this.ListRainBombSkill[i] != null && this.ListRainBombSkill[i].gameObject.activeSelf)
			{
				this.ListRainBombSkill[i].SetResume();
			}
		}
	}

	public void CreateRainBomb(Vector2 pos, bool hasDamage = true)
	{
		try
		{
			RainBombSkill rainBombSkill = this.PoolRainBombSkill.New();
			if (rainBombSkill == null)
			{
				rainBombSkill = UnityEngine.Object.Instantiate<RainBombSkill>(this.rainBombSkill, this.parentRainBombSkill);
				this.ListRainBombSkill.Add(rainBombSkill);
			}
			rainBombSkill.gameObject.SetActive(true);
			rainBombSkill.gameObject.transform.position = pos;
			rainBombSkill.Init(pos, hasDamage);
		}
		catch (Exception ex)
		{
		}
	}

	public CallEyeBotSupport callEyeBotSupport;

	public ButtonSkill[] buttonSkill;

	private bool isInit;

	[Header("RainBombSkill")]
	public Transform parentRainBombSkill;

	public RainBombSkill rainBombSkill;

	private List<RainBombSkill> ListRainBombSkill;

	public ObjectPooling<RainBombSkill> PoolRainBombSkill;

	private int IDSKILL;
}
