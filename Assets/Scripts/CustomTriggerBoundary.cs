using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class CustomTriggerBoundary : MonoBehaviour
{
	private void OnValidate()
	{
		if (!this.box)
		{
			this.box = base.GetComponent<BoxCollider2D>();
			this.box.isTrigger = true;
		}
		if (base.gameObject.layer != 0)
		{
			base.gameObject.layer = 0;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Rambo"))
		{
			if (this.useMid)
			{
				Vector3 position = base.transform.position;
				if (this.useTop)
				{
					base.StartCoroutine(this.ChangeBoundaryTop(this.top + position.y));
				}
				if (this.useBottom)
				{
					base.StartCoroutine(this.ChangeBoundaryBottom(this.bottom + position.y));
				}
				if (this.useLeft)
				{
					base.StartCoroutine(this.ChangeBoundaryLeft(this.left + position.x));
				}
				if (this.useRight)
				{
					base.StartCoroutine(this.ChangeBoundaryRight(this.right + position.x));
				}
			}
			else
			{
				if (this.useTop)
				{
					base.StartCoroutine(this.ChangeBoundaryTop(this.top));
				}
				if (this.useBottom)
				{
					base.StartCoroutine(this.ChangeBoundaryBottom(this.bottom));
				}
				if (this.useLeft)
				{
					base.StartCoroutine(this.ChangeBoundaryLeft(this.left));
				}
				if (this.useRight)
				{
					base.StartCoroutine(this.ChangeBoundaryRight(this.right));
				}
			}
			this.OnTriggerEvent.Invoke();
		}
	}

	private IEnumerator ChangeBoundaryTop(float value)
	{
		float velo = 0f;
		while (CameraController.Instance.NumericBoundaries.TopBoundary != value)
		{
			CameraController.Instance.NumericBoundaries.TopBoundary = Mathf.SmoothDamp(CameraController.Instance.NumericBoundaries.TopBoundary, value, ref velo, 0.3f);
			if (Mathf.Abs(CameraController.Instance.NumericBoundaries.TopBoundary - value) <= 0.01f)
			{
				CameraController.Instance.NumericBoundaries.TopBoundary = value;
			}
			yield return 0;
		}
		yield break;
	}

	private IEnumerator ChangeBoundaryBottom(float value)
	{
		float velo = 0f;
		while (CameraController.Instance.NumericBoundaries.BottomBoundary != value)
		{
			CameraController.Instance.NumericBoundaries.BottomBoundary = Mathf.SmoothDamp(CameraController.Instance.NumericBoundaries.BottomBoundary, value, ref velo, 0.3f);
			if (Mathf.Abs(CameraController.Instance.NumericBoundaries.BottomBoundary - value) <= 0.01f)
			{
				CameraController.Instance.NumericBoundaries.BottomBoundary = value;
			}
			yield return 0;
		}
		yield break;
	}

	private IEnumerator ChangeBoundaryLeft(float value)
	{
		float velo = 0f;
		while (CameraController.Instance.NumericBoundaries.LeftBoundary != value)
		{
			CameraController.Instance.NumericBoundaries.LeftBoundary = Mathf.SmoothDamp(CameraController.Instance.NumericBoundaries.LeftBoundary, value, ref velo, 0.3f);
			if (Mathf.Abs(CameraController.Instance.NumericBoundaries.LeftBoundary - value) <= 0.01f)
			{
				CameraController.Instance.NumericBoundaries.LeftBoundary = value;
			}
			yield return 0;
		}
		yield break;
	}

	private IEnumerator ChangeBoundaryRight(float value)
	{
		float velo = 0f;
		while (CameraController.Instance.NumericBoundaries.RightBoundary != value)
		{
			CameraController.Instance.NumericBoundaries.RightBoundary = Mathf.SmoothDamp(CameraController.Instance.NumericBoundaries.RightBoundary, value, ref velo, 0.3f);
			if (Mathf.Abs(CameraController.Instance.NumericBoundaries.RightBoundary - value) <= 0.01f)
			{
				CameraController.Instance.NumericBoundaries.RightBoundary = value;
			}
			yield return 0;
		}
		yield break;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.blue;
		if (this.useMid)
		{
			Vector3 position = base.transform.position;
			if (this.useTop)
			{
				this.start.x = position.x - 3f;
				this.start.y = (this.end.y = position.y + this.top);
				this.end.x = position.x + 3f;
				Gizmos.DrawLine(this.start, this.end);
			}
			if (this.useBottom)
			{
				this.start.x = position.x - 3f;
				this.start.y = (this.end.y = position.y + this.bottom);
				this.end.x = position.x + 3f;
				Gizmos.DrawLine(this.start, this.end);
			}
			if (this.useLeft)
			{
				this.start.x = (this.end.x = position.x + this.left);
				this.start.y = position.y + 3f;
				this.end.y = position.y - 3f;
				Gizmos.DrawLine(this.start, this.end);
			}
			if (this.useRight)
			{
				this.start.x = (this.end.x = position.x + this.right);
				this.start.y = position.y + 3f;
				this.end.y = position.y - 3f;
				Gizmos.DrawLine(this.start, this.end);
			}
		}
		else
		{
			if (this.useTop)
			{
				this.start.x = 0f;
				this.start.y = (this.end.y = this.top);
				this.end.x = 200f;
				Gizmos.DrawLine(this.start, this.end);
			}
			if (this.useBottom)
			{
				this.start.x = 0f;
				this.start.y = (this.end.y = this.bottom);
				this.end.x = 200f;
				Gizmos.DrawLine(this.start, this.end);
			}
			if (this.useLeft)
			{
				this.start.x = (this.end.x = this.left);
				this.start.y = -20f;
				this.end.y = 20f;
				Gizmos.DrawLine(this.start, this.end);
			}
			if (this.useRight)
			{
				this.start.x = (this.end.x = this.right);
				this.start.y = -20f;
				this.end.y = 20f;
				Gizmos.DrawLine(this.start, this.end);
			}
		}
	}

	public bool useTop;

	public float top;

	public bool useBottom;

	public float bottom;

	public bool useLeft;

	public float left;

	public bool useRight;

	public float right;

	public bool useMid = true;

	public BoxCollider2D box;

	public UnityEvent OnTriggerEvent;

	private Vector3 start;

	private Vector3 end;
}
