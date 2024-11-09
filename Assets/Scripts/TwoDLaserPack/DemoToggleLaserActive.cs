using System;
using UnityEngine;

namespace TwoDLaserPack
{
	public class DemoToggleLaserActive : MonoBehaviour
	{
		private void Start()
		{
		}

		private void OnMouseOver()
		{
			if (this.lineLaserRef != null && Input.GetMouseButtonDown(0))
			{
				this.lineLaserRef.SetLaserState(!this.lineLaserRef.laserActive);
			}
			if (this.spriteLaserRef != null && Input.GetMouseButtonDown(0))
			{
				this.spriteLaserRef.SetLaserState(!this.spriteLaserRef.laserActive);
			}
		}

		private void Update()
		{
		}

		public LineBasedLaser lineLaserRef;

		public SpriteBasedLaser spriteLaserRef;
	}
}
