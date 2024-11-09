using System;
using UnityEngine;

namespace DigitalRuby.RainMaker
{
	public class RainScript : BaseRainScript
	{
		private void UpdateRain()
		{
			if (this.RainFallParticleSystem != null)
			{
				if (this.FollowCamera)
				{
                    var temp = this.RainFallParticleSystem.shape;

                    temp.shapeType = ParticleSystemShapeType.ConeVolume;
					this.RainFallParticleSystem.transform.position = this.Camera.transform.position;
					this.RainFallParticleSystem.transform.Translate(0f, this.RainHeight, this.RainForwardOffset);
					this.RainFallParticleSystem.transform.rotation = Quaternion.Euler(0f, this.Camera.transform.rotation.eulerAngles.y, 0f);
					if (this.RainMistParticleSystem != null)
					{
                        var temp1 = this.RainMistParticleSystem.shape;
                        temp1.shapeType = ParticleSystemShapeType.HemisphereShell;
						Vector3 position = this.Camera.transform.position;
						position.y += this.RainMistHeight;
						this.RainMistParticleSystem.transform.position = position;
					}
				}
				else
				{
                    var temp2 = this.RainFallParticleSystem.shape;
                    temp2.shapeType = ParticleSystemShapeType.Box;
					if (this.RainMistParticleSystem != null)
					{
                        var temp3 = this.RainMistParticleSystem.shape;
                        temp3.shapeType = ParticleSystemShapeType.Box;
						Vector3 position2 = this.RainFallParticleSystem.transform.position;
						position2.y += this.RainMistHeight;
						position2.y -= this.RainHeight;
						this.RainMistParticleSystem.transform.position = position2;
					}
				}
			}
		}

		protected override void Start()
		{
			base.Start();
		}

		protected override void Update()
		{
			base.Update();
			this.UpdateRain();
		}

		[Tooltip("The height above the camera that the rain will start falling from")]
		public float RainHeight = 25f;

		[Tooltip("How far the rain particle system is ahead of the player")]
		public float RainForwardOffset = -7f;

		[Tooltip("The top y value of the mist particles")]
		public float RainMistHeight = 3f;
	}
}
