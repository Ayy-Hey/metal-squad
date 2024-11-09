using System;
using UnityEngine;

public class BulletWithTailCollision : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Enemy") || other.CompareTag("Tank") || other.CompareTag("Boss") || other.CompareTag("Ground"))
		{
			this.objParrent.SetActive(false);
		}
	}

	public GameObject objParrent;

	private int DAMAGED = 20;
}
