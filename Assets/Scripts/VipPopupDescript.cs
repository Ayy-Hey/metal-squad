using System;
using UnityEngine;
using UnityEngine.UI;

public class VipPopupDescript : MonoBehaviour
{
	public void Show(VipLevel vipLevel)
	{
		for (int i = 0; i < this.obj.Length; i++)
		{
			this.obj[i].SetActive(i < vipLevel.idDes.Length);
			string[] nameSpec = vipLevel.valueDes[i];
			this.texts[i].text = PopupManager.Instance.GetText((Localization0)vipLevel.idDes[i], nameSpec);
			if (i == vipLevel.idDes.Length - 1)
			{
				this.texts[i].color = this.color_yellow;
			}
			else
			{
				this.texts[i].color = this.color_normal;
			}
		}
	}

	public GameObject[] obj;

	public Text[] texts;

	public Color color_normal;

	public Color color_yellow;
}
