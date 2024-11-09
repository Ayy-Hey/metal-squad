using System;
using UnityEngine;

public class SwipeInput : MonoBehaviour
{
	private void Start()
	{
		Input.multiTouchEnabled = true;
	}

	private void Update()
	{
		if (UnityEngine.Input.touchCount <= 0)
		{
			return;
		}
		foreach (Touch touch in Input.touches)
		{
			if (touch.phase == TouchPhase.Began)
			{
				this.initialTouch = touch;
			}
			else if (touch.phase == TouchPhase.Moved)
			{
				float num = touch.position.x - this.initialTouch.position.x;
				float num2 = touch.position.y - this.initialTouch.position.y;
				float num3 = Mathf.Abs(num) + Mathf.Abs(num2);
				if (num3 > this.minSwipeDistance && (Mathf.Abs(num) > 0f || Mathf.Abs(num2) > 0f))
				{
					this.swiping = true;
					this.CalculateSwipeDirection(num, num2);
				}
			}
			else if (touch.phase == TouchPhase.Ended)
			{
				this.initialTouch = default(Touch);
				this.swiping = false;
				this.direction = SwipeInput.SwipeDirection.None;
			}
			else if (touch.phase == TouchPhase.Canceled)
			{
				this.initialTouch = default(Touch);
				this.swiping = false;
				this.direction = SwipeInput.SwipeDirection.None;
			}
		}
	}

	private void CalculateSwipeDirection(float deltaX, float deltaY)
	{
		bool flag = Mathf.Abs(deltaX) > Mathf.Abs(deltaY);
		if (flag && Mathf.Abs(deltaY) <= this.errorRange)
		{
			if (deltaX > 0f)
			{
				this.direction = SwipeInput.SwipeDirection.Right;
			}
			else if (deltaX < 0f)
			{
				this.direction = SwipeInput.SwipeDirection.Left;
			}
		}
		else if (!flag && Mathf.Abs(deltaX) <= this.errorRange)
		{
			if (deltaY > 0f)
			{
				this.direction = SwipeInput.SwipeDirection.Up;
			}
			else if (deltaY < 0f)
			{
				this.direction = SwipeInput.SwipeDirection.Down;
			}
		}
		else
		{
			this.swiping = false;
		}
	}

	public bool swiping;

	public float minSwipeDistance;

	public float errorRange;

	public SwipeInput.SwipeDirection direction = SwipeInput.SwipeDirection.None;

	private Touch initialTouch;

	public enum SwipeDirection
	{
		Right,
		Left,
		Up,
		Down,
		None
	}
}
