using System;
using System.Collections;
using UnityEngine;

public class TestFPS : MonoBehaviour
{
	private IEnumerator Start()
	{
		yield return new WaitUntil(() => this.splass.isDone);
		for (;;)
		{
			int lastFrameCount = Time.frameCount;
			float lastTime = Time.realtimeSinceStartup;
			yield return new WaitForSeconds(this.frequency);
			float timeSpan = Time.realtimeSinceStartup - lastTime;
			int frameCount = Time.frameCount - lastFrameCount;
			this.fps = string.Format("FPS: {0}", (float)frameCount / timeSpan);
		}
		yield break;
	}

	private void OnGUI()
	{
		GUI.Label(new Rect((float)(Screen.width - 100), 50f, 150f, 100f), this.fps);
	}

	private int FramesPerSec;

	private float frequency = 1f;

	private string fps;

	public SplashScreen splass;
}
