using System;
using System.Collections;
using UnityEngine;

public class DemoAirplaneSpMan : DemoBaseObject
{
	private void OnEnable()
	{
		base.Init();
		base.StartCoroutine(this.Move());
		this.canMove = true;
	}

	private void Update()
	{
		if (this.canMove)
		{
			base.transform.Translate(Time.deltaTime * this.speed, 0f, 0f);
		}
	}

	private IEnumerator Move()
	{
		yield return new WaitForSeconds(this.timeMove);
		this.canMove = false;
		this.manSP.SetActive(true);
		yield return new WaitForSeconds(0.3f);
		this.canMove = true;
		yield return new WaitForSeconds(this.timeMove);
		this.canMove = false;
		yield break;
	}

	[SerializeField]
	private float speed;

	[SerializeField]
	private float timeMove;

	[SerializeField]
	private GameObject manSP;

	private bool canMove;

	private bool isActiveMan;
}
