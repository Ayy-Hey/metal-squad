using System;
using UnityEngine;

public class BPDebug : MonoBehaviour
{
	public static BPDebug Instance
	{
		get
		{
			if (BPDebug.mInstance == null)
			{
				BPDebug.mInstance = new GameObject("ScreenLogger").AddComponent<BPDebug>();
			}
			return BPDebug.mInstance;
		}
	}

	public static void LogMessage(string msg, bool error = false)
	{
		BPDebug.LogMessage(msg, false, error);
	}

	public static void LogMessage(string msg, bool clearScreen, bool error = false)
	{
		BPDebug instance = BPDebug.Instance;
		if (!instance.enableLog)
		{
			return;
		}
		if (clearScreen)
		{
			instance.logMessage = msg;
		}
		else
		{
			instance.lineCount++;
			if (instance.lineCount == 50)
			{
				instance.lineCount = 0;
				instance.logMessage = string.Empty;
			}
			BPDebug bpdebug = instance;
			bpdebug.logMessage = bpdebug.logMessage + "\n" + msg;
		}
	}

	private void Start()
	{
		this.logPosition = new Rect(10f, 10f, (float)(Screen.width - 10), (float)(Screen.height - 10));
	}

	private void OnGUI()
	{
		GUI.skin.label.fontSize = 40;
		GUI.Label(this.logPosition, this.logMessage);
	}

	private string logMessage = string.Empty;

	private Rect logPosition;

	private int lineCount;

	private bool enableLog;

	private static BPDebug mInstance;
}
