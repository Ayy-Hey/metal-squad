using System;
using UnityEngine;

public class BatdauDalan : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Rambo"))
		{
			this.dalanManager.OnBegin();
		}
	}

	[SerializeField]
	private DalanManager dalanManager;
}
