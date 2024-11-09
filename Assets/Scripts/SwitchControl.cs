using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SwitchControl : MonoBehaviour
{
	private void OnEnable()
	{
		this._controlId = ProfileManager.settingProfile.TypeControl;
		this._nodeSize = this.rectControls[0].sizeDelta.x;
		base.StartCoroutine(this.MoveRectTranform(this._controlId));
	}

	public void Ok()
	{
		ProfileManager.settingProfile.TypeControl = this._controlId;
		if (this.popupSetting != null)
		{
			this.popupSetting.SetControl();
		}
	}

	public void ChangeControl(bool next)
	{
		base.StopAllCoroutines();
		this._controlId += ((!next) ? -1 : 1);
		this._controlId = ((this._controlId < this.rectControls.Length) ? this._controlId : 0);
		this._controlId = ((this._controlId >= 0) ? this._controlId : (this.rectControls.Length - 1));
		this.Ok();
		base.StartCoroutine(this.MoveRectTranform(this._controlId));
	}

	private IEnumerator MoveRectTranform(int toNote)
	{
		float velo = 0f;
		float controlSize = 0f;
		float toX = -this.rectControls[toNote].anchoredPosition.x;
		this._anchored = this.rect.anchoredPosition;
		this.rectControls[toNote].SetAsLastSibling();
		for (int i = 0; i < this.rectControls.Length; i++)
		{
			this.rectControls[i].GetComponent<Image>().color = ((i != toNote) ? Color.gray : Color.white);
		}
		this.ReplateControl();
		while (this._anchored.x != toX)
		{
			this._anchored.x = Mathf.SmoothDamp(this._anchored.x, toX, ref velo, 0.3f);
			if (Mathf.Abs(this._anchored.x - toX) <= 1f)
			{
				this._anchored.x = toX;
			}
			this.rect.anchoredPosition = this._anchored;
			for (int j = 0; j < this.rectControls.Length; j++)
			{
				controlSize = this.topSize - Mathf.Min(this.deltaSize * Mathf.Abs(this.rectControls[j].anchoredPosition.x + this._anchored.x) / this.distanceChangeSize, this.deltaSize);
				this.rectControls[j].localScale = Vector3.one * controlSize;
			}
			yield return 0;
		}
		for (int k = 0; k < this.rectControls.Length; k++)
		{
			this.rectControls[k].localScale = ((k != toNote) ? (this.topSize - this.deltaSize) : this.topSize) * Vector3.one;
		}
		yield break;
	}

	private void ReplateControl()
	{
		int num = this._controlId + 1;
		num = ((num < this.rectControls.Length) ? num : 0);
		int num2 = this._controlId - 1;
		num2 = ((num2 >= 0) ? num2 : (this.rectControls.Length - 1));
		Vector2 anchoredPosition = this.rectControls[num].anchoredPosition;
		Vector2 anchoredPosition2 = this.rectControls[num2].anchoredPosition;
		Vector2 anchoredPosition3 = this.rectControls[this._controlId].anchoredPosition;
		anchoredPosition.x = anchoredPosition3.x + this._nodeSize;
		anchoredPosition2.x = anchoredPosition3.x - this._nodeSize;
		this.rectControls[num].anchoredPosition = anchoredPosition;
		this.rectControls[num2].anchoredPosition = anchoredPosition2;
	}

	public void OnCustom(int idControl)
	{
		PopupManager.Instance.ShowPopupCustomControl(idControl, null);
	}

	public RectTransform rect;

	public RectTransform[] rectControls;

	public float topSize;

	public float deltaSize;

	public float distanceChangeSize;

	private Vector2 _anchored;

	private int _controlId;

	private float _nodeSize;

	public PopupSetting popupSetting;
}
