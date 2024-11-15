using System;
using UnityEngine;

public class SplineWalker : MonoBehaviour
{
	private void Update()
	{
		if (this.goingForward)
		{
			this.progress += Time.deltaTime / this.duration;
			if (this.progress > 1f)
			{
				if (this.mode == SplineWalkerMode.Once)
				{
					this.progress = 1f;
				}
				else if (this.mode == SplineWalkerMode.Loop)
				{
					this.progress -= 1f;
				}
				else
				{
					this.progress = 2f - this.progress;
					this.goingForward = false;
				}
			}
		}
		else
		{
			this.progress -= Time.deltaTime / this.duration;
			if (this.progress < 0f)
			{
				this.progress = -this.progress;
				this.goingForward = true;
			}
		}
		Vector3 point = this.spline.GetPoint(this.progress);
		base.transform.localPosition = point;
		if (this.lookForward)
		{
			base.transform.LookAt(point + this.spline.GetDirection(this.progress));
		}
	}

	public BezierSpline spline;

	public float duration;

	public bool lookForward;

	public SplineWalkerMode mode;

	private float progress;

	private bool goingForward = true;
}
