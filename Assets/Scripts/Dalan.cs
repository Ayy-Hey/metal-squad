using System;
using UnityEngine;

public class Dalan : CachingMonoBehaviour
{
	public void OnInit(Vector3 pos)
	{
		base.gameObject.SetActive(true);
		this.transform.position = pos;
		if (ProfileManager.settingProfile.IsSound)
		{
			this.mAudio.Play();
		}
	}

	private void OnDisable()
	{
		try
		{
			this.mAudio.Stop();
			TrapManager.Instance.PoolDalan.Store(this);
		}
		catch
		{
		}
	}

	public float Damaged = 10f;

	[SerializeField]
	private AudioSource mAudio;
}
