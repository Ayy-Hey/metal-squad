using System;
using System.Collections;
using UnityEngine;

public class GoGoController : MonoBehaviour
{
	public void Show(GoGoController.Direction direction)
	{
		this.direction = direction;
		if (this.coroutine != null)
		{
			base.StopCoroutine(this.coroutine);
		}
		this.coroutine = base.StartCoroutine(this.Show((int)direction));
	}

	private IEnumerator Show(int id)
	{
		for (int i = 0; i < this.ArrDirection.Length; i++)
		{
			this.ArrDirection[i].SetActive(false);
		}
		this.ArrDirection[id].SetActive(true);
		yield return new WaitForSeconds(3f);
		this.ArrDirection[id].SetActive(false);
		yield break;
	}

	[SerializeField]
	private GameObject[] ArrDirection;

	private GoGoController.Direction direction;

	private Coroutine coroutine;

	public enum Direction
	{
		UP,
		DOWN,
		LEFT,
		RIGHT
	}
}
