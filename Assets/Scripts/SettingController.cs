using System;
using UnityEngine;

public class SettingController : MonoBehaviour
{
	private void OnEnable()
	{
		if ((float)Screen.width < (float)Screen.height * 1.7f)
		{
			base.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
		}
		else
		{
			base.transform.localScale = Vector3.one;
		}
	}

	private void Start()
	{
		this.MusicOff.SetActive(!ProfileManager.settingProfile.IsMusic);
		this.SoundOff.SetActive(!ProfileManager.settingProfile.IsSound);
	}

	public void OnClickMusic()
	{
		AudioClick.Instance.OnClick();
		ProfileManager.settingProfile.IsMusic = !ProfileManager.settingProfile.IsMusic;
		this.MusicOff.SetActive(!ProfileManager.settingProfile.IsMusic);
		if (ProfileManager.settingProfile.IsMusic)
		{
			SingletonGame<AudioController>.Instance.PlayMusic(MenuManager.Instance.bgMenu);
		}
		else
		{
			SingletonGame<AudioController>.Instance.StopMusic();
		}
		
	}

	public void OnClickSound()
	{
		AudioClick.Instance.OnClick();
		ProfileManager.settingProfile.IsSound = !ProfileManager.settingProfile.IsSound;
		this.SoundOff.SetActive(!ProfileManager.settingProfile.IsSound);
		
	}

	public GameObject MusicOff;

	public GameObject SoundOff;
}
