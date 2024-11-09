using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
	private void OnEnable()
	{
		for (int i = this.s.Length - 1; i >= 0; i--)
		{
			if (this.y >= this.s[i])
			{
				this.n = i;
				break;
			}
		}
		this.SetValue();
	}

	private void SetValue()
	{
		if (this.old < this.n)
		{
			this.x = ((this.old < 0) ? 0f : this.s[this.old]);
			this.old++;
			this.z = this.s[this.old];
		}
		else if (this.y > this.z)
		{
			this.x = this.s[this.old];
			this.z = this.y;
		}
		this.imgCurrent.sprite = this.sprites[this.old];
		this.imgNext.sprite = this.sprites[this.old + 1];
		this.t = this.x;
		this.delta = Mathf.Round((this.z - this.x) / 200f);
		this.delta = Mathf.Max(this.delta, 1f);
		if (this.x < this.y)
		{
			base.StartCoroutine(this.Run());
		}
	}

	private IEnumerator Run()
	{
		while (this.x < this.z)
		{
			this.x = Mathf.MoveTowards(this.x, this.z, this.delta);
			if (this.z == this.s[this.old])
			{
				this.raw.fillAmount = (this.x - this.t) / (this.z - this.t);
				this.txt.text = this.x + "/" + this.z;
			}
			else
			{
				this.raw.fillAmount = (this.x - this.t) / (this.s[Mathf.Min(this.old + 1, this.s.Length - 1)] - this.t);
				this.txt.text = this.x + "/" + this.s[Mathf.Min(this.old + 1, this.s.Length - 1)];
			}
			yield return 0;
		}
		if (this.z == this.s[this.old])
		{
			MonoBehaviour.print(this.old);
		}
		this.SetValue();
		yield break;
	}

	private void Update()
	{
	}

	public Sprite sp;

	public TextAsset vip;

	public SpriteRenderer spriteRenderer;

	public Image raw;

	public LayerMask mask;

	private float distanceGround;

	private float distance;

	private RaycastHit2D[] hit;

	public bool run;

	public float[] s;

	public int old;

	public int n;

	public float x;

	public float z;

	public float y;

	public float delta;

	public float t;

	public Text txt;

	public Image imgCurrent;

	public Image imgNext;

	public Sprite[] sprites;

	private bool isHit;
}
