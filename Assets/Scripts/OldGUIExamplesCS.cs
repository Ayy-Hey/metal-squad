using System;
using UnityEngine;

public class OldGUIExamplesCS : MonoBehaviour
{
	private void Start()
	{
		this.w = (float)Screen.width;
		this.h = (float)Screen.height;
		this.buttonRect1 = new LTRect(0.1f * this.w, 0.8f * this.h, 0.2f * this.w, 0.14f * this.h);
		this.buttonRect2 = new LTRect(1.2f * this.w, 0.8f * this.h, 0.2f * this.w, 0.14f * this.h);
		this.buttonRect3 = new LTRect(0.35f * this.w, 0f * this.h, 0.3f * this.w, 0.2f * this.h, 0f);
		this.buttonRect4 = new LTRect(0f * this.w, 0.4f * this.h, 0.3f * this.w, 0.2f * this.h, 1f, 15f);
		this.grumpyRect = new LTRect(0.5f * this.w - (float)this.grumpy.width * 0.5f, 0.5f * this.h - (float)this.grumpy.height * 0.5f, (float)this.grumpy.width, (float)this.grumpy.height);
		this.beautyTileRect = new LTRect(0f, 0f, 1f, 1f);
		LeanTween.move(this.buttonRect2, new Vector2(0.55f * this.w, this.buttonRect2.rect.y), 0.7f).setEase(LeanTweenType.easeOutQuad);
	}

	public void catMoved()
	{
		UnityEngine.Debug.Log("cat moved...");
	}

	private void OnGUI()
	{
		GUI.DrawTexture(this.grumpyRect.rect, this.grumpy);
		Rect position = new Rect(0f * this.w, 0f * this.h, 0.2f * this.w, 0.14f * this.h);
		if (GUI.Button(position, "Move Cat") && !LeanTween.isTweening(this.grumpyRect))
		{
			Vector2 to = new Vector2(this.grumpyRect.rect.x, this.grumpyRect.rect.y);
			LeanTween.move(this.grumpyRect, new Vector2(1f * (float)Screen.width - (float)this.grumpy.width, 0f * (float)Screen.height), 1f).setEase(LeanTweenType.easeOutBounce).setOnComplete(new Action(this.catMoved));
			LeanTween.move(this.grumpyRect, to, 1f).setDelay(1f).setEase(LeanTweenType.easeOutBounce);
		}
		if (GUI.Button(this.buttonRect1.rect, "Scale Centered"))
		{
			LeanTween.scale(this.buttonRect1, new Vector2(this.buttonRect1.rect.width, this.buttonRect1.rect.height) * 1.2f, 0.25f).setEase(LeanTweenType.easeOutQuad);
			LeanTween.move(this.buttonRect1, new Vector2(this.buttonRect1.rect.x - this.buttonRect1.rect.width * 0.1f, this.buttonRect1.rect.y - this.buttonRect1.rect.height * 0.1f), 0.25f).setEase(LeanTweenType.easeOutQuad);
		}
		if (GUI.Button(this.buttonRect2.rect, "Scale"))
		{
			LeanTween.scale(this.buttonRect2, new Vector2(this.buttonRect2.rect.width, this.buttonRect2.rect.height) * 1.2f, 0.25f).setEase(LeanTweenType.easeOutBounce);
		}
		position = new Rect(0.76f * this.w, 0.53f * this.h, 0.2f * this.w, 0.14f * this.h);
		if (GUI.Button(position, "Flip Tile"))
		{
			LeanTween.move(this.beautyTileRect, new Vector2(0f, this.beautyTileRect.rect.y + 1f), 1f).setEase(LeanTweenType.easeOutBounce);
		}
		GUI.DrawTextureWithTexCoords(new Rect(0.8f * this.w, 0.5f * this.h - (float)this.beauty.height * 0.5f, (float)this.beauty.width * 0.5f, (float)this.beauty.height * 0.5f), this.beauty, this.beautyTileRect.rect);
		if (GUI.Button(this.buttonRect3.rect, "Alpha"))
		{
			LeanTween.alpha(this.buttonRect3, 0f, 1f).setEase(LeanTweenType.easeOutQuad);
			LeanTween.alpha(this.buttonRect3, 1f, 1f).setDelay(1f).setEase(LeanTweenType.easeInQuad);
			LeanTween.alpha(this.grumpyRect, 0f, 1f).setEase(LeanTweenType.easeOutQuad);
			LeanTween.alpha(this.grumpyRect, 1f, 1f).setDelay(1f).setEase(LeanTweenType.easeInQuad);
		}
		GUI.color = new Color(1f, 1f, 1f, 1f);
		if (GUI.Button(this.buttonRect4.rect, "Rotate"))
		{
			LeanTween.rotate(this.buttonRect4, 150f, 1f).setEase(LeanTweenType.easeOutElastic);
			LeanTween.rotate(this.buttonRect4, 0f, 1f).setDelay(1f).setEase(LeanTweenType.easeOutElastic);
		}
		GUI.matrix = Matrix4x4.identity;
	}

	public Texture2D grumpy;

	public Texture2D beauty;

	private float w;

	private float h;

	private LTRect buttonRect1;

	private LTRect buttonRect2;

	private LTRect buttonRect3;

	private LTRect buttonRect4;

	private LTRect grumpyRect;

	private LTRect beautyTileRect;
}
