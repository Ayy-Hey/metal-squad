using System;
using UnityEngine;
using UnityEngine.UI;

public class CardTutorial : CardBase
{
	private void OnEnable()
	{
		for (int i = 0; i < this.textLocalization.Length; i++)
		{
			if (this.textLocalization[i].txt_Text != null)
			{
				if (this.textLocalization[i].isUpcaseText)
				{
					this.textLocalization[i].txt_Text.text = (this.textLocalization[i].textStart + PopupManager.Instance.GetText(this.textLocalization[i].key, null) + this.textLocalization[i].textEnd).ToUpper();
				}
				else
				{
					this.textLocalization[i].txt_Text.text = this.textLocalization[i].textStart + PopupManager.Instance.GetText(this.textLocalization[i].key, null) + this.textLocalization[i].textEnd;
				}
			}
		}
	}

	private void Update()
	{
		if (this.obj_Target != null && this.transform_TipInfor != null)
		{
			this.transform_TipInfor.position = this.obj_Target.transform.position;
		}
	}

	[Header("---------------CardTutorial---------------")]
	public TextLocalization[] textLocalization;

	public bool isRaycaster;

	public GameObject obj_Target;

	public Text txt_Infor;

	public Transform transform_TipInfor;

	public RectTransform rect_BtnSkip;

	public FormUI IDForm;

	public int IDObj;
}
