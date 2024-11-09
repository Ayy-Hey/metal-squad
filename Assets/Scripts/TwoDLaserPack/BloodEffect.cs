using System;
using UnityEngine;

namespace TwoDLaserPack
{
	public class BloodEffect : MonoBehaviour
	{
		private void Awake()
		{
			this.sprite = base.gameObject.GetComponent<SpriteRenderer>();
		}

		private void OnEnable()
		{
		}

		private void OnDisable()
		{
			this.spriteColor = new Color(this.sprite.GetComponent<Renderer>().material.color.r, this.sprite.GetComponent<Renderer>().material.color.g, this.sprite.GetComponent<Renderer>().material.color.b, 1f);
		}

		private void Start()
		{
		}

		private void Update()
		{
			this.elapsedTimeBeforeFadeStarts += Time.deltaTime;
			if (this.elapsedTimeBeforeFadeStarts >= this.timeBeforeFadeStarts)
			{
				this.spriteColor = new Color(this.sprite.GetComponent<Renderer>().material.color.r, this.sprite.GetComponent<Renderer>().material.color.g, this.sprite.GetComponent<Renderer>().material.color.b, Mathf.Lerp(this.sprite.GetComponent<Renderer>().material.color.a, 0f, Time.deltaTime * this.fadespeed));
				this.sprite.GetComponent<Renderer>().material.color = this.spriteColor;
				if (this.sprite.material.color.a <= 0f)
				{
					base.gameObject.SetActive(false);
				}
			}
		}

		public float fadespeed = 2f;

		public float timeBeforeFadeStarts = 1f;

		private float elapsedTimeBeforeFadeStarts;

		private SpriteRenderer sprite;

		private Color spriteColor;
	}
}
