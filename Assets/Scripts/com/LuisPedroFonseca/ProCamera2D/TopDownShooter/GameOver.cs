using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Com.LuisPedroFonseca.ProCamera2D.TopDownShooter
{
	public class GameOver : MonoBehaviour
	{
		private void Awake()
		{
			this.GameOverScreen.gameObject.SetActive(false);
		}

		public void ShowScreen()
		{
			this.GameOverScreen.gameObject.SetActive(true);
			Time.timeScale = 0f;
		}

		public void PlayAgain()
		{
			Time.timeScale = 1f;
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

		public Canvas GameOverScreen;
	}
}
