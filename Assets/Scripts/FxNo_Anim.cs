using System;
using UnityEngine;

public class FxNo_Anim : CachingMonoBehaviour
{
	public void Init(Vector3 pos, Vector3 scale, Action<FxNo_Anim> hideCallback)
	{
		this.actionHide = hideCallback;
		base.gameObject.SetActive(true);
		this.transform.position = pos;
		this.transform.localScale = scale;
		if (this.audioClip)
		{
			SingletonGame<AudioController>.Instance.PlaySound(this.audioClip, 0.2f);
		}
	}

	public void OnEventCompleted()
	{
		base.gameObject.SetActive(false);
		this.actionHide(this);
	}

	public AudioClip audioClip;

	private Action<FxNo_Anim> actionHide;
}
