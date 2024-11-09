using System;
using UnityEngine;

public class ControllerSetting : MonoBehaviour
{
	private void OnEnable()
	{
		if ((float)Screen.width < (float)Screen.height * 1.7f)
		{
			this.tableObj.localScale = new Vector3(0.75f, 0.75f, 0.75f);
		}
		else
		{
			this.tableObj.localScale = Vector3.one;
		}
	}

	public void Show(Action onBack = null)
	{
		this.OnBack = onBack;
		base.gameObject.SetActive(true);
	}

	public void OnShow()
	{
		base.gameObject.SetActive(true);
	}

	public void Back()
	{
		base.StopAllCoroutines();
		base.gameObject.SetActive(false);
		if (!object.ReferenceEquals(this.OnBack, null))
		{
			this.OnBack();
		}
	}

	public void Hide()
	{
		AudioClick.Instance.OnClick();
		base.gameObject.SetActive(false);
		if (!object.ReferenceEquals(this.OnBack, null))
		{
			this.OnBack();
		}
	}

	public void Hide2(Vector2 pos)
	{
		AudioClick.Instance.OnClick();
		LeanTween.scale(base.gameObject, Vector3.zero, 1f);
		LeanTween.move(base.gameObject, pos, 1f).setOnComplete(delegate()
		{
			base.gameObject.SetActive(false);
		});
		if (!object.ReferenceEquals(this.OnBack, null))
		{
			this.OnBack();
		}
	}

	public void SetControl(int IdControl)
	{
		AudioClick.Instance.OnClick();
		this.IdControl = IdControl;
		ProfileManager.settingProfile.TypeControl = IdControl;
		for (int i = 0; i < this.imgTick.Length; i++)
		{
			this.imgTick[i].SetActive(IdControl == i);
		}
		string parameterValue = "GamePlay";
		try
		{
			parameterValue = MenuManager.Instance.GetNameForm(MenuManager.Instance.formCurrent.idForm);
		}
		catch
		{
		}
		
	}

	public void OnCustom(int idControl)
	{
		this.tableObj.gameObject.SetActive(false);
		PopupManager.Instance.ShowPopupCustomControl(idControl, delegate
		{
			this.tableObj.gameObject.SetActive(true);
		});
	}

	private Action OnBack;

	[SerializeField]
	private GameObject[] imgTick;

	private int IdControl;

	[SerializeField]
	private RectTransform tableObj;
}
