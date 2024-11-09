using System;
using UnityEngine;

public class DungDalan : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Rambo"))
		{
			this.dalanManager.OnStop();
		}
	}

	[SerializeField]
	private DalanManager dalanManager;
}
