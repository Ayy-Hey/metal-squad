using System;
using UnityEngine;

namespace PlayerStory
{
	public class PlayerSetupGameBegin : MonoBehaviour
	{
		public void OnRun(Action OnEnded)
		{
			this.OnEnded = OnEnded;
			base.gameObject.SetActive(true);
			if (this.tfBegin == null)
			{
				GameManager.Instance.player.transform.position = PlayerManagerStory.Instance.posPlayerStart;
			}
			else
			{
				GameManager.Instance.player.transform.position = this.tfBegin.position;
			}
			this.isReadyRun = true;
			GameManager.Instance.player.isAutoRun = true;
		}

		private void Update()
		{
			if (!this.isReadyRun)
			{
				return;
			}
			GameManager.Instance.player.OnMovement(BaseCharactor.EMovementBasic.Right);
		}

		private void OnTriggerEnter2D(Collider2D col)
		{
			if (col.CompareTag("Rambo"))
			{
				if (this.boxCurrent != null)
				{
					this.boxCurrent.enabled = false;
				}
				GameManager.Instance.player.isAutoRun = false;
				this.isReadyRun = false;
				if (this.OnEnded != null)
				{
					this.OnEnded();
				}
			}
		}

		public Transform tfBegin;

		public BoxCollider2D boxCurrent;

		private Action OnEnded;

		private bool isReadyRun;
	}
}
