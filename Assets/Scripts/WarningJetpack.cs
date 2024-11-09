using System;
using UnityEngine;

public class WarningJetpack : CachingMonoBehaviour
{
	public void ShowWarning(float y)
	{
		this.y = y;
		Vector2 anchoredPosition = this.rectPos.anchoredPosition;
		anchoredPosition.y = y;
		this.rectPos.anchoredPosition = anchoredPosition;
		this.counter = 0f;
		this.isShoot = true;
		if (this.mAudio != null && !this.mAudio.isPlaying && ProfileManager.settingProfile.IsSound)
		{
			this.mAudio.Play();
		}
	}

	public override void UpdateObject()
	{
		if (!this.isShoot)
		{
			return;
		}
		this.counter += Time.deltaTime;
		if (this.counter >= 3f)
		{
			JetpackManager.Instance.CreateRocket(this.y / 100f);
			base.gameObject.SetActive(false);
			this.isShoot = false;
		}
	}

	private void OnDisable()
	{
		try
		{
			JetpackManager.Instance.WarningPool.Store(this);
		}
		catch (Exception ex)
		{
		}
	}

	private const float TIME_WAIT = 2f;

	private float counter;

	private bool isShoot;

	private float y;

	public RectTransform rectPos;

	public AudioSource mAudio;
}
