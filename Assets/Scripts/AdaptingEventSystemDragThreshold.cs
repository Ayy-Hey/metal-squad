using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class AdaptingEventSystemDragThreshold : MonoBehaviour
{
	private void Awake()
	{
		if (this.runOnAwake)
		{
			this.UpdatePixelDrag(Screen.dpi);
		}
	}

	public void UpdatePixelDrag(float screenDpi)
	{
		if (this.eventSystem == null)
		{
			UnityEngine.Debug.LogWarning("Trying to set pixel drag for adapting to screen dpi, but there is no event system assigned to the script", this);
		}
		this.eventSystem.pixelDragThreshold = Mathf.RoundToInt(screenDpi / (float)this.referenceDPI * this.referencePixelDrag);
	}

	private void Reset()
	{
		if (this.eventSystem == null)
		{
			this.eventSystem = base.GetComponent<EventSystem>();
		}
	}

	[SerializeField]
	private EventSystem eventSystem;

	[SerializeField]
	private int referenceDPI = 100;

	[SerializeField]
	private float referencePixelDrag = 8f;

	[SerializeField]
	private bool runOnAwake = true;
}
