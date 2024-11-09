using System;
using UnityEngine;
using UnityEngine.UI;

public class InforOptionCharactor : MonoBehaviour
{
	public void Show(int idCharactor, int option)
	{
		float x = 0f;
		int num = ProfileManager.rambos[idCharactor].LevelUpgrade + 1;
		float option2 = ProfileManager.rambos[idCharactor].GetOption(option);
		float x2 = option2 / (float)this.MAX_VALUE * 130f;
		if (num < ProfileManager.rambos[idCharactor].optionProfile[option].Length)
		{
			float num2 = ProfileManager.rambos[idCharactor].GetOption(option) + ProfileManager.rambos[idCharactor].optionProfile[option][num];
			x = num2 / (float)this.MAX_VALUE * 130f;
		}
		LeanTween.value(base.gameObject, new Vector2(0f, 11f), new Vector2(x2, this.lines[1].sizeDelta.y), 0.5f).setOnUpdate(delegate(Vector2 val)
		{
			this.lines[1].sizeDelta = val;
			this.oldValue = val.x;
		}, null);
		this.lines[0].sizeDelta = new Vector2(x, 11f);
		this.txtValue.text = option2.ToString();
	}

	public void Show2(int idCharactor, int option)
	{
		float x = 0f;
		int num = ProfileManager.rambos[idCharactor].LevelUpgrade + 1;
		float option2 = ProfileManager.rambos[idCharactor].GetOption(option);
		float x2 = option2 / (float)this.MAX_VALUE * 130f;
		LeanTween.value(base.gameObject, new Vector2(this.oldValue, 11f), new Vector2(x2, this.lines[1].sizeDelta.y), 0.5f).setOnUpdate(delegate(Vector2 val)
		{
			this.lines[1].sizeDelta = val;
		}, null);
		this.lines[0].sizeDelta = new Vector2(x, 11f);
		this.txtValue.text = option2.ToString();
	}

	public RectTransform[] lines;

	public Text txtValue;

	public Text txtValueNew;

	public int MAX_VALUE;

	private float oldValue;
}
