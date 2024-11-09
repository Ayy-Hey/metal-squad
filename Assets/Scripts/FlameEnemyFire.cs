using System;
using UnityEngine;

public class FlameEnemyFire : CachingMonoBehaviour
{
	public void Active(float damage, Vector3 pos, Quaternion rotation)
	{
		this.damage = damage;
		base.gameObject.SetActive(true);
		this.transform.position = pos;
		this.transform.rotation = rotation;
		this.particle.Play();
		this.isInit = true;
	}

	public void Off()
	{
		this.isInit = false;
		this.particle.Stop();
		base.gameObject.SetActive(false);
	}

	public void OnUpdate(Vector3 pos, Quaternion rotation)
	{
		this.transform.position = pos;
		this.transform.rotation = rotation;
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("Rambo"))
		{
			try
			{
				GameManager.Instance.player.AddHealthPoint(-this.damage * Time.deltaTime, EWeapon.NONE);
			}
			catch
			{
			}
		}
	}

	public bool isInit;

	[SerializeField]
	private ParticleSystem particle;

	private float damage;
}
