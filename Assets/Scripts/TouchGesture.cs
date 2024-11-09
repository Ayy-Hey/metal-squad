using System;
using System.Collections;
using UnityEngine;

public class TouchGesture
{
	public TouchGesture(TouchGesture.GestureSettings gestureSettings)
	{
		this.settings = gestureSettings;
	}

	public IEnumerator CheckHorizontalSwipes(Action onLeftSwipe, Action onRightSwipe)
	{
		for (;;)
		{
			foreach (Touch touch in Input.touches)
			{
				TouchPhase phase = touch.phase;
				if (phase != TouchPhase.Began)
				{
					if (phase != TouchPhase.Stationary)
					{
						if (phase == TouchPhase.Ended)
						{
							if (this.isASwipe(touch))
							{
								this.couldBeSwipe = false;
								if (Mathf.Sign(touch.position.x - this.startPos.x) == 1f)
								{
									onRightSwipe();
								}
								else
								{
									onLeftSwipe();
								}
							}
						}
					}
					else if (this.isContinouslyStationary(6))
					{
						this.couldBeSwipe = false;
					}
				}
				else
				{
					this.couldBeSwipe = true;
					this.startPos = touch.position;
					this.swipeStartTime = Time.time;
					this.stationaryForFrames = 0;
				}
				this.lastPhase = touch.phase;
			}
			yield return null;
		}
		yield break;
	}

	private bool isContinouslyStationary(int frames)
	{
		if (this.lastPhase == TouchPhase.Stationary)
		{
			this.stationaryForFrames++;
		}
		else
		{
			this.stationaryForFrames = 1;
		}
		return this.stationaryForFrames > frames;
	}

	private bool isASwipe(Touch touch)
	{
		float num = Time.time - this.swipeStartTime;
		float num2 = Mathf.Abs(touch.position.x - this.startPos.x);
		return this.couldBeSwipe && num < this.settings.maxSwipeTime && num2 > this.settings.minSwipeDist;
	}

	private TouchGesture.GestureSettings settings;

	private float swipeStartTime;

	private bool couldBeSwipe;

	private Vector2 startPos;

	private int stationaryForFrames;

	private TouchPhase lastPhase;

	[Serializable]
	public class GestureSettings
	{
		public float minSwipeDist = 100f;

		public float maxSwipeTime = 10f;
	}
}
