using System;
using UnityEngine;

namespace Com.LuisPedroFonseca.ProCamera2D.Platformer
{
	public class ToggleCinematics : MonoBehaviour
	{
		private void OnGUI()
		{
			if (GUI.Button(new Rect(5f, 5f, 180f, 30f), ((!this.Cinematics.IsPlaying) ? "Start" : "Stop") + " Cinematics"))
			{
				if (this.Cinematics.IsPlaying)
				{
					this.Cinematics.Stop();
				}
				else
				{
					this.Cinematics.Play();
				}
			}
			if (this.Cinematics.IsPlaying && GUI.Button(new Rect(195f, 5f, 40f, 30f), ">"))
			{
				this.Cinematics.GoToNextTarget();
			}
		}

		public ProCamera2DCinematics Cinematics;
	}
}
