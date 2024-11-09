using System;
using System.Collections;
using Spine.Unity;
using UnityEngine;

public class PingPongColorSpine : SingletonGame<PingPongColorSpine>
{
	public void PingPongColor(SkeletonAnimation skeleton, float time)
	{
		base.StartCoroutine(this.StartPingPongColor(skeleton, time));
	}

	public void PingPongColor(SkeletonAnimation skeleton, float time, Color colorStart, Color colorEnd)
	{
		base.StartCoroutine(this.StartPingPongColor(skeleton, time, colorStart, colorEnd));
	}

	private IEnumerator StartPingPongColor(SkeletonAnimation skeleton, float timePingPongColor)
	{
		while (timePingPongColor > 0f)
		{
			timePingPongColor -= Time.fixedDeltaTime;
			Color c = Color.Lerp(this.color0, this.color1, Mathf.PingPong(Time.time * 10f, 1f));
			skeleton.skeleton.SetColor(c);
			yield return 0;
		}
		skeleton.skeleton.SetColor(Color.white);
		yield break;
	}

	private IEnumerator StartPingPongColor(SkeletonAnimation skeleton, float timePingPongColor, Color colorStart, Color colorEnd)
	{
		while (timePingPongColor > 0f)
		{
			timePingPongColor -= Time.fixedDeltaTime;
			Color c = Color.Lerp(colorStart, colorEnd, Mathf.PingPong(Time.time * 10f, 1f));
			skeleton.skeleton.SetColor(c);
			yield return 0;
		}
		skeleton.skeleton.SetColor(Color.white);
		yield break;
	}

	public Color color0 = new Color(1f, 0.25f, 0f);

	public Color color1 = new Color(1f, 0.75f, 0f);
}
