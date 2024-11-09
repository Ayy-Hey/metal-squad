using System;
using UnityEngine;

public class LogoCinematic : MonoBehaviour
{
	private void Awake()
	{
	}

	private void Start()
	{
		this.tween.transform.localPosition += -Vector3.right * 15f;
		LeanTween.moveLocalX(this.tween, this.tween.transform.localPosition.x + 15f, 0.4f).setEase(LeanTweenType.linear).setDelay(0f).setOnComplete(new Action(this.playBoom));
		this.tween.transform.RotateAround(this.tween.transform.position, Vector3.forward, -30f);
		LeanTween.rotateAround(this.tween, Vector3.forward, 30f, 0.4f).setEase(LeanTweenType.easeInQuad).setDelay(0.4f).setOnComplete(new Action(this.playBoom));
		this.lean.transform.position += Vector3.up * 5.1f;
		LeanTween.moveY(this.lean, this.lean.transform.position.y - 5.1f, 0.6f).setEase(LeanTweenType.easeInQuad).setDelay(0.6f).setOnComplete(new Action(this.playBoom));
	}

	private void playBoom()
	{
		AnimationCurve volume = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(-0.001454365f, 0.006141067f, -3.698472f, -3.698472f),
			new Keyframe(0.007561419f, 1.006896f, -3.613532f, -3.613532f),
			new Keyframe(0.9999977f, 0.00601998f, -0.1788428f, -0.1788428f)
		});
		AnimationCurve frequency = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0.001724138f, 0.01912267f, 0.01912267f),
			new Keyframe(0.9981073f, 0.007586207f, 0f, 0f)
		});
		AudioClip audio = LeanAudio.createAudio(volume, frequency, LeanAudio.options().setVibrato(new Vector3[]
		{
			new Vector3(0.1f, 0f, 0f)
		}).setFrequency(11025));
		LeanAudio.play(audio, Vector3.zero, 1f, 1f);
	}

	public GameObject lean;

	public GameObject tween;
}
