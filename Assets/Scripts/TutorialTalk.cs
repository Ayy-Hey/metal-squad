using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTalk : MonoBehaviour
{
	public void Show(string text, int idIcon, float time, Action callback)
	{
		base.gameObject.SetActive(true);
		this.icon.sprite = this.icons[idIcon];
		this.callback = callback;
		if (!this.isMap)
		{
			base.StartCoroutine(this.TypeText(text, time));
		}
		else
		{
			this.txtTalk.text = text;
		}
	}

	private IEnumerator TypeText(string msg, float time)
	{
		this.txtTalk.text = string.Empty;
		foreach (char letter in msg.ToCharArray())
		{
			Text text = this.txtTalk;
			text.text += letter;
			if (ProfileManager.settingProfile.IsSound && !this.soundTyping.isPlaying)
			{
				this.soundTyping.Play();
			}
			yield return new WaitForSeconds(time);
		}
		this.soundTyping.Stop();
		yield return new WaitForSeconds(1f);
		this.Hide();
		yield break;
	}

	public void Hide()
	{
		if (this.callback != null)
		{
			this.callback();
		}
	}

	[SerializeField]
	private Sprite[] icons;

	[SerializeField]
	private Image icon;

	[SerializeField]
	private Text txtTalk;

	[SerializeField]
	private AudioSource soundTyping;

	private Action callback;

	[SerializeField]
	private bool isMap;
}
