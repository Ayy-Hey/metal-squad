using System;
using UnityEngine;

[Serializable]
public class OldGUIExamplesJS : MonoBehaviour
{
	public OldGUIExamplesJS()
	{
		this.w = (float)Screen.width;
		this.h = (float)Screen.height;
	}

	public virtual void Start()
	{
		this.w = (float)Screen.width;
		this.h = (float)Screen.height;
		this.buttonRect1 = new LTRect(0.1f * this.w, 0.8f * this.h, 0.25f * this.w, 0.14f * this.h);
		this.buttonRect2 = new LTRect(1.2f * this.w, 0.8f * this.h, 0.2f * this.w, 0.14f * this.h);
		this.buttonRect3 = new LTRect(0.35f * this.w, (float)0 * this.h, 0.3f * this.w, 0.2f * this.h);
		this.buttonRect4 = new LTRect((float)0 * this.w, 0.4f * this.h, 0.3f * this.w, 0.2f * this.h, 1f, 15f);
		this.grumpyRect = new LTRect(0.5f * this.w - (float)this.grumpy.width / 2f, 0.5f * this.h - (float)this.grumpy.height / 2f, (float)this.grumpy.width, (float)this.grumpy.height);
		this.beautyTileRect = new LTRect((float)0, (float)0, (float)1, (float)1);
		LeanTween.move(this.buttonRect2, new Vector2(0.55f * this.w, this.buttonRect2.rect.y), 0.7f).setEase(LeanTweenType.easeOutQuad);
	}

	public virtual void OnGUI()
	{
		Rect position = new Rect((float)0 * this.w, (float)0 * this.h, 0.2f * this.w, 0.14f * this.h);
		if (GUI.Button(position, "Move Cat") && !LeanTween.isTweening(this.grumpyRect))
		{
			Vector2 to = new Vector2(this.grumpyRect.rect.x, this.grumpyRect.rect.y);
			LeanTween.move(this.grumpyRect, new Vector2(1f * this.w - (float)this.grumpy.width, (float)0 * this.h), 1f).setEase(LeanTweenType.easeOutBounce);
			LeanTween.move(this.grumpyRect, to, 1f).setEase(LeanTweenType.easeOutBounce).setDelay(1f);
		}
		GUI.DrawTexture(this.grumpyRect.rect, this.grumpy);
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
			LeanTween.move(this.beautyTileRect, new Vector2((float)0, this.beautyTileRect.rect.y + (float)1), 1f).setEase(LeanTweenType.easeOutBounce);
		}
		GUI.DrawTextureWithTexCoords(new Rect(0.8f * this.w, 0.5f * this.h - (float)this.beauty.height / 2f, (float)this.beauty.width * 0.5f, (float)this.beauty.height * 0.5f), this.beauty, this.beautyTileRect.rect);
		if (GUI.Button(this.buttonRect3.rect, "Alpha"))
		{
			LeanTween.alpha(this.buttonRect3, (float)0, 1f).setEase(LeanTweenType.easeOutQuad);
			LeanTween.alpha(this.buttonRect3, 1f, 1f).setEase(LeanTweenType.easeOutQuad).setDelay(1f);
		}
		float a = 1f;
		Color color = GUI.color;
		float num = color.a = a;
		Color color2 = GUI.color = color;
		if (GUI.Button(this.buttonRect4.rect, "Rotate"))
		{
			LeanTween.rotate(this.buttonRect4, 150f, 1f).setEase(LeanTweenType.easeOutElastic);
			LeanTween.rotate(this.buttonRect4, (float)0, 1f).setEase(LeanTweenType.easeOutElastic).setDelay(1f);
		}
		GUI.matrix = Matrix4x4.identity;
	}

	public virtual void Main()
	{
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
