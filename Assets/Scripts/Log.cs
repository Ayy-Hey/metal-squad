using System;
using System.Diagnostics;
using UnityEngine;

public class Log
{
	[Conditional("ENABLE_LOG")]
	public static void Info(object message)
	{
	}

	[Conditional("ENABLE_LOG")]
	public static void Warning(object message)
	{
	}

	[Conditional("ENABLE_LOG")]
	public static void Error(object message)
	{
	}

	[Conditional("ENABLE_LOG")]
	public static void DrawRay(Vector3 origin, Vector3 direction, Color color, float duration)
	{
	}
}
