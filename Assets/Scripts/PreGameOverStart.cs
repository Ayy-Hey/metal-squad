using System;
using UnityEngine;

public class PreGameOverStart : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Rambo"))
		{
			if (PreGameOver.Instance.OnStarted != null)
			{
				PreGameOver.Instance.OnStarted();
			}
			base.gameObject.SetActive(false);
		}
	}
}
