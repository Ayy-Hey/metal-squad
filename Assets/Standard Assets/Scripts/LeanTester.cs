using System;
using System.Collections;
using UnityEngine;

public class LeanTester : MonoBehaviour
{
	public void Start()
	{
		base.StartCoroutine(this.timeoutCheck());
	}

	private IEnumerator timeoutCheck()
	{
		float pauseEndTime = Time.realtimeSinceStartup + this.timeout;
		while (Time.realtimeSinceStartup < pauseEndTime)
		{
			yield return 0;
		}
		if (!LeanTest.testsFinished)
		{
			UnityEngine.Debug.Log(LeanTest.formatB("Tests timed out!"));
			LeanTest.overview();
		}
		yield break;
	}

	public float timeout = 15f;
}
