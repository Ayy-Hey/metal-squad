using System;
using UnityEngine;

namespace PlayerStory
{
	public class PlayerSetupGameOverJump : MonoBehaviour
	{
		private void OnTriggerEnter2D(Collider2D col)
		{
			if (col.CompareTag("Rambo"))
			{
				base.gameObject.SetActive(false);
				GameManager.Instance.player._PlayerInput.OnJump(true);
			}
		}
	}
}
