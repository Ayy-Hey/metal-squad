using System;
using UnityEngine;

public class FxNo_Particle : MonoBehaviour
{
	public void Init(Vector3 pos, float scale, Action<FxNo_Particle> hideCallback)
	{
		this.hideAction = hideCallback;
		base.gameObject.SetActive(true);
		base.transform.localScale = Vector3.one * scale;
		base.transform.position = pos;
		this.particle.Play();
		if (this.effAudio)
		{
			SingletonGame<AudioController>.Instance.PlaySound(this.effAudio, 0.15f);
		}
		this.isInit = true;
	}

	public void Init(int type, Transform parent, float scale, Action<FxNo_Particle> hideCallback)
	{
		this.hideAction = hideCallback;
		base.gameObject.SetActive(true);
		base.transform.localScale = Vector3.one * scale;
		base.transform.parent = parent;
		base.transform.localPosition = Vector3.zero;
		this.particle.startColor = ((type != 0) ? Color.magenta : Color.white);
		this.particle.Play();
		if (this.effAudio)
		{
			SingletonGame<AudioController>.Instance.PlaySound(this.effAudio, 0.5f);
		}
		this.isInit = true;
	}

	public void PlayParticle()
	{
		this.particle.Play();
	}

	public void StopParticle()
	{
		this.particle.Stop();
	}

	public void OnUpdate(float deltaTime)
	{
		if (!this.isInit)
		{
			return;
		}
		if (this.particle.isStopped)
		{
			this.isInit = false;
			try
			{
				base.gameObject.SetActive(false);
				this.hideAction(this);
			}
			catch
			{
			}
		}
	}

	[SerializeField]
	private ParticleSystem particle;

	[SerializeField]
	private AudioClip effAudio;

	private Action<FxNo_Particle> hideAction;

	[HideInInspector]
	public bool isInit;
}
