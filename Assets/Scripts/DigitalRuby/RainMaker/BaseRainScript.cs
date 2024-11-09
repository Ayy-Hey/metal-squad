using System;
using UnityEngine;

namespace DigitalRuby.RainMaker
{
	public class BaseRainScript : MonoBehaviour
	{
		private void UpdateWind()
		{
			if (this.EnableWind && this.WindZone != null && this.WindSpeedRange.y > 1f)
			{
				this.WindZone.gameObject.SetActive(true);
				if (this.FollowCamera)
				{
					this.WindZone.transform.position = this.Camera.transform.position;
				}
				if (!this.Camera.orthographic)
				{
					this.WindZone.transform.Translate(0f, this.WindZone.radius, 0f);
				}
				if (this.nextWindTime < Time.time)
				{
					this.WindZone.windMain = UnityEngine.Random.Range(this.WindSpeedRange.x, this.WindSpeedRange.y);
					this.WindZone.windTurbulence = UnityEngine.Random.Range(this.WindSpeedRange.x, this.WindSpeedRange.y);
					if (this.Camera.orthographic)
					{
						int num = UnityEngine.Random.Range(0, 2);
						this.WindZone.transform.rotation = Quaternion.Euler(0f, (num != 0) ? -90f : 90f, 0f);
					}
					else
					{
						this.WindZone.transform.rotation = Quaternion.Euler(UnityEngine.Random.Range(-30f, 30f), UnityEngine.Random.Range(0f, 360f), 0f);
					}
					this.nextWindTime = Time.time + UnityEngine.Random.Range(this.WindChangeInterval.x, this.WindChangeInterval.y);
					this.audioSourceWind.Play(this.WindZone.windMain / this.WindSpeedRange.z * this.WindSoundVolumeModifier);
				}
			}
			else
			{
				if (this.WindZone != null)
				{
					this.WindZone.gameObject.SetActive(false);
				}
				this.audioSourceWind.Stop();
			}
			this.audioSourceWind.Update();
		}

		private void CheckForRainChange()
		{
			if (this.lastRainIntensityValue != this.RainIntensity)
			{
				this.lastRainIntensityValue = this.RainIntensity;
				if (this.RainIntensity <= 0.01f)
				{
					if (this.audioSourceRainCurrent != null)
					{
						this.audioSourceRainCurrent.Stop();
						this.audioSourceRainCurrent = null;
					}
					if (this.RainFallParticleSystem != null)
					{
                        var temp = this.RainFallParticleSystem.emission;

                        temp.enabled = false;
						this.RainFallParticleSystem.Stop();
					}
					if (this.RainMistParticleSystem != null)
					{
                        var temp = this.RainFallParticleSystem.emission;
                        temp.enabled = false;
						this.RainMistParticleSystem.Stop();
					}
				}
				else
				{
					LoopingAudioSource loopingAudioSource;
					if (this.RainIntensity >= 0.67f)
					{
						loopingAudioSource = this.audioSourceRainHeavy;
					}
					else if (this.RainIntensity >= 0.33f)
					{
						loopingAudioSource = this.audioSourceRainMedium;
					}
					else
					{
						loopingAudioSource = this.audioSourceRainLight;
					}
					if (this.audioSourceRainCurrent != loopingAudioSource)
					{
						if (this.audioSourceRainCurrent != null)
						{
							this.audioSourceRainCurrent.Stop();
						}
						this.audioSourceRainCurrent = loopingAudioSource;
						this.audioSourceRainCurrent.Play(1f);
					}
					if (this.RainFallParticleSystem != null)
					{
						ParticleSystem.EmissionModule emission = this.RainFallParticleSystem.emission;
						bool enabled = true;
						this.RainFallParticleSystem.GetComponent<Renderer>().enabled = enabled;
						emission.enabled = enabled;
						if (!this.RainFallParticleSystem.isPlaying)
						{
							this.RainFallParticleSystem.Play();
						}
						ParticleSystem.MinMaxCurve rateOverTime = emission.rateOverTime;
						rateOverTime.mode = ParticleSystemCurveMode.Constant;
						float num = this.RainFallEmissionRate();
						rateOverTime.constantMax = num;
						rateOverTime.constantMin = num;
						emission.rateOverTime = rateOverTime;
					}
					if (this.RainMistParticleSystem != null)
					{
						ParticleSystem.EmissionModule emission2 = this.RainMistParticleSystem.emission;
						bool enabled = true;
						this.RainMistParticleSystem.GetComponent<Renderer>().enabled = enabled;
						emission2.enabled = enabled;
						if (!this.RainMistParticleSystem.isPlaying)
						{
							this.RainMistParticleSystem.Play();
						}
						float num2;
						if (this.RainIntensity < this.RainMistThreshold)
						{
							num2 = 0f;
						}
						else
						{
							num2 = this.MistEmissionRate();
						}
						ParticleSystem.MinMaxCurve rateOverTime2 = emission2.rateOverTime;
						rateOverTime2.mode = ParticleSystemCurveMode.Constant;
						float num = num2;
						rateOverTime2.constantMax = num;
						rateOverTime2.constantMin = num;
						emission2.rateOverTime = rateOverTime2;
					}
				}
			}
		}

