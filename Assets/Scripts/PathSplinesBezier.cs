using System;
using UnityEngine;

public class PathSplinesBezier : MonoBehaviour
{
	private void OnEnable()
	{
		this.cr = new LTBezierPath(new Vector3[]
		{
			this.trans[0].position,
			this.trans[2].position,
			this.trans[1].position,
			this.trans[3].position,
			this.trans[3].position,
			this.trans[5].position,
			this.trans[4].position,
			this.trans[6].position
		});
	}

	private void Start()
	{
		this.avatar1 = GameObject.Find("Avatar1");
		LTDescr ltdescr = LeanTween.move(this.avatar1, this.cr.pts, 6.5f).setOrientToPath(true).setRepeat(-1);
		UnityEngine.Debug.Log("length of path 1:" + this.cr.length);
		UnityEngine.Debug.Log("length of path 2:" + ltdescr.path.length);
	}

	private void Update()
	{
		this.iter += Time.deltaTime * 0.07f;
		if (this.iter > 1f)
		{
			this.iter = 0f;
		}
	}

	private void OnDrawGizmos()
	{
		if (this.cr != null)
		{
			this.OnEnable();
		}
		Gizmos.color = Color.red;
		if (this.cr != null)
		{
			this.cr.gizmoDraw(-1f);
		}
	}

	public Transform[] trans;

	private LTBezierPath cr;

	private GameObject avatar1;

	private float iter;
}
