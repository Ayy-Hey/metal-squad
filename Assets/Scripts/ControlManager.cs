using System;
using System.Collections;
using PVPManager;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
	public static ControlManager Instance
	{
		get
		{
			if (ControlManager.instance == null)
			{
				ControlManager.instance = UnityEngine.Object.FindObjectOfType<ControlManager>();
			}
			return ControlManager.instance;
		}
	}

	private void LateUpdate()
	{
		if (GameManager.Instance.StateManager.EState != EGamePlay.RUNNING)
		{
			if (this._canvas.targetDisplay != 0)
			{
				this.Controls[ProfileManager.settingProfile.TypeControl].OnUpdatePosition();
			}
			return;
		}
		float deltaTime = Time.deltaTime;
		float num = deltaTime * 2f / 3.5f;
		float num2 = deltaTime / 2f;
		this.timeCooldownBomb -= num;
		this.timeCooldownHP -= num2;
		this.Controls[ProfileManager.settingProfile.TypeControl].grenade.imgFill.fillAmount = Mathf.Clamp01(this.timeCooldownBomb);
		this.Controls[ProfileManager.settingProfile.TypeControl].hpPack.imgFill.fillAmount = Mathf.Clamp01(this.timeCooldownHP);
		if (SplashScreen._isBuildMarketing)
		{
			this._canvas.targetDisplay = 1;
		}
	}

	public void OnShowControl(float time = 1.1f, bool isSkipCanvas = false)
	{
		this.OnHideControl();
		base.gameObject.SetActive(true);
		this._canvas.targetDisplay = 1;
		base.StartCoroutine(this.IEShowControl(time, isSkipCanvas));
		for (int i = 0; i < this.Controls.Length; i++)
		{
			this.Controls[i].gameObject.SetActive(ProfileManager.settingProfile.TypeControl == i);
		}
		this.Controls[ProfileManager.settingProfile.TypeControl].Show();
		this.Controls[ProfileManager.settingProfile.TypeControl].OnUpdatePositionWithCoroutines();
	}

	public void OnShowControlStartGame()
	{
		if (GameMode.Instance.Style == GameMode.GameStyle.MultiPlayer && GameMode.Instance.modePlay == GameMode.ModePlay.PvpMode && !PVPManager.PVPManager.Instance.isInit)
		{
			return;
		}
		if (GameMode.Instance.Style == GameMode.GameStyle.MultiPlayer && GameMode.Instance.modePlay == GameMode.ModePlay.CoOpMode && !CoOpManager.Instance.isInit)
		{
			return;
		}
		this.OnHideControl();
		base.gameObject.SetActive(true);
		this._canvas.targetDisplay = 1;
		for (int i = 0; i < this.Controls.Length; i++)
		{
			this.Controls[i].gameObject.SetActive(ProfileManager.settingProfile.TypeControl == i);
		}
		this.Controls[ProfileManager.settingProfile.TypeControl].Show();
		base.StartCoroutine(this.IEShowControlStartGame());
	}

	private IEnumerator IEShowControlStartGame()
	{
		yield return new WaitUntil(() => GameManager.Instance.StateManager.EState == EGamePlay.RUNNING);
		this._canvas.targetDisplay = 0;
		yield break;
	}

	private IEnumerator IEShowControl(float time, bool isSkipCanvas)
	{
		yield return new WaitForSeconds(time);
		if (!isSkipCanvas && this._canvas.targetDisplay != 0)
		{
			this._canvas.targetDisplay = 0;
		}
		yield break;
	}

	public void OnShowControlWithTutorial(bool isSkipCanvas = false)
	{
		base.gameObject.SetActive(true);
		this._canvas.targetDisplay = 1;
		base.StartCoroutine(this.IEShowControl(1.1f, isSkipCanvas));
		for (int i = 0; i < this.Controls.Length; i++)
		{
			this.Controls[i].gameObject.SetActive(ProfileManager.settingProfile.TypeControl == i);
		}
		this.Controls[ProfileManager.settingProfile.TypeControl].ShowWithTutorial();
	}

	public void OnHideControl()
	{
		base.StopAllCoroutines();
		base.gameObject.SetActive(false);
		this._canvas.targetDisplay = 1;
	}

	public void BombValueChange()
	{
		string text = string.Empty;
		if (ProfileManager.grenadesProfile[GameManager.Instance.player._PlayerData.IDGrenades].TotalBomb > 0)
		{
			text = Mathf.Min(ProfileManager.grenadesProfile[GameManager.Instance.player._PlayerData.IDGrenades].TotalBomb, 999).ToString();
		}
		this.Controls[ProfileManager.settingProfile.TypeControl].grenade.txtValue.text = text;
		this.Controls[ProfileManager.settingProfile.TypeControl].ChangeColorGrenadeButton();
	}

	public void ChangeImageWeaponIcon(bool isGunDefault)
	{
		this.Controls[ProfileManager.settingProfile.TypeControl].imgWeapon.sprite = ((!isGunDefault) ? this.spriteWeapon2[Mathf.Max(0, GameManager.Instance.player._PlayerData.IDGUN2)] : this.spriteWeapon1[Mathf.Max(0, GameManager.Instance.player._PlayerData.IDGUN1)]);
	}

	private static ControlManager instance;

	public OptionControl[] Controls;

	public float timeCooldownBomb = 1f;

	public float timeCooldownHP = 1f;

	public Sprite[] spriteWeapon1;

	public Sprite[] spriteWeapon2;

	[SerializeField]
	private Canvas _canvas;
}
