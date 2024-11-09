using System;
using System.Collections;
using UnityEngine;

public class StopMoveWithoutGround : MonoBehaviour
{
	private IEnumerator Start()
	{
		yield return new WaitUntil(() => GameManager.Instance.StateManager.EState == EGamePlay.RUNNING);
		this.box.enabled = true;
		yield break;
	}

	public BoxCollider2D box;
}
