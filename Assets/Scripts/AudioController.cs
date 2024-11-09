using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : SingletonGame<AudioController>
{
	private void Awake()
	{
		this.audioSource = base.gameObject.AddComponent<AudioSource>();
		this.audioS_Music = base.gameObject.AddComponent<AudioSource>();
		this.audioS_Player = base.gameObject.AddComponent<AudioSource>();
		this.ListAudioSource = new List<AudioSource>();
		this.audioSource.playOnAwake = false;
		this.audioS_Music.playOnAwake = false;
		this.audioS_Player.playOnAwake = false;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	private AudioSource AddAudioSource(float volume)
	{
		AudioSource audioSource = base.gameObject.AddComponent<AudioSource>();
		audioSource.loop = false;
		audioSource.volume = volume;
		audioSource.playOnAwake = false;
		this.ListAudioSource.Add(audioSource);
		return audioSource;
	}

	private AudioSource GetAudioS(float volume)
	{
		for (int i = 0; i < this.ListAudioSource.Count; i++)
		{
			if (this.ListAudioSource[i].volume == volume)
			{
				return this.ListAudioSource[i];
			}
		}
		return this.AddAudioSource(volume);
	}

	private void StopListAudioS()
	{
		for (int i = 0; i < this.ListAudioSource.Count; i++)
		{
			this.ListAudioSource[i].Stop();
		}
	}

	public void Play(AudioClip clip, float volume)
	{
		this.GetAudioS(volume).PlayOneShot(clip);
	}

	public void PlayMusic(AudioClip clip)
	{
		if (ProfileManager.settingProfile.IsMusic)
		{
			if (this.audioS_Music.isPlaying && this.audioS_Music.clip == clip)
			{
				return;
			}
			this.audioS_Music.clip = clip;
			this.audioS_Music.loop = true;
			this.audioS_Music.volume = 1f;
			this.audioS_Music.Play();
		}
	}

	public void PlaySound(AudioClip clip, float volume = 1f)
	{
		if (ProfileManager.settingProfile.IsSound)
		{
			this.Play(clip, volume);
		}
	}

	public void PlaySound_P(AudioClip clip, float volume = 1f)
	{
		if (ProfileManager.settingProfile.IsSound)
		{
			this.Play(clip, volume);
		}
	}

	public void PlayLoopSound(AudioClip clip, float volume = 1f, float speed = 1f)
	{
		if (ProfileManager.settingProfile.IsSound)
		{
			this.audioSource.clip = clip;
			this.audioSource.loop = true;
			this.audioSource.volume = volume;
			this.audioSource.pitch = speed;
			this.audioSource.Play();
		}
	}

	public void PlayLoopSound_P(AudioClip clip)
	{
		if (ProfileManager.settingProfile.IsSound)
		{
			this.audioS_Player.clip = clip;
			this.audioS_Player.loop = true;
			this.audioS_Player.volume = 1f;
			this.audioS_Player.Play();
		}
	}

	public void StopSound()
	{
		this.audioSource.Stop();
		this.StopListAudioS();
	}

	public void MuteAll(bool mute)
	{
		this.audioSource.mute = mute;
		this.audioS_Music.mute = mute;
		this.audioS_Player.mute = mute;
	}

	public void StopAll()
	{
		this.audioSource.Stop();
		this.audioS_Music.Stop();
		this.audioS_Player.Stop();
		this.StopListAudioS();
	}

	public void StopAudio_P()
	{
		this.audioS_Player.Stop();
	}

	public void StopMusic()
	{
		this.audioS_Music.Stop();
	}

	internal AudioSource audioSource;

	internal AudioSource audioS_Music;

	internal AudioSource audioS_Player;

	private List<AudioSource> ListAudioSource;
}
