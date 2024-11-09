using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDrag : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
{
	private void Start()
	{
		this.anchorCanvas = base.GetComponentInParent<Canvas>().rectTransform().anchoredPosition;
		this.camSize = Camera.main.orthographicSize;
		this.scaleSize = this.anchorCanvas.y / this.camSize;
		this.camSizeX = this.camSize * this.anchorCanvas.x / this.anchorCanvas.y;
	}

	private void Update()
	{
		if (this.isDragging)
		{
			this.pos = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
			this.pos.y = this.pos.y + this.camSize;
			this.pos.x = this.pos.x + this.camSizeX;
			base.transform.position = this.scaleSize * this.pos;
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		this.isDragging = true;
		try
		{
			this.OnStartDrag(this);
		}
		catch
		{
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		this.isDragging = false;
		try
		{
			this.OnEndDrag(this);
		}
		catch
		{
		}
	}

	public Action<UIDrag> OnStartDrag;

	public Action<UIDrag> OnEndDrag;

	[HideInInspector]
	public bool isDragging;

	[HideInInspector]
	public Vector2 pos;

	public Vector2 anchorCanvas;

	private float camSize;

	private float scaleSize;

	private float camSizeX;
}
