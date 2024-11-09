using System;
using UnityEngine;

public class GeneralUISpaceCS : MonoBehaviour
{
	private void Start()
	{
		this.mainWindow.localScale = Vector3.zero;
		LeanTween.scale(this.mainWindow, new Vector3(1f, 1f, 1f), 0.6f).setEase(LeanTweenType.easeOutBack);
		this.mainParagraphText.anchoredPosition3D += new Vector3(0f, -10f, 0f);
		LeanTween.textAlpha(this.mainParagraphText, 0f, 0.6f).setFrom(0f).setDelay(0f);
		LeanTween.textAlpha(this.mainParagraphText, 1f, 0.6f).setEase(LeanTweenType.easeOutQuad).setDelay(0.6f);
		LeanTween.move(this.mainParagraphText, this.mainParagraphText.anchoredPosition3D + new Vector3(0f, 10f, 0f), 0.6f).setEase(LeanTweenType.easeOutQuad).setDelay(0.6f);
		LeanTween.textColor(this.mainTitleText, new Color(0.521568656f, 0.5686275f, 0.8745098f), 0.6f).setEase(LeanTweenType.easeOutQuad).setDelay(0.6f).setLoopPingPong().setRepeat(-1);
		LeanTween.textAlpha(this.mainButton2, 1f, 2f).setFrom(0f).setDelay(0f).setEase(LeanTweenType.easeOutQuad);
		LeanTween.alpha(this.mainButton2, 1f, 2f).setFrom(0f).setDelay(0f).setEase(LeanTweenType.easeOutQuad);
		this.pauseWindow.anchoredPosition3D += new Vector3(0f, 200f, 0f);
		LeanTween.moveY(this.pauseWindow, this.pauseWindow.anchoredPosition3D.y + -200f, 0.6f).setEase(LeanTweenType.easeOutSine).setDelay(0.6f);
		RectTransform component = this.pauseWindow.Find("PauseText").GetComponent<RectTransform>();
		LeanTween.moveZ(component, component.anchoredPosition3D.z - 80f, 1.5f).setEase(LeanTweenType.punch).setDelay(2f);
		LeanTween.rotateAroundLocal(this.pauseRing1, Vector3.forward, 360f, 12f).setRepeat(-1);
		LeanTween.rotateAroundLocal(this.pauseRing2, Vector3.forward, -360f, 22f).setRepeat(-1);
		this.chatWindow.RotateAround(this.chatWindow.position, Vector3.up, -180f);
		LeanTween.rotateAround(this.chatWindow, Vector3.up, 180f, 2f).setEase(LeanTweenType.easeOutElastic).setDelay(1.2f);
		LeanTween.play(this.chatRect, this.chatSprites).setLoopPingPong();
		LeanTween.color(this.chatBar2, new Color(0.972549f, 0.2627451f, 0.423529416f, 0.5f), 1.2f).setEase(LeanTweenType.easeInQuad).setLoopPingPong().setDelay(1.2f);
		LeanTween.scale(this.chatBar2, new Vector2(1f, 0.7f), 1.2f).setEase(LeanTweenType.easeInQuad).setLoopPingPong();
	}

	public RectTransform mainWindow;

	public RectTransform mainParagraphText;

	public RectTransform mainTitleText;

	public RectTransform mainButton1;

	public RectTransform mainButton2;

	public RectTransform pauseRing1;

	public RectTransform pauseRing2;

	public RectTransform pauseWindow;

	public RectTransform chatWindow;

	public RectTransform chatRect;

	public Sprite[] chatSprites;

	public RectTransform chatBar1;

	public RectTransform chatBar2;
}
