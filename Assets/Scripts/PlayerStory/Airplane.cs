using System;
using UnityEngine;

namespace PlayerStory
{
	public class Airplane : CachingMonoBehaviour
	{
		public void OnGameBegin(Action OnJumpNow)
		{
			base.gameObject.SetActive(true);
			this.transform.position = this.tfBegin.position;
			this.OnJumpNow = OnJumpNow;
			this.isDropPlayer = false;
			if (ProfileManager.settingProfile.IsMusic)
			{
				this.mAudio.volume = 0.3f;
				this.mAudio.Play();
			}
		}

		private void FixedUpdate()
		{
			if (this.isHold)
			{
				this.timeCounDownHold += Time.deltaTime;
				if (this.timeCounDownHold > 1f)
				{
					this.speed = 10f;
					this.isHold = false;
				}
				return;
			}
			Vector2 position = this.rigidbody2D.position;
			this.rigidbody2D.MovePosition(position + this.speed * Vector2.right * Time.deltaTime);
		}

		private void OnTriggerEnter2D(Collider2D col)
		{
			if (col.CompareTag("Found_Wall"))
			{
				if (this.OnJumpNow != null)
				{
					this.OnJumpNow();
				}
				this.timeCounDownHold = 0f;
				this.isHold = true;
				this.isDropPlayer = true;
				col.gameObject.SetActive(false);
			}
		}

		private void OnBecameInvisible()
		{
			if (this.isDropPlayer)
			{
				base.gameObject.SetActive(false);
			}
		}

		private Action OnJumpNow;

		private bool isDropPlayer;

		public Transform tfBegin;

		private bool isHold;

		private float timeCounDownHold;

		public AudioSource mAudio;

		private float speed = 5f;
	}
}
