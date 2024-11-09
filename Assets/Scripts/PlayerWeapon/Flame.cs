using System;
using UnityEngine;

namespace PlayerWeapon
{
	public class Flame : MonoBehaviour
	{
		private void OnEnable()
		{
			SingletonGame<AudioController>.Instance.PlayLoopSound_P(this.mSound);
		}

		private void OnDisable()
		{
			SingletonGame<AudioController>.Instance.StopAudio_P();
		}

		[SerializeField]
		private AudioClip mSound;
	}
}
