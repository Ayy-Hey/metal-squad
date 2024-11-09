using System;
using UnityEngine;

public class SkillPhunDocBoss13 : MonoBehaviour
{
	public void Active()
	{
		base.gameObject.SetActive(true);
		this.particle.Play();
	}

	public void Off()
	{
		base.gameObject.SetActive(false);
		this.particle.Stop();
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("Rambo"))
		{
			try
			{
				float num = this.damage * Time.deltaTime;
				GameManager.Instance.player.AddHealthPoint(-num, EWeapon.NONE);
				GameManager.Instance.player.TrungDoc();
			}
			catch
			{
			}
		}
	}

	public ParticleSystem particle;

	public float damage;
}
