using System;
using UnityEngine;

public class LineBloodEnemy : MonoBehaviour
{
	public void Show(float hpCurrent, float HP, bool isAutoHide = true)
	{
		this.isAutoHide = isAutoHide;
		for (int i = 0; i < this.lineSprite.Length; i++)
		{
			this.lineSprite[i].color = Color.white;
		}
		if (this.lineFx != null)
		{
			this.lineFx.color = Color.white;
			this.lineFx.transform.localScale = (this.lineFxScale = this.lineSprite[1].transform.localScale);
		}
		this.vtRate = new Vector3(Mathf.Clamp01(hpCurrent / HP), 1f, 1f);
		this.lineSprite[1].transform.localScale = this.vtRate;
		this.countdown = 3f;
	}

	private void LateUpdate()
	{
		if (this.countdown <= 0f || !this.isAutoHide || this.lineFx == null)
		{
			return;
		}
		float deltaTime = Time.deltaTime;
		this.countdown -= deltaTime;
		this.countdown = Mathf.Max(0f, this.countdown);
		for (int i = 0; i < this.lineSprite.Length; i++)
		{
			this.lineSprite[i].color = new Color(1f, 1f, 1f, this.countdown / 3f);
		}
		this.lineFx.color = new Color(1f, 1f, 1f, this.countdown / 6f);
		this.lineFxScale.x = Mathf.MoveTowards(this.lineFxScale.x, this.vtRate.x, deltaTime / 3f);
		this.lineFx.transform.localScale = this.lineFxScale;
	}

	public void Reset()
	{
		if (!this.lineFx)
		{
			if (this.lineSprite[0].transform.childCount == 1)
			{
				this.lineFx = UnityEngine.Object.Instantiate<SpriteRenderer>(this.lineSprite[1], this.lineSprite[1].transform.position, this.lineSprite[1].transform.rotation, this.lineSprite[1].transform.parent);
			}
			else
			{
				this.lineFx = this.lineSprite[0].transform.GetChild(1).GetComponent<SpriteRenderer>();
			}
		}
		Transform transform = this.lineFx.transform;
		Vector3 one = Vector3.one;
		this.lineSprite[1].transform.localScale = one;
		transform.localScale = one;
		SpriteRenderer spriteRenderer = this.lineFx;
		Color color = new Color(1f, 1f, 1f, 0f);
		this.lineSprite[1].color = color;
		color = color;
		this.lineSprite[0].color = color;
		spriteRenderer.color = color;
		this.countdown = 0f;
	}

	public void Hide()
	{
		base.gameObject.SetActive(false);
	}

	private void updateValueExampleCallback(Color val)
	{
		for (int i = 0; i < this.lineSprite.Length; i++)
		{
			this.lineSprite[i].color = val;
		}
	}

	private const float TIME_HIDE = 3f;

	public SpriteRenderer[] lineSprite;

	private LTDescr ltDescr;

	private float countdown;

	private bool isAutoHide;

	private SpriteRenderer lineFx;

	private Vector3 lineFxScale;

	private Vector3 vtRate;
}
