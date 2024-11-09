using System;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
	private void OnEnable()
	{
		if (this.objCanvasTutorial != null)
		{
			this.objCanvasTutorial.SetActive(false);
		}
		float num = Mathf.Clamp(((float)Screen.width / (float)Screen.height - 1.333f) / 0.444f, 0f, 1f);
		this.rect_Foreground.localScale = Vector3.one * (0.9f + 0.2f * num);
		this.text.text = PopupManager.Instance.GetText((Localization0)UnityEngine.Random.Range(4, 24), null);
	}

	public Text text;

	[SerializeField]
	private RectTransform rect_Foreground;

	public GameObject objCanvasTutorial;
}
