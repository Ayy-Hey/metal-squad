using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CustomController : MonoBehaviour
{
	public void Show(Action hide = null)
	{
		this.HideAction = hide;
		this.dataChanged = false;
		for (int i = 0; i < this.uiPositions.Length; i++)
		{
			this.uiPositions[i].OnTouch = new Action<UIPosition>(this.OnTouch);
		}
		this.sliderSize.onValueChanged.AddListener(new UnityAction<float>(this.ChangeSize));
		this.sliderAlpha.onValueChanged.AddListener(new UnityAction<float>(this.ChangeAlpha));
		this.ActiveChangeTool(false);
	}

	private void OnTouch(UIPosition obj)
	{
		this.dataChanged = true;
		if (!object.ReferenceEquals(obj, this.lastTouch))
		{
			if (this.lastTouch)
			{
				this.lastTouch.imgHightlight.enabled = false;
			}
			this.lastTouch = obj;
			this.ActiveChangeTool(true);
		}
	}

	private void ActiveChangeTool(bool enable)
	{
		this.sliderAlpha.enabled = enable;
		this.sliderSize.enabled = enable;
		this.up.enabled = enable;
		this.down.enabled = enable;
		this.left.enabled = enable;
		this.right.enabled = enable;
		if (enable && this.lastTouch)
		{
			this.sliderAlpha.value = this.lastTouch.color.a;
			this.sliderSize.value = this.lastTouch.scale.x;
		}
	}

	public void ChangePosition(int type)
	{
		if (this.lastTouch)
		{
			this.posOption = this.lastTouch.rect.anchoredPosition;
			switch (type)
			{
			case 0:
				this.posOption.y = this.posOption.y + 2f;
				break;
			case 1:
				this.posOption.y = this.posOption.y - 2f;
				break;
			case 2:
				this.posOption.x = this.posOption.x - 2f;
				break;
			case 3:
				this.posOption.x = this.posOption.x + 2f;
				break;
			}
			this.lastTouch.SetPosition(this.posOption);
		}
	}

	private void ChangeSize(float size)
	{
		if (this.lastTouch)
		{
			this.lastTouch.SetSize(size);
		}
	}

	private void ChangeAlpha(float alpha)
	{
		if (this.lastTouch)
		{
			this.lastTouch.SetAlpha(alpha);
		}
	}

	public void Default()
	{
		for (int i = 0; i < this.uiPositions.Length; i++)
		{
			this.uiPositions[i].Default();
		}
		if (this.lastTouch)
		{
			this.lastTouch.imgHightlight.enabled = false;
		}
		this.ActiveChangeTool(false);
	}

	public void Save()
	{
		for (int i = 0; i < this.uiPositions.Length; i++)
		{
			this.uiPositions[i].Save();
		}
		this.dataChanged = false;
	}

	public void Exit()
	{
		if (this.dataChanged)
		{
			PopupManager.Instance.ShowDialog(new Action<bool>(this.DialogExitCallBack), 1, PopupManager.Instance.GetText(Localization0.Do_you_want_save_your_changed, null), string.Empty);
		}
		else
		{
			this.Back();
		}
	}

	private void DialogExitCallBack(bool ok)
	{
		if (ok)
		{
			this.Save();
			this.Back();
		}
		else
		{
			this.Back();
		}
	}

	private void Back()
	{
		if (this.lastTouch)
		{
			this.lastTouch.imgHightlight.enabled = false;
			this.lastTouch = null;
		}
		base.gameObject.SetActive(false);
		if (!object.ReferenceEquals(this.HideAction, null))
		{
			this.HideAction();
		}
	}

	public UIPosition[] uiPositions;

	public Slider sliderSize;

	public Slider sliderAlpha;

	public Button up;

	public Button down;

	public Button left;

	public Button right;

	private Action HideAction;

	private UIPosition lastTouch;

	private bool dataChanged;

	private Vector2 posOption;
}
