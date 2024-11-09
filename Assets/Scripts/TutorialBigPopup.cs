using System;
using UnityEngine;

public class TutorialBigPopup : MonoBehaviour
{
	public void Show(bool isLeft, string text, int idAvatar, Action callback)
	{
		base.gameObject.SetActive(true);
		if (!this.isMap)
		{
		}
		this.callback = callback;
		if (isLeft)
		{
			this.popupRight.gameObject.SetActive(false);
			this.popupLeft.Show(text, idAvatar, delegate
			{
				callback();
			});
		}
		else
		{
			this.popupLeft.gameObject.SetActive(false);
			this.popupRight.Show(text, idAvatar, delegate
			{
				callback();
			});
		}
	}

	[SerializeField]
	private TutorialBigPopupChild popupLeft;

	[SerializeField]
	private TutorialBigPopupChild popupRight;

	private Action callback;

	[SerializeField]
	private bool isMap;
}
