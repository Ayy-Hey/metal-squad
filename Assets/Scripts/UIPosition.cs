using System;
using UnityEngine;
using UnityEngine.UI;

public class UIPosition : CachingMonoBehaviour
{
	private void OnEnable()
	{
		if (!this.rect)
		{
			this.rect = base.GetComponent<RectTransform>();
		}
		if (this.canDrag)
		{
			this.uiDrag.OnStartDrag = delegate(UIDrag obj)
			{
				if (this.imgHightlight)
				{
					this.imgHightlight.enabled = true;
				}
				if (!object.ReferenceEquals(this.OnTouch, null))
				{
					this.OnTouch(this);
				}
			};
			this.uiDrag.OnEndDrag = delegate(UIDrag obj)
			{
				this.anchoredPos = this.rect.anchoredPosition;
			};
		}
		this.LoadData();
	}

	private void LoadData()
	{
	}

	public void SetPosition(Vector2 anchored)
	{
		this.anchoredPos = anchored;
		this.anchoredPos.x = Mathf.Min(640f, Mathf.Max(-640f, this.anchoredPos.x));
		this.anchoredPos.y = Mathf.Min(360f, Mathf.Max(-360f, this.anchoredPos.y));
		this.rect.anchoredPosition = this.anchoredPos;
	}

	public void SetSize(float size)
	{
		this.scale.x = size;
		this.scale.y = size;
		this.scale.x = size;
		this.transform.localScale = this.scale;
	}

	public void SetAlpha(float alpha)
	{
		this.color.a = alpha;
		if (object.ReferenceEquals(this.images, null))
		{
			return;
		}
		for (int i = 0; i < this.images.Length; i++)
		{
			if (this.canDrag)
			{
				this.images[i].color = this.color;
			}
			else
			{
				ETCButton component = this.images[i].GetComponent<ETCButton>();
				if (component)
				{
					component.normalColor = (component.pressedColor = this.color);
					MonoBehaviour.print(base.name + alpha);
				}
				else
				{
					this.images[i].color = this.color;
				}
			}
		}
	}

	public void Save()
	{
	}

	public void Default()
	{
		if (this.imgHightlight)
		{
			this.imgHightlight.enabled = false;
		}
		this.LoadData();
	}

	public Action<UIPosition> OnTouch;

	public EOptionControl option;

	public bool canDrag;

	public UIDrag uiDrag;

	public Image[] images;

	public Image imgHightlight;

	[HideInInspector]
	public Vector2 anchoredPos;

	[HideInInspector]
	public Vector3 scale;

	[HideInInspector]
	public Color color = Color.white;

	private Vector2 startPos;

	[HideInInspector]
	public RectTransform rect;
}
