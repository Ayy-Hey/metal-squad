using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class FadeText : MonoBehaviour
{
	private void Start()
	{
		this.textRef = base.GetComponent<Text>();
	}

	private void Update()
	{
		this.alpha = this.textRef.color.a;
		if (this.alpha > 0f)
		{
			this.alpha -= 0.01f;
			Color color = new Color(1f, 1f, 1f, this.alpha);
			this.textRef.color = color;
		}
	}

	private Text textRef;

	public float alpha;
}
