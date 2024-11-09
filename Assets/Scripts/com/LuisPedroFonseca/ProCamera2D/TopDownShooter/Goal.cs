using System;
using UnityEngine;

namespace Com.LuisPedroFonseca.ProCamera2D.TopDownShooter
{
	public class Goal : MonoBehaviour
	{
		private void OnTriggerEnter(Collider other)
		{
			this.GameOverScreen.ShowScreen();
		}

		public GameOver GameOverScreen;
	}
}
