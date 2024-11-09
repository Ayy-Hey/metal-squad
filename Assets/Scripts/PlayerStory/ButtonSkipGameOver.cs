using System;
using UnityEngine;

namespace PlayerStory
{
	public class ButtonSkipGameOver : MonoBehaviour
	{
		public void OnSkip()
		{
			PlayerManagerStory.Instance.playerGameOver.OnSkip();
			base.gameObject.SetActive(false);
		}
	}
}
