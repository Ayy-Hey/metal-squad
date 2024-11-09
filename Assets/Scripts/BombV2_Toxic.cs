using System;
using UnityEngine;

public class BombV2_Toxic : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Rambo"))
		{
			try
			{
				collision.GetComponent<BaseRambo>().TrungDoc();
			}
			catch
			{
			}
		}
	}

	public ParticleSystem particle;
}
