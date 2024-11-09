using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListRevival : MonoBehaviour
{
	private IEnumerator Start()
	{
		yield return new WaitUntil(() => GameManager.Instance.StateManager.EState == EGamePlay.RUNNING);
		GameManager.Instance.listRevival.Clear();
		for (int i = 0; i < this.ListObject.Count; i++)
		{
			if (this.ListObject[i] != null)
			{
				GameManager.Instance.listRevival.Add(this.ListObject[i]);
			}
		}
		yield break;
	}

	public List<Transform> ListObject;
}
