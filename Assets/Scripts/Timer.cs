using System;
using System.Collections;
using UnityEngine;

public class Timer
{
	public static void Schedule(MonoBehaviour _behaviour, float delay, Timer.Task task)
	{
		Timer.behaviour = _behaviour;
		Timer.behaviour.StartCoroutine(Timer.DoTask(task, delay));
	}

	private static IEnumerator DoTask(Timer.Task task, float delay)
	{
		yield return new WaitForSeconds(delay);
		task();
		yield break;
	}

	private static MonoBehaviour behaviour;

	public delegate void Task();
}
