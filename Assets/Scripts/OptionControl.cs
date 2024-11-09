using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OptionControl : MonoBehaviour
{
	public virtual void Show()
	{
		base.gameObject.SetActive(true);
		this.ChangeColorGrenadeButton();
		if (GameManager.Instance.hudManager.Max_Hp_Booster > 0)
		{
			this.hpPack.txtValue.text = GameManager.Instance.hudManager.Max_Hp_Booster.ToString();
			this.hpPack.objValue.SetActive(true);
			Color baseColor = this.UButtons[3].baseColor;
			baseColor.a = 1f;
			this.UButtons[3].UpdateBaseColor(baseColor);
		}
		else
		{
			this.hpPack.objValue.SetActive(false);
			Color baseColor2 = this.UButtons[3].baseColor;
			baseColor2.a = 0.5f;
			this.UButtons[3].UpdateBaseColor(baseColor2);
		}
		try
		{
			this.UButtons[1].gameObject.SetActive(false);
		}
		catch (Exception ex)
		{
		}
	}

	public void OnUpdatePositionWithCoroutines()
	{
		base.StopAllCoroutines();
		base.StartCoroutine(this.ResetPosition());
	}

	public void OnUpdatePosition()
	{
		for (int i = 0; i < this.joysticks.Length; i++)
		{
			this.joysticks[i].UpdatePositioning();
		}
		for (int j = 0; j < this.UButtons.Length; j++)
		{
			this.UButtons[j].UpdatePositioning();
		}
	}

	public virtual void ShowWithTutorial()
	{
		base.gameObject.SetActive(true);
	}

	private IEnumerator ResetPosition()
	{
		for (int i = 0; i < this.joysticks.Length; i++)
		{
			this.joysticks[i].UpdatePositioning();
		}
		for (int j = 0; j < this.UButtons.Length; j++)
		{
			this.UButtons[j].UpdatePositioning();
		}
		yield return new WaitForSeconds(0.5f);
		for (int k = 0; k < this.joysticks.Length; k++)
		{
			this.joysticks[k].UpdatePositioning();
		}
		for (int l = 0; l < this.UButtons.Length; l++)
		{
			this.UButtons[l].UpdatePositioning();
		}
		yield return new WaitForSeconds(0.5f);
		for (int m = 0; m < this.joysticks.Length; m++)
		{
			this.joysticks[m].UpdatePositioning();
		}
		for (int n = 0; n < this.UButtons.Length; n++)
		{
			this.UButtons[n].UpdatePositioning();
		}
		yield break;
	}

	public void TouchBomb(bool down)
	{
		if (GameManager.Instance.player.EMovement == BaseCharactor.EMovementBasic.DIE)
		{
			return;
		}
		if (down && this.grenade.imgFill.fillAmount <= 0f)
		{
			ControlManager.Instance.timeCooldownBomb = 1f;
			GameManager.Instance.player._PlayerInput.OnThrowGrenades(true);
		}
		else
		{
			GameManager.Instance.player._PlayerInput.OnThrowGrenades(false);
		}
	}

	public void TouchHP(bool down)
	{
		if (GameManager.Instance.player.EMovement == BaseCharactor.EMovementBasic.DIE)
		{
			return;
		}
		if (down && this.hpPack.imgFill.fillAmount <= 0f && GameManager.Instance.hudManager.Max_Hp_Booster > 0)
		{
			if (GameManager.Instance.player.HPCurrent >= GameManager.Instance.player._PlayerData.ramboProfile.HP)
			{
				return;
			}
			GameManager.Instance.player.AddHealthPoint(GameManager.Instance.player._PlayerData.HPMax(), EWeapon.NONE);
			PopupManager.Instance.SaveReward(Item.Medkit, -1, "Use", null);
			DailyQuestManager.Instance.MisionBooster(EBooster.MEDKIT, delegate(bool isCompleted, DailyQuestManager.InforQuest infor)
			{
				if (isCompleted)
				{
					UIShowInforManager.Instance.OnShowDailyQuest(infor.Desc, infor.item, infor.amount);
				}
			});
			GameManager.Instance.hudManager.Max_Hp_Booster--;
			this.hpPack.txtValue.text = GameManager.Instance.hudManager.Max_Hp_Booster.ToString();
			ControlManager.Instance.timeCooldownHP = 1f;
			GameManager.Instance.numberHpPack++;
		}
		if (GameManager.Instance.hudManager.Max_Hp_Booster > 0)
		{
			this.hpPack.txtValue.text = GameManager.Instance.hudManager.Max_Hp_Booster.ToString();
			this.hpPack.objValue.SetActive(true);
			Color baseColor = this.UButtons[3].baseColor;
			baseColor.a = 1f;
			this.UButtons[3].UpdateBaseColor(baseColor);
		}
		else
		{
			this.hpPack.objValue.SetActive(false);
			Color baseColor2 = this.UButtons[3].baseColor;
			baseColor2.a = 0.5f;
			this.UButtons[3].UpdateBaseColor(baseColor2);
		}
	}

	public void TouchJump(bool down)
	{
		if (GameManager.Instance.player.EMovement == BaseCharactor.EMovementBasic.DIE)
		{
			return;
		}
		GameManager.Instance.player._PlayerInput.OnJump(down);
	}

	public void TouchSwitchGun()
	{
		if (GameManager.Instance.player.EMovement == BaseCharactor.EMovementBasic.DIE)
		{
			return;
		}
		GameManager.Instance.audioManager.Click();
		bool isGunDefault = GameManager.Instance.player.isGunDefault;
		if (!GameManager.Instance.player.IsRemotePlayer && GameManager.Instance.player.syncRamboState != null)
		{
			GameManager.Instance.player.syncRamboState.SendRpc_SwitchGun(!isGunDefault, 0);
		}
		GameManager.Instance.player._PlayerInput.SwitchGun(!isGunDefault);
	}

	public void ChangeColorSwitchGun(float alphaColor)
	{
		Color color = this.imgWeapon.color;
		color.a = alphaColor;
		this.imgWeapon.color = color;
		color = this.UButtons[1].baseColor;
		color.a = alphaColor;
		this.UButtons[1].UpdateBaseColor(color);
	}

	public void ChangeColorGrenadeButton()
	{
		if (GameMode.Instance.EMode == GameMode.Mode.TUTORIAL)
		{
			return;
		}
		if (ProfileManager.grenadesProfile[ProfileManager.grenadeCurrentId.Data.Value].TotalBomb > 0)
		{
			this.SetColorGrenade(1f);
		}
		else
		{
			this.SetColorGrenade(0.2f);
		}
	}

	private void SetColorGrenade(float alphaValue)
	{
		Color baseColor = this.UButtons[0].baseColor;
		baseColor.a = alphaValue;
		this.UButtons[0].UpdateBaseColor(baseColor);
		this.grenade.objValue.SetActive(alphaValue >= 0.9f);
	}

	public OptionControl.Grenade grenade;

	public OptionControl.HPPack hpPack;

	public Image imgWeapon;

	public UltimateButton[] UButtons;

	public UltimateJoystick[] joysticks;

	[Serializable]
	public struct Grenade
	{
		public Text txtValue;

		public Image imgFill;

		public GameObject obj;

		public GameObject objValue;
	}

	[Serializable]
	public struct HPPack
	{
		public Text txtValue;

		public Image imgFill;

		public GameObject obj;

		public GameObject objValue;
	}
}
