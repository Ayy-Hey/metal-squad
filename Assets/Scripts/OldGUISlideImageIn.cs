using System;
using UnityEngine;

[Serializable]
public class OldGUISlideImageIn : MonoBehaviour
{
	public virtual void Start()
	{
		this.grumpyRect = new LTRect((float)(-(float)this.grumpy.width), 0.5f * (float)Screen.height - (float)this.grumpy.height / 2f, (float)this.grumpy.width, (float)this.grumpy.height);
		LeanTween.move(this.grumpyRect, new Vector2(0.5f * (float)Screen.width - (float)this.grumpy.width / 2f, this.grumpyRect.rect.y), 1f).setEase(LeanTweenType.easeOutQuad);
	}

	public virtual void OnGUI()
	{
		GUI.DrawTexture(this.grumpyRect.rect, this.grumpy);
	}

	public virtual void Main()
	{
	}

	public Texture2D grumpy;

	private LTRect grumpyRect;
}
