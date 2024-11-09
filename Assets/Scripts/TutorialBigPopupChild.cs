using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialBigPopupChild : MonoBehaviour
{
	public void Show(string text, int idAvatar, Action callback)
	{
		this.callback = callback;
		this.text = text;
		this.CountTouch = 0;
		if (this.objSkip != null)
		{
			this.objSkip.SetActive(false);
		}
		base.gameObject.SetActive(true);
		if (this.isMap)
		{
			this.CountTouch = 3;
			this.txtTalk.text = text;
		}
		else
		{
			base.StartCoroutine(this.TypeText(text));
		}
	}

	private IEnumerator TypeText(string msg)
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
			yield return new WaitForSeconds(0.01f);
		}
		this.soundTyping.Stop();
		this.CountTouch += 2;
		yield break;
	}

	public void Hide()
	{
		base.StopAllCoroutines();
		this.CountTouch++;
		if (this.CountTouch >= 2)
		{
			if (this.objSkip != null)
			{
				this.objSkip.SetActive(true);
			}
			if (this.callback != null)
			{
				this.callback();
			}
		}
		else
		{
			this.txtTalk.text = this.text;
		}
	}

	[SerializeField]
	private Text txtTalk;

	[SerializeField]
	private AudioSource soundTyping;

	private Action callback;

	private int CountTouch;

	private string text;

	[SerializeField]
	private bool isMap;

	[SerializeField]
	private GameObject objSkip;
}
