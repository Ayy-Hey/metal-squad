using System;
using Boo.Lang.Runtime;
using UnityEngine;

[Serializable]
public class PathLocalCurveJS : MonoBehaviour
{
	public virtual void Start()
	{
		this.ltLogo = GameObject.Find("LeanTweenLogo");
		this.containingSphere = GameObject.Find("ContaingCube").transform;
		Vector3[] array = new Vector3[]
		{
			this.pt1.position,
			this.pt1.position,
			this.pt2.position,
			this.pt3.position,
			this.pt4.position,
			this.pt4.position
		};
		this.spline = new LTSpline(array);
		LeanTween.moveSplineLocal(this.ltLogo, array, 3f).setEase(LeanTweenType.easeInQuad).setOrientToPath(true).setRepeat(-1);
	}

	public virtual void Update()
	{
		float y = this.containingSphere.transform.eulerAngles.y + Time.deltaTime * 3f;
		Vector3 eulerAngles = this.containingSphere.transform.eulerAngles;
		float num = eulerAngles.y = y;
		Vector3 vector = this.containingSphere.transform.eulerAngles = eulerAngles;
	}

	public virtual void OnDrawGizmos()
	{
		if (!RuntimeServices.EqualityOperator(this.spline, null))
		{
			this.spline.gizmoDraw(1f);
		}
	}

	public virtual void Main()
	{
	}

	public AnimationCurve customAnimationCurve;

	public Transform pt1;

	public Transform pt2;

	public Transform pt3;

	public Transform pt4;

	private Transform containingSphere;

	private LTSpline spline;

	private GameObject ltLogo;
}
