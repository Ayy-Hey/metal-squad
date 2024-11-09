using System;
using UnityEngine;

public class AudioClick : MonoBehaviour
{
	public static AudioClick Instance
	{
		get
		{
			if (AudioClick.instance == null)
			{
				AudioClick.instance = UnityEngine.Object.FindObjectOfType<AudioClick>();
			}
			return AudioClick.instance;
		}
	}

	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	public void OnUpgrade()
	{
		if (ProfileManager.settingProfile.IsSound)
		{
			this.audioUpgrade.Play();
		}
	}

	public void OnBumb()
	{
		if (ProfileManager.settingProfile.IsSound)
		{
			this.audioBumb.Play();
		}
	}

	public void OnClick()
	{
		if (ProfileManager.settingProfile.IsSound)
		{
			this.audioClick.Play();
		}
	}

	public void OnTyping()
	{
		if (ProfileManager.settingProfile.IsSound)
		{
			this.audioTyping.Play();
		}
	}

	public void OnReceiveItem()
	{
		if (ProfileManager.settingProfile.IsSound)
		{
			this.audioReceiveItem.Play();
		}
	}

	public void OnOpenBox()
	{
		if (ProfileManager.settingProfile.IsSound)
		{
			this.audioOpenBox.Play();
		}
	}

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Debug.Break();
        }
    }

	private static AudioClick instance;

	[SerializeField]
	private AudioSource audioClick;

	[SerializeField]
	private AudioSource audioTyping;

	[SerializeField]
	private AudioSource audioBumb;

	[SerializeField]
	private AudioSource audioUpgrade;

	[SerializeField]
	private AudioSource audioReceiveItem;

	[SerializeField]
	private AudioSource audioOpenBox;
}
