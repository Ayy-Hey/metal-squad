using System;
using UnityEngine;

namespace DigitalRuby.RainMaker
{
	public class RainScript2D : BaseRainScript
	{
		private void EmitExplosion(ref Vector3 pos)
		{
			for (int num = UnityEngine.Random.Range(2, 5); num != 0; num--)
			{
				float x = UnityEngine.Random.Range(-2f, 2f) * this.cameraMultiplier;
				float y = UnityEngine.Random.Range(1f, 3f) * this.cameraMultiplier;
				float startLifetime = UnityEngine.Random.Range(0.1f, 0.2f);
				float startSize = UnityEngine.Random.Range(0.05f, 0.1f) * this.cameraMultiplier;
				ParticleSystem.EmitParams emitParams = default(ParticleSystem.EmitParams);
				emitParams.position = pos;
				emitParams.velocity = new Vector3(x, y, 0f);
				emitParams.startLifetime = startLifetime;
				emitParams.startSize = startSize;
				emitParams.startColor = RainScript2D.explosionColor;
				this.RainExplosionParticleSystem.Emit(emitParams, 1);
			}
		}

		private void TransformParticleSystem(ParticleSystem p, Vector2 initialStartSpeed, Vector2 initialStartSize)
		{
			if (p == null)
			{
				return;
			}
			if (this.FollowCamera)
			{
				p.transform.position = new Vector3(this.Camera.transform.position.x, this.visibleBounds.max.y + this.yOffset, p.transform.position.z);
			}
			else
			{
				p.transform.position = new Vector3(p.transform.position.x, this.visibleBounds.max.y + this.yOffset, p.transform.position.z);
			}
			p.transform.localScale = new Vector3(this.visibleWorldWidth * this.RainWidthMultiplier, 1f, 1f);
			ParticleSystem.MainModule main = p.main;
			ParticleSystem.MinMaxCurve startSpeed = main.startSpeed;
			ParticleSystem.MinMaxCurve startSize = main.startSize;
			startSpeed.constantMin = initialStartSpeed.x * this.cameraMultiplier;
			startSpeed.constantMax = initialStartSpeed.y * this.cameraMultiplier;
			startSize.constantMin = initialStartSize.x * this.cameraMultiplier;
			startSize.constantMax = initialStartSize.y * this.cameraMultiplier;
			main.startSpeed = startSpeed;
			main.startSize = startSize;
		}

		private void CheckForCollisionsRainParticles()
		{
			int num = 0;
			bool flag = false;
			if (this.CollisionMask != 0)
			{
				num = this.RainFallParticleSystem.GetParticles(this.particles);
				for (int i = 0; i < num; i++)
				{
					Vector3 vector = this.particles[i].position + this.RainFallParticleSystem.transform.position;
					RaycastHit2D raycastHit2D = Physics2D.Raycast(vector, this.particles[i].velocity.normalized, this.particles[i].velocity.magnitude * Time.deltaTime);
					if (raycastHit2D.collider != null && (1 << raycastHit2D.collider.gameObject.layer & this.CollisionMask) != 0)
					{
						if (this.CollisionLifeTimeRain == 0f)
						{
							this.particles[i].remainingLifetime = 0f;
						}
						else
						{
							this.particles[i].remainingLifetime = Mathf.Min(this.particles[i].remainingLifetime, UnityEngine.Random.Range(this.CollisionLifeTimeRain * 0.5f, this.CollisionLifeTimeRain * 2f));
							vector += this.particles[i].velocity * Time.deltaTime;
						}
						flag = true;
					}
				}
			}
			if (this.RainExplosionParticleSystem != null)
			{
				if (num == 0)
				{
					num = this.RainFallParticleSystem.GetParticles(this.particles);
				}
				for (int j = 0; j < num; j++)
				{
					if (this.particles[j].remainingLifetime < 0.24f)
					{
						Vector3 vector2 = this.particles[j].position + this.RainFallParticleSystem.transform.position;
						this.EmitExplosion(ref vector2);
					}
				}
			}
			if (flag)
			{
				this.RainFallParticleSystem.SetParticles(this.particles, num);
			}
		}