		protected virtual void Start()
		{
			if (this.Camera == null)
			{
				this.Camera = Camera.main;
			}
			this.audioSourceRainLight = new LoopingAudioSource(this, this.RainSoundLight);
			this.audioSourceRainMedium = new LoopingAudioSource(this, this.RainSoundMedium);
			this.audioSourceRainHeavy = new LoopingAudioSource(this, this.RainSoundHeavy);
			this.audioSourceWind = new LoopingAudioSource(this, this.WindSound);
			if (this.RainFallParticleSystem != null)
			{
                var temp = this.RainFallParticleSystem.emission;
                temp.enabled = false;
				Renderer component = this.RainFallParticleSystem.GetComponent<Renderer>();
				component.enabled = false;
				this.rainMaterial = new Material(component.material);
				this.rainMaterial.EnableKeyword("SOFTPARTICLES_OFF");
				component.material = this.rainMaterial;
			}
			if (this.RainExplosionParticleSystem != null)
			{
                var temp = this.RainExplosionParticleSystem.emission;
                temp.enabled = false;
				Renderer component2 = this.RainExplosionParticleSystem.GetComponent<Renderer>();
				this.rainExplosionMaterial = new Material(component2.material);
				this.rainExplosionMaterial.EnableKeyword("SOFTPARTICLES_OFF");
				component2.material = this.rainExplosionMaterial;
			}
			if (this.RainMistParticleSystem != null)
			{
                var temp = this.RainMistParticleSystem.emission;
                temp.enabled = false;
				Renderer component3 = this.RainMistParticleSystem.GetComponent<Renderer>();
				component3.enabled = false;
				this.rainMistMaterial = new Material(component3.material);
				if (this.UseRainMistSoftParticles)
				{
					this.rainMistMaterial.EnableKeyword("SOFTPARTICLES_ON");
				}
				else
				{
					this.rainMistMaterial.EnableKeyword("SOFTPARTICLES_OFF");
				}
				component3.material = this.rainMistMaterial;
			}
		}

		protected virtual void Update()
		{
			this.CheckForRainChange();
			this.UpdateWind();
			this.audioSourceRainLight.Update();
			this.audioSourceRainMedium.Update();
			this.audioSourceRainHeavy.Update();
		}

		protected virtual float RainFallEmissionRate()
		{
			return (float)this.RainFallParticleSystem.main.maxParticles / this.RainFallParticleSystem.main.startLifetime.constant * this.RainIntensity;
		}

		protected virtual float MistEmissionRate()
		{
			return (float)this.RainMistParticleSystem.main.maxParticles / this.RainMistParticleSystem.main.startLifetime.constant * this.RainIntensity * this.RainIntensity;
		}

		protected virtual bool UseRainMistSoftParticles
		{
			get
			{
				return true;
			}
		}

		[Tooltip("Camera the rain should hover over, defaults to main camera")]
		public Camera Camera;

		[Tooltip("Whether rain should follow the camera. If false, rain must be moved manually and will not follow the camera.")]
		public bool FollowCamera = true;

		[Tooltip("Light rain looping clip")]
		public AudioClip RainSoundLight;

		[Tooltip("Medium rain looping clip")]
		public AudioClip RainSoundMedium;

		[Tooltip("Heavy rain looping clip")]
		public AudioClip RainSoundHeavy;

		[Range(0f, 1f)]
		[Tooltip("Intensity of rain (0-1)")]
		public float RainIntensity;

		[Tooltip("Rain particle system")]
		public ParticleSystem RainFallParticleSystem;

		[Tooltip("Particles system for when rain hits something")]
		public ParticleSystem RainExplosionParticleSystem;

		[Tooltip("Particle system to use for rain mist")]
		public ParticleSystem RainMistParticleSystem;

		[Range(0f, 1f)]
		[Tooltip("The threshold for intensity (0 - 1) at which mist starts to appear")]
		public float RainMistThreshold = 0.5f;

		[Tooltip("Wind looping clip")]
		public AudioClip WindSound;

		[Tooltip("Wind sound volume modifier, use this to lower your sound if it's too loud.")]
		public float WindSoundVolumeModifier = 0.5f;

		[Tooltip("Wind zone that will affect and follow the rain")]
		public WindZone WindZone;

		[Tooltip("X = minimum wind speed. Y = maximum wind speed. Z = sound multiplier. Wind speed is divided by Z to get sound multiplier value. Set Z to lower than Y to increase wind sound volume, or higher to decrease wind sound volume.")]
		public Vector3 WindSpeedRange = new Vector3(50f, 500f, 500f);

		[Tooltip("How often the wind speed and direction changes (minimum and maximum change interval in seconds)")]
		public Vector2 WindChangeInterval = new Vector2(5f, 30f);

		[Tooltip("Whether wind should be enabled.")]
		public bool EnableWind = true;

		protected LoopingAudioSource audioSourceRainLight;

		protected LoopingAudioSource audioSourceRainMedium;

		protected LoopingAudioSource audioSourceRainHeavy;

		protected LoopingAudioSource audioSourceRainCurrent;

		protected LoopingAudioSource audioSourceWind;

		protected Material rainMaterial;

		protected Material rainExplosionMaterial;

		protected Material rainMistMaterial;

		private float lastRainIntensityValue = -1f;

		private float nextWindTime;
	}
}
