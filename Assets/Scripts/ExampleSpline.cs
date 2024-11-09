using System;
using UnityEngine;

public class ExampleSpline : MonoBehaviour
{
	private void Start()
	{
		this.spline = new LTSpline(new Vector3[]
		{
			this.trans[0].position,
			this.trans[1].position,
			this.trans[2].position,
			this.trans[3].position,
			this.trans[4].position
		});
		this.ltLogo = GameObject.Find("LeanTweenLogo1");
		this.ltLogo2 = GameObject.Find("LeanTweenLogo2");
		LeanTween.moveSpline(this.ltLogo2, this.spline.pts, 1f).setEase(LeanTweenType.easeInOutQuad).setLoopPingPong().setOrientToPath(true);
		LTDescr ltdescr = LeanTween.moveSpline(this.ltLogo2, new Vector3[]
		{
			Vector3.zero,
			Vector3.zero,
			new Vector3(1f, 1f, 1f),
			new Vector3(2f, 1f, 1f),
			new Vector3(2f, 1f, 1f)
		}, 1.5f);
		ltdescr.setUseEstimatedTime(true);
	}

	private void Update()
	{
		this.ltLogo.transform.position = this.spline.point(this.iter);
		this.iter += Time.deltaTime * 0.1f;
		if (this.iter > 1f)
		{
			this.iter = 0f;
		}
	}

	private void OnDrawGizmos()
	{
		if (this.spline != null)
		{
			this.spline.gizmoDraw(-1f);
		}
	}

	public Transform[] trans;

	private LTSpline spline;

	private GameObject ltLogo;

	private GameObject ltLogo2;

	private float iter;
}
