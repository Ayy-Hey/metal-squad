using System;
using ABI;
using UnityEngine;

public class PopupController : MonoBehaviour
{
	public void Show()
	{
		base.gameObject.SetActive(true);
		this.popupSetting.gameObject.SetActive(true);
		this.popupSetting.anchoredPosition = new Vector2(0f, 600f);
		LeanTween.moveY(this.popupSetting, 0f, 0.5f).setEase(LeanTweenType.easeInExpo).setOnComplete(delegate()
		{
		});
	}

	public void OnLogoutFB()
	{
		PopupManager.Instance.ShowDialog(delegate(bool callback)
		{
			if (callback)
			{
				MonoSingleton<FBAndPlayfabMgr>.Instance.LogOutFacebook();
				this.objLogOut.SetActive(false);
			}
		}, 1, PopupManager.Instance.GetText(Localization0.Do_you_want_to_logout_facebook, null), PopupManager.Instance.GetText(Localization0.Logout, null));
	}

	public void Hide()
	{
		if (this.settingControl.gameObject.activeSelf)
		{
			this.settingControl.Hide();
			return;
		}
		this.popupSetting.anchoredPosition = new Vector2(0f, 600f);
		this.popupSetting.gameObject.SetActive(false);
		base.gameObject.SetActive(false);
	}

	public RectTransform popupSetting;

	public ControllerSetting settingControl;

	[SerializeField]
	private GameObject objLogOut;
}
