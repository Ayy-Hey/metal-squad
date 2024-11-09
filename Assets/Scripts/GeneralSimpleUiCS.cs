using System;
using UnityEngine;
using UnityEngine.UI;

public class GeneralSimpleUiCS : MonoBehaviour
{
	private void Start()
	{
		UnityEngine.Debug.Log("For better examples see the 4.6_Examples folder!");
		if (this.button == null)
		{
			UnityEngine.Debug.LogError("Button not assigned! Create a new button via Hierarchy->Create->UI->Button. Then assign it to the button variable");
			return;
		}
		LeanTween.value(this.button.gameObject, this.button.anchoredPosition, new Vector2(200f, 100f), 1f).setOnUpdate(delegate(Vector2 val)
		{
			this.button.anchoredPosition = val;
		}, null);
		LeanTween.value(base.gameObject, 1f, 0.5f, 1f).setOnUpdate(delegate(float volume)
		{
			UnityEngine.Debug.Log("volume:" + volume);
		});
		LeanTween.value(base.gameObject, base.gameObject.transform.position, base.gameObject.transform.position + new Vector3(0f, 1f, 0f), 1f).setOnUpdate(delegate(Vector3 val)
		{
			base.gameObject.transform.position = val;
		}, null);
		LeanTween.value(base.gameObject, Color.red, Color.green, 1f).setOnUpdate(delegate(Color val)
		{
			Image image = (Image)this.button.gameObject.GetComponent(typeof(Image));
			image.color = val;
		});
		LeanTween.move(this.button, new Vector3(200f, -100f, 0f), 1f).setDelay(1f);
		LeanTween.rotateAround(this.button, Vector3.forward, 90f, 1f).setDelay(2f);
		LeanTween.scale(this.button, this.button.localScale * 2f, 1f).setDelay(3f);
		LeanTween.rotateAround(this.button, Vector3.forward, -90f, 1f).setDelay(4f).setEase(LeanTweenType.easeInOutElastic);
	}

	public RectTransform button;
}
