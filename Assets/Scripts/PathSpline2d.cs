using System;
using UnityEngine;

public class PathSpline2d : MonoBehaviour
{
	private void Start()
	{
		this.cr = new LTSpline(new Vector3[]
		{
			this.trans[0].position,
			this.trans[1].position,
			this.trans[2].position,
			this.trans[3].position,
			this.trans[4].position
		});
		this.sprite1 = GameObject.Find("sprite1");
		this.sprite2 = GameObject.Find("sprite2");
		this.sprite1.AddComponent<SpriteRenderer>();
		this.sprite1.GetComponent<SpriteRenderer>().sprite = Sprite.Create(this.spriteTexture, new Rect(0f, 0f, 100f, 100f), new Vector2(50f, 50f), 10f);
		this.sprite2.AddComponent<SpriteRenderer>();
		this.sprite2.GetComponent<SpriteRenderer>().sprite = Sprite.Create(this.spriteTexture, new Rect(0f, 0f, 100f, 100f), new Vector2(0f, 0f), 10f);
		LTDescr ltdescr = LeanTween.moveSpline(this.sprite2, new Vector3[]
		{
			Vector3.zero,
			Vector3.zero,
			new Vector3(1f, 1f, 1f),
			new Vector3(2f, 1f, 1f),
			new Vector3(2f, 1f, 1f)
		}, 1.5f).setOrientToPath2d(true);
		ltdescr.setUseEstimatedTime(true);
	}

	private void Update()
	{
		this.cr.place2d(this.sprite1.transform, this.iter);
		this.iter += Time.deltaTime * 0.1f;
		if (this.iter > 1f)
		{
			this.iter = 0f;
		}
	}

	private void OnDrawGizmos()
	{
		if (this.cr != null)
		{
			this.cr.gizmoDraw(-1f);
		}
	}

	public Transform[] trans;

	public Texture2D spriteTexture;

	private LTSpline cr;

	private GameObject sprite1;

	private GameObject sprite2;

	private float iter;
}
