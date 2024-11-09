using System;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("NGUI/Tween/Tween Alpha")]
public class TweenAlpha : UITweener
{
	[Obsolete("Use 'value' instead")]
	public float alpha
	{
		get
		{
			return this.value;
		}
		set
		{
			this.value = value;
		}
	}

	private void Cache()
	{
		this.mCached = true;
		this.mRect = base.GetComponent<UIRect>();
		this.mSr = base.GetComponent<SpriteRenderer>();
		this.mImg = base.GetComponent<Graphic>();
		this.mCg = base.GetComponent<CanvasGroup>();
		this.mLr = base.GetComponent<LineRenderer>();
		if (this.mRect == null && this.mSr == null && this.mImg == null && this.mCg == null && this.mLr == null)
		{
			Renderer component = base.GetComponent<Renderer>();
			if (component != null)
			{
				this.mMat = component.material;
			}
			if (this.mMat == null)
			{
				this.mRect = base.GetComponentInChildren<UIRect>();
			}
		}
	}

	public float value
	{
		get
		{
			if (!this.mCached)
			{
				this.Cache();
			}
			if (this.mRect != null)
			{
				return this.mRect.alpha;
			}
			if (this.mSr != null)
			{
				return this.mSr.color.a;
			}
			if (this.mCg != null)
			{
				return this.mCg.alpha;
			}
			if (this.mImg != null)
			{
				return this.mImg.color.a;
			}
			if (this.mLr != null)
			{
				return this.mLr.startColor.a;
			}
			return (!(this.mMat != null)) ? 1f : this.mMat.color.a;
		}
		set
		{
			if (!this.mCached)
			{
				this.Cache();
			}
			if (this.mRect != null)
			{
				this.mRect.alpha = value;
			}
			else if (this.mSr != null)
			{
				Color color = this.mSr.color;
				color.a = value;
				this.mSr.color = color;
			}
			else if (this.mCg != null)
			{
				this.mCg.alpha = value;
			}
			else if (this.mImg != null)
			{
				Color color2 = this.mImg.color;
				color2.a = value;
				this.mImg.color = color2;
			}
			else if (this.mLr != null)
			{
				Color startColor = this.mLr.startColor;
				startColor.a = value;
				this.mLr.startColor = startColor;
				this.mLr.endColor = startColor;
			}
			else if (this.mMat != null)
			{
				Color color3 = this.mMat.color;
				color3.a = value;
				this.mMat.color = color3;
			}
		}
	}

	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = Mathf.Lerp(this.from, this.to, factor);
	}

	public static TweenAlpha Begin(GameObject go, float duration, float alpha)
	{
		TweenAlpha tweenAlpha = UITweener.Begin<TweenAlpha>(go, duration);
		tweenAlpha.from = tweenAlpha.value;
		tweenAlpha.to = alpha;
		if (duration <= 0f)
		{
			tweenAlpha.Sample(1f, true);
			tweenAlpha.enabled = false;
		}
		return tweenAlpha;
	}

	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	[Range(0f, 1f)]
	public float from = 1f;

	[Range(0f, 1f)]
	public float to = 1f;

	private bool mCached;

	private UIRect mRect;

	private Material mMat;

	private SpriteRenderer mSr;

	private Graphic mImg;

	private CanvasGroup mCg;

	private LineRenderer mLr;
}
