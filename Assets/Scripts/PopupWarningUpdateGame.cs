using System;
using UnityEngine;
using UnityEngine.UI;

public class PopupWarningUpdateGame : PopupBase
{
	public override void Show()
	{
		base.Show();
		for (int i = 0; i < FirebaseDatabaseManager.Instance._version.ReleaseNotes.Length; i++)
		{
			if (!string.IsNullOrEmpty(FirebaseDatabaseManager.Instance._version.ReleaseNotes[i]))
			{
				Text text = UnityEngine.Object.Instantiate<Text>(this.txtChild, this.tfParrent);
				text.gameObject.SetActive(true);
				text.text = FirebaseDatabaseManager.Instance._version.ReleaseNotes[i];
			}
		}
	}

	public void UpdateGame()
	{
		this.OnClose();
		string url_RATE_ANDROID = GameConfig.URL_RATE_ANDROID;
		Application.OpenURL(url_RATE_ANDROID);
	}

	public Transform tfParrent;

	public Text txtChild;
}
