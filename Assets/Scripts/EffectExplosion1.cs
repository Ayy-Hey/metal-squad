using System;
using System.Collections;
using UnityEngine;

public class EffectExplosion1 : CachingMonoBehaviour
{
	public void Show(Vector3 pos, Action<EffectExplosion1> callBack)
	{
		base.gameObject.SetActive(true);
		this.callBack = callBack;
		this.transform.position = pos;
		this.particle.Play();
		if (this.effAudio)
		{
			SingletonGame<AudioController>.Instance.PlaySound(this.effAudio, 1f);
		}
		base.StartCoroutine(this.WaitParticleDone());
	}

	private IEnumerator WaitParticleDone()
	{
		yield return new WaitUntil(() => this.particle.isStopped);
		try
		{
			base.gameObject.SetActive(false);
			this.callBack(this);
		}
		catch
		{
		}
		yield break;
	}

	[SerializeField]
	private ParticleSystem particle;

	[SerializeField]
	private AudioClip effAudio;

	private Action<EffectExplosion1> callBack;
}
