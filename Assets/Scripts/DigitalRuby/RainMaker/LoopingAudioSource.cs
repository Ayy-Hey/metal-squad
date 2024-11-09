using System;
using UnityEngine;

namespace DigitalRuby.RainMaker
{
	public class LoopingAudioSource
	{
		public LoopingAudioSource(MonoBehaviour script, AudioClip clip)
		{
			this.AudioSource = script.gameObject.AddComponent<AudioSource>();
			this.AudioSource.loop = true;
			this.AudioSource.clip = clip;
			this.AudioSource.playOnAwake = false;
			this.AudioSource.volume = 0f;
			this.AudioSource.Stop();
			this.TargetVolume = 1f;
		}

		public AudioSource AudioSource { get; private set; }

		public float TargetVolume { get; private set; }

		public void Play(float targetVolume)
		{
			if (!ProfileManager.settingProfile.IsMusic)
			{
				return;
			}
			if (!this.AudioSource.isPlaying)
			{
				this.AudioSource.volume = 0f;
				this.AudioSource.Play();
			}
			this.TargetVolume = targetVolume;
		}

		public void Stop()
		{
			this.TargetVolume = 0f;
		}

		public void Update()
		{
			bool flag = GameManager.Instance.StateManager.EState == EGamePlay.LOST || GameManager.Instance.StateManager.EState == EGamePlay.WIN || !ProfileManager.settingProfile.IsMusic;
			if (this.AudioSource.isPlaying)
			{
				float num = Mathf.Lerp(this.AudioSource.volume, this.TargetVolume, Time.deltaTime);
				this.AudioSource.volume = num;
				if (num == 0f)
				{
					goto IL_84;
				}
			}
			if (!flag)
			{
				return;
			}
			IL_84:
			this.AudioSource.Stop();
		}
	}
}
