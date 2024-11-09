using System;
using UnityEngine;

public class LuoiCua : MonoBehaviour
{
	private void Update()
	{
		if (!this.isInit)
		{
			return;
		}
		base.transform.Rotate(0f, 0f, this.speedRotate);
	}

	private void OnBecameVisible()
	{
		if (!this.mAudio.isPlaying && ProfileManager.settingProfile.IsSound)
		{
			this.mAudio.Play();
		}
		this.isInit = true;
		if (this.isMovePingpong)
		{
			LeanTween.moveX(base.gameObject, this.tf1.position.x, 2f).setLoopPingPong();
			this.isMovePingpong = false;
		}
	}

	private void OnBecameInvisible()
	{
		if (this.mAudio != null)
		{
			this.mAudio.Stop();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Rambo"))
		{
			try
			{
				collision.GetComponent<IHealth>().AddHealthPoint(-this.damage, EWeapon.NONE);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.Log(ex.Message);
			}
		}
	}

	[SerializeField]
	private float damage;

	[Header("tốc độ xoay lưỡi cưa")]
	[SerializeField]
	private float speedRotate;

	[SerializeField]
	private AudioSource mAudio;

	private bool isInit;

	public bool isMovePingpong;

	public Transform tf1;
}
