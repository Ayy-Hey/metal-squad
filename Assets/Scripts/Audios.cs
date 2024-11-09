using System;
using System.Collections;
using UnityEngine;

public class Audios : MonoBehaviour
{
	public void Init()
	{
		if (this.audios.Length > 0)
		{
			this.audioSource = this.audios[UnityEngine.Random.Range(0, this.audios.Length)];
		}
		base.StartCoroutine(this.Hide());
	}

	private IEnumerator Hide()
	{
		yield return this.timeHide;
		this.audioSource.Stop();
		base.gameObject.SetActive(false);
		yield break;
	}

	private void OnDisable()
	{
		switch (this.types)
		{
		case 1:
			GameManager.Instance.audioManager.ThrownGrenadePool.Store(this);
			break;
		case 2:
			GameManager.Instance.audioManager.BoomPool.Store(this);
			break;
		case 6:
			GameManager.Instance.audioManager.EnemyDiePool.Store(this);
			break;
		case 8:
			GameManager.Instance.audioManager.KnifePool.Store(this);
			break;
		case 9:
			GameManager.Instance.audioManager.CoinPool.Store(this);
			break;
		}
	}

	public AudioSource audioSource;

	public AudioSource[] audios;

	public int types;

	private WaitForSeconds timeHide = new WaitForSeconds(1f);
}
