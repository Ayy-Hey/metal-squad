using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VipBar : MonoBehaviour
{
	private void OnAwake()
	{
	}

	public void Show()
	{
		this._vipLevel = (int)ProfileManager.inAppProfile.vipProfile.level;
		this._point = (float)ProfileManager.inAppProfile.vipProfile.Point;
		this._maxVip = DataLoader.vipData.Levels.Length - 1;
		this.imgCurrentVip.sprite = this.spriteVip[Mathf.Min(this._vipLevel + 1, this._maxVip)];
		this.imgNextVip.sprite = this.spriteVip[Mathf.Min(this._vipLevel + 2, this._maxVip + 1)];
		this.txtValue.text = this._point - (float)((this._vipLevel >= 0) ? DataLoader.vipData.Levels[this._vipLevel].point : 0) + "/" + (DataLoader.vipData.Levels[Mathf.Min(this._vipLevel + 1, this._maxVip)].point - ((this._vipLevel >= 0) ? DataLoader.vipData.Levels[this._vipLevel].point : 0));
		this.imgProgress.fillAmount = (this._point - (float)((this._vipLevel >= 0) ? DataLoader.vipData.Levels[this._vipLevel].point : 0)) / (float)(DataLoader.vipData.Levels[Mathf.Min(this._vipLevel + 1, this._maxVip)].point - ((this._vipLevel >= 0) ? DataLoader.vipData.Levels[this._vipLevel].point : 0));
		this.CheckHasGift();
	}

	public void ShowVipPopup()
	{
		this.inappPopup.gameObject.SetActive(false);
		PopupManager.Instance.ShowVipPopup(delegate(bool closed)
		{
			this.CheckHasGift();
			this.inappPopup.ShowCoin();
			this.inappPopup.gameObject.SetActive(true);
		}, true);
	}

	public void CheckVipLevel()
	{
		this._oldVipLevel = this._vipLevel;
		this._vipLevel = (int)ProfileManager.inAppProfile.vipProfile.level;
		this.Proccessing();
	}

	public void CheckHasGift()
	{
		this.imgThongBaoBenefits.enabled = ProfileManager.inAppProfile.vipProfile.HasAnyGift();
	}

	private void Proccessing()
	{
		this.imgCurrentVip.sprite = this.spriteVip[Mathf.Min(this._oldVipLevel + 1, this._maxVip)];
		this.imgNextVip.sprite = this.spriteVip[Mathf.Min(this._oldVipLevel + 2, this._maxVip + 1)];
		if (this._oldVipLevel < this._vipLevel)
		{
			this._oldVipLevel++;
			this._targetPoint = (float)DataLoader.vipData.Levels[this._oldVipLevel].point;
			this._oldPoint = this._point;
		}
		else if (this._targetPoint < (float)ProfileManager.inAppProfile.vipProfile.Point)
		{
			this._targetPoint = (float)ProfileManager.inAppProfile.vipProfile.Point;
			this._oldPoint = (float)DataLoader.vipData.Levels[this._oldVipLevel].point;
		}
		this.Show();
	}

	private IEnumerator ApplyPoint()
	{
		while (this._point < this._targetPoint)
		{
			this._point = Mathf.MoveTowards(this._point, this._targetPoint, this._deltaPoint);
			if (this._targetPoint == (float)DataLoader.vipData.Levels[this._oldVipLevel].point)
			{
				this.imgProgress.fillAmount = (this._point - this._oldPoint) / (this._targetPoint - this._oldPoint);
				this.txtValue.text = this._point + "/" + this._targetPoint;
			}
			else
			{
				this.imgProgress.fillAmount = (this._point - this._oldPoint) / (this._newPoint - this._oldPoint);
				this.txtValue.text = this._point + "/" + this._newPoint;
			}
			yield return 0;
		}
		this.Proccessing();
		yield break;
	}

	[SerializeField]
	private InappPopup inappPopup;

	[Header("Sprites Vip:")]
	[SerializeField]
	private Sprite[] spriteVip;

	[SerializeField]
	[Header("Vip UI:")]
	private Image imgCurrentVip;

	[SerializeField]
	private Image imgNextVip;

	[SerializeField]
	private Image imgProgress;

	[SerializeField]
	private Text txtValue;

	[SerializeField]
	private Image imgThongBaoBenefits;

	private int _vipLevel;

	private int _oldVipLevel;

	private int _newVipLevel;

	private float _point;

	private float _oldPoint;

	private float _newPoint;

	private float _targetPoint;

	private float _deltaPoint;

	private int _maxVip;
}
