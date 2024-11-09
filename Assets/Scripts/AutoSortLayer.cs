using System;
using System.Collections;
using UnityEngine;

public class AutoSortLayer : MonoBehaviour
{
	private IEnumerator Start()
	{
		yield return new WaitUntil(() => GameManager.Instance.StateManager.EState == EGamePlay.RUNNING);
		this.isInit = true;
		yield break;
	}

	private void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.CompareTag("Rambo"))
		{
			GameManager.Instance.player.isDetectedGround = true;
		}
		if (coll.gameObject.CompareTag("Enemy"))
		{
			BaseEnemy component = coll.gameObject.GetComponent<BaseEnemy>();
			if (!object.ReferenceEquals(component, null))
			{
				component.meshRenderer.sortingOrder = -2;
			}
		}
	}

	private void OnCollisionExit2D(Collision2D coll)
	{
		if (coll.gameObject.CompareTag("Rambo"))
		{
			GameManager.Instance.player.isDetectedGround = false;
		}
		if (coll.gameObject.CompareTag("Enemy"))
		{
			BaseEnemy component = coll.gameObject.GetComponent<BaseEnemy>();
			if (!object.ReferenceEquals(component, null))
			{
				component.meshRenderer.sortingOrder = 6;
			}
		}
	}

	private void Update()
	{
		if (!this.isInit)
		{
			return;
		}
		if (GameManager.Instance.player._controller.isGrounded && GameManager.Instance.player.isDetectedGround)
		{
			GameManager.Instance.player.meshRenderer.sortingOrder = -2;
		}
		else
		{
			GameManager.Instance.player.meshRenderer.sortingOrder = 19;
		}
	}

	private bool isInit;
}
