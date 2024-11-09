using System;
using UnityEngine;

public class DinoFlame : MonoBehaviour
{
	public void Active()
	{
		base.gameObject.SetActive(true);
		this.particle.Play();
	}

	public void Off()
	{
		this.particle.Stop();
		base.gameObject.SetActive(false);
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("Rambo"))
		{
			try
			{
				GameManager.Instance.player.AddHealthPoint(-this.damage, EWeapon.NONE);
			}
			catch
			{
			}
		}
	}

	public ParticleSystem particle;

	public float damage;
}