		private void CheckForCollisionsMistParticles()
		{
			if (this.RainMistParticleSystem == null || this.CollisionMask == 0)
			{
				return;
			}
			int num = this.RainMistParticleSystem.GetParticles(this.particles);
			bool flag = false;
			for (int i = 0; i < num; i++)
			{
				Vector3 v = this.particles[i].position + this.RainMistParticleSystem.transform.position;
				RaycastHit2D raycastHit2D = Physics2D.Raycast(v, this.particles[i].velocity.normalized, this.particles[i].velocity.magnitude * Time.deltaTime);
				if (raycastHit2D.collider != null && (1 << raycastHit2D.collider.gameObject.layer & this.CollisionMask) != 0)
				{
					ParticleSystem.Particle[] array = this.particles;
					int num2 = i;
					array[num2].velocity = array[num2].velocity * this.RainMistCollisionMultiplier;
					flag = true;
				}
			}
			if (flag)
			{
				this.RainMistParticleSystem.SetParticles(this.particles, num);
			}
		}

		protected override void Start()
		{
			base.Start();
			this.initialEmissionRain = this.RainFallParticleSystem.emission.rateOverTime.constant;
			this.initialStartSpeedRain = new Vector2(this.RainFallParticleSystem.main.startSpeed.constantMin, this.RainFallParticleSystem.main.startSpeed.constantMax);
			this.initialStartSizeRain = new Vector2(this.RainFallParticleSystem.main.startSize.constantMin, this.RainFallParticleSystem.main.startSize.constantMax);
			if (this.RainMistParticleSystem != null)
			{
				this.initialStartSpeedMist = new Vector2(this.RainMistParticleSystem.main.startSpeed.constantMin, this.RainMistParticleSystem.main.startSpeed.constantMax);
				this.initialStartSizeMist = new Vector2(this.RainMistParticleSystem.main.startSize.constantMin, this.RainMistParticleSystem.main.startSize.constantMax);
			}
			if (this.RainExplosionParticleSystem != null)
			{
				this.initialStartSpeedExplosion = new Vector2(this.RainExplosionParticleSystem.main.startSpeed.constantMin, this.RainExplosionParticleSystem.main.startSpeed.constantMax);
				this.initialStartSizeExplosion = new Vector2(this.RainExplosionParticleSystem.main.startSize.constantMin, this.RainExplosionParticleSystem.main.startSize.constantMax);
			}
		}

		protected override void Update()
		{
			base.Update();
			this.cameraMultiplier = this.Camera.orthographicSize * 0.25f;
			this.visibleBounds.min = Camera.main.ViewportToWorldPoint(Vector3.zero);
			this.visibleBounds.max = Camera.main.ViewportToWorldPoint(Vector3.one);
			this.visibleWorldWidth = this.visibleBounds.size.x;
			this.yOffset = (this.visibleBounds.max.y - this.visibleBounds.min.y) * this.RainHeightMultiplier;
			this.TransformParticleSystem(this.RainFallParticleSystem, this.initialStartSpeedRain, this.initialStartSizeRain);
			this.TransformParticleSystem(this.RainMistParticleSystem, this.initialStartSpeedMist, this.initialStartSizeMist);
			this.TransformParticleSystem(this.RainExplosionParticleSystem, this.initialStartSpeedExplosion, this.initialStartSizeExplosion);
			this.CheckForCollisionsRainParticles();
			this.CheckForCollisionsMistParticles();
		}

		protected override float RainFallEmissionRate()
		{
			return this.initialEmissionRain * this.RainIntensity;
		}

		protected override bool UseRainMistSoftParticles
		{
			get
			{
				return false;
			}
		}

		private static readonly Color32 explosionColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		private float cameraMultiplier = 1f;

		private Bounds visibleBounds = default(Bounds);

		private float yOffset;

		private float visibleWorldWidth;

		private float initialEmissionRain;

		private Vector2 initialStartSpeedRain;

		private Vector2 initialStartSizeRain;

		private Vector2 initialStartSpeedMist;

		private Vector2 initialStartSizeMist;

		private Vector2 initialStartSpeedExplosion;

		private Vector2 initialStartSizeExplosion;

		private readonly ParticleSystem.Particle[] particles = new ParticleSystem.Particle[2048];

		[Tooltip("The starting y offset for rain and mist. This will be offset as a percentage of visible height from the top of the visible world.")]
		public float RainHeightMultiplier = 0.15f;

		[Tooltip("The total width of the rain and mist as a percentage of visible width")]
		public float RainWidthMultiplier = 1.5f;

		[Tooltip("Collision mask for the rain particles")]
		public LayerMask CollisionMask = -1;

		[Tooltip("Lifetime to assign to rain particles that have collided. 0 for instant death. This can allow the rain to penetrate a little bit beyond the collision point.")]
		[Range(0f, 0.5f)]
		public float CollisionLifeTimeRain = 0.02f;

		[Range(0f, 0.99f)]
		[Tooltip("Multiply the velocity of any mist colliding by this amount")]
		public float RainMistCollisionMultiplier = 0.75f;
	}
}
