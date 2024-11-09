using System;
using UnityEngine;

namespace Com.LuisPedroFonseca.ProCamera2D.TopDownShooter
{
	public class DoorKey : MonoBehaviour
	{
		private void OnTriggerEnter(Collider other)
		{
			if (other.transform.CompareTag(this.PickupTag) && !this.Door.IsOpen)
			{
				this.Door.OpenDoor(-1f);
				if (this.Cinematics != null)
				{
					this.Cinematics.Play();
				}
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		public Door Door;

		public string PickupTag = "Player";

		public ProCamera2DCinematics Cinematics;
	}
}
