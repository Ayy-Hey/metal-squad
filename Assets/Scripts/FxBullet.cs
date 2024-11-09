using System;
using UnityEngine;

public class FxBullet : MonoBehaviour
{
	public void Show(Vector3 pos)
	{
		base.transform.position = pos;
		base.gameObject.SetActive(true);
		this.isInit = true;
		this.time = 0f;
		this.particle.Play();
	}

	public void OnUpdate(float deltaTime)
	{
		if (!this.isInit)
		{
			return;
		}
		this.time += Time.deltaTime;
		if (this.time >= 1f)
		{
			base.gameObject.SetActive(false);
		}
	}

	private void OnDisable()
	{
		this.isInit = false;
		this.particle.Stop();
		if (this.isPreview)
		{
			PreviewWeapon.Instance.PoolFxBullet.Store(this);
		}
		else
		{
			GameManager.Instance.fxManager.PoolFxBullet.Store(this);
		}
	}

	private bool isInit;

	private float time;

	[SerializeField]
	private ParticleSystem particle;

	[SerializeField]
	private bool isPreview;
}
