using System;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalRuby.RainMaker
{
	public class RainCollision : MonoBehaviour
	{
		private void Start()
		{
		}

		private void Update()
		{
		}

		private void Emit(ParticleSystem p, ref Vector3 pos)
		{
			for (int num = UnityEngine.Random.Range(2, 5); num != 0; num--)
			{
				float y = UnityEngine.Random.Range(1f, 3f);
				float z = UnityEngine.Random.Range(-2f, 2f);
				float x = UnityEngine.Random.Range(-2f, 2f);
				float startSize = UnityEngine.Random.Range(0.05f, 0.1f);
				p.Emit(new ParticleSystem.EmitParams
				{
					position = pos,
					velocity = new Vector3(x, y, z),
					startLifetime = 0.75f,
					startSize = startSize,
					startColor = RainCollision.color
				}, 1);
			}
		}

		private void OnParticleCollision(GameObject obj)
		{
			if (this.RainExplosion != null && this.RainParticleSystem != null)
			{
				int num = this.RainParticleSystem.GetCollisionEvents(obj, this.collisionEvents);
				for (int i = 0; i < num; i++)
				{
					Vector3 intersection = this.collisionEvents[i].intersection;
					this.Emit(this.RainExplosion, ref intersection);
				}
			}
		}

		private static readonly Color32 color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		private readonly List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

		public ParticleSystem RainExplosion;

		public ParticleSystem RainParticleSystem;
	}
}
