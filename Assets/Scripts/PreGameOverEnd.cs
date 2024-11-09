using System;
using UnityEngine;

public class PreGameOverEnd : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Rambo"))
		{
			if (this.isOnEnd)
			{
				return;
			}
			this.isOnEnd = true;
			if (PreGameOver.Instance.OnEnded != null)
			{
				PreGameOver.Instance.OnEnded();
			}
			base.gameObject.SetActive(false);
		}
	}

	private bool isOnEnd;
}
