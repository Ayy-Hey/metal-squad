using System;
using UnityEngine;

public class TrapLaser : MonoBehaviour
{
	public void Init()
	{
		this.isInit = true;
		this.mAudio.Stop();
		this.ObjLaser.gameObject.SetActive(false);
		this.obj1.SetActive(false);
		this.obj2.SetActive(false);
		if (this.line1 != null)
		{
			this.line1.sortingLayerName = "Gameplay";
			this.line1.sortingOrder = 55;
		}
		if (this.line2 != null)
		{
			this.line2.sortingLayerName = "Gameplay";
			this.line2.sortingOrder = 55;
		}
	}

	public void UpdateTrap()
	{
		if (!this.isInit)
		{
			return;
		}
		if (this.isRotate)
		{
			base.transform.Rotate(0f, 0f, Time.deltaTime * 20f);
		}
		float num = Vector2.Distance(base.transform.position, GameManager.Instance.player.GetPosition());
		if (num <= 12.8f)
		{
			this.ObjLaser.gameObject.SetActive(true);
			this.obj1.SetActive(true);
			this.obj2.SetActive(true);
			if (!this.mAudio.isPlaying && ProfileManager.settingProfile.IsSound)
			{
				this.mAudio.Play();
			}
		}
		if (base.transform.position.x < GameManager.Instance.player.GetPosition().x - 6.4f)
		{
			this.isInit = false;
			base.gameObject.SetActive(false);
			this.obj1.SetActive(false);
			this.obj2.SetActive(false);
		}
	}

	public void Pause()
	{
		this.ObjLaser.gameObject.SetActive(false);
		this.obj1.SetActive(false);
		this.obj2.SetActive(false);
		if (this.mAudio.isPlaying)
		{
			this.mAudio.Pause();
		}
	}

	public void Resume()
	{
		float num = Vector2.Distance(base.transform.position, GameManager.Instance.player.GetPosition());
		if (num <= 12.8f)
		{
			this.ObjLaser.gameObject.SetActive(true);
			this.obj1.SetActive(true);
			this.obj2.SetActive(true);
			if (!this.mAudio.isPlaying && ProfileManager.settingProfile.IsSound)
			{
				this.mAudio.Play();
			}
		}
	}

	public void SetDistance(float distance = 2f)
	{
		Vector2 zero = Vector2.zero;
		zero.x = distance;
		this.obj2.transform.localPosition = zero;
	}

	public void SetAngle(float angle)
	{
		base.transform.eulerAngles = new Vector3(0f, 0f, angle);
	}

	public void SetRotate(bool isRotate)
	{
	}

	private void OnDisable()
	{
		try
		{
			JetpackManager.Instance.TrapLaserPool.Store(this);
		}
		catch
		{
		}
	}

	private bool isInit;

	private bool isRotate;

	public AudioSource mAudio;

	public GameObject ObjLaser;

	public GameObject obj1;

	public GameObject obj2;

	[SerializeField]
	private LineRenderer line1;

	[SerializeField]
	private LineRenderer line2;
}
