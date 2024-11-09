using System;
using UnityEngine;

namespace HedgehogTeam.EasyTouch
{
	public class EasyTouchInput
	{
		public int TouchCount()
		{
			return this.getTouchCount(true);
		}

		private int getTouchCount(bool realTouch)
		{
			int num = 0;
			if (realTouch || EasyTouch.instance.enableRemote)
			{
				num = UnityEngine.Input.touchCount;
			}
			else if (Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
			{
				num = 1;
				if (EasyTouch.GetSecondeFingerSimulation())
				{
					if (UnityEngine.Input.GetKey(KeyCode.LeftAlt) || UnityEngine.Input.GetKey(EasyTouch.instance.twistKey) || UnityEngine.Input.GetKey(KeyCode.LeftControl) || UnityEngine.Input.GetKey(EasyTouch.instance.swipeKey))
					{
						num = 2;
					}
					if (UnityEngine.Input.GetKeyUp(KeyCode.LeftAlt) || UnityEngine.Input.GetKeyUp(EasyTouch.instance.twistKey) || UnityEngine.Input.GetKeyUp(KeyCode.LeftControl) || UnityEngine.Input.GetKeyUp(EasyTouch.instance.swipeKey))
					{
						num = 2;
					}
				}
				if (num == 0)
				{
					this.complexCenter = Vector2.zero;
					this.oldMousePosition[0] = new Vector2(-1f, -1f);
					this.oldMousePosition[1] = new Vector2(-1f, -1f);
				}
			}
			return num;
		}

		public Finger GetMouseTouch(int fingerIndex, Finger myFinger)
		{
			Finger finger;
			if (myFinger != null)
			{
				finger = myFinger;
			}
			else
			{
				finger = new Finger();
				finger.gesture = EasyTouch.GestureType.None;
			}
			if (fingerIndex == 1 && (UnityEngine.Input.GetKeyUp(KeyCode.LeftAlt) || UnityEngine.Input.GetKeyUp(EasyTouch.instance.twistKey) || UnityEngine.Input.GetKeyUp(KeyCode.LeftControl) || UnityEngine.Input.GetKeyUp(EasyTouch.instance.swipeKey)))
			{
				finger.fingerIndex = fingerIndex;
				finger.position = this.oldFinger2Position;
				finger.deltaPosition = finger.position - this.oldFinger2Position;
				finger.tapCount = this.tapCount[fingerIndex];
				finger.deltaTime = Time.realtimeSinceStartup - this.deltaTime[fingerIndex];
				finger.phase = TouchPhase.Ended;
				return finger;
			}
			if (Input.GetMouseButton(0))
			{
				finger.fingerIndex = fingerIndex;
				finger.position = this.GetPointerPosition(fingerIndex);
				if ((double)(Time.realtimeSinceStartup - this.tapeTime[fingerIndex]) > 0.5)
				{
					this.tapCount[fingerIndex] = 0;
				}
				if (Input.GetMouseButtonDown(0) || (fingerIndex == 1 && (UnityEngine.Input.GetKeyDown(KeyCode.LeftAlt) || UnityEngine.Input.GetKeyDown(EasyTouch.instance.twistKey) || UnityEngine.Input.GetKeyDown(KeyCode.LeftControl) || UnityEngine.Input.GetKeyDown(EasyTouch.instance.swipeKey))))
				{
					finger.position = this.GetPointerPosition(fingerIndex);
					finger.deltaPosition = Vector2.zero;
					this.tapCount[fingerIndex] = this.tapCount[fingerIndex] + 1;
					finger.tapCount = this.tapCount[fingerIndex];
					this.startActionTime[fingerIndex] = Time.realtimeSinceStartup;
					this.deltaTime[fingerIndex] = this.startActionTime[fingerIndex];
					finger.deltaTime = 0f;
					finger.phase = TouchPhase.Began;
					if (fingerIndex == 1)
					{
						this.oldFinger2Position = finger.position;
						this.oldMousePosition[fingerIndex] = finger.position;
					}
					else
					{
						this.oldMousePosition[fingerIndex] = finger.position;
					}
					if (this.tapCount[fingerIndex] == 1)
					{
						this.tapeTime[fingerIndex] = Time.realtimeSinceStartup;
					}
					return finger;
				}
				finger.deltaPosition = finger.position - this.oldMousePosition[fingerIndex];
				finger.tapCount = this.tapCount[fingerIndex];
				finger.deltaTime = Time.realtimeSinceStartup - this.deltaTime[fingerIndex];
				if (finger.deltaPosition.sqrMagnitude < 1f)
				{
					finger.phase = TouchPhase.Stationary;
				}
				else
				{
					finger.phase = TouchPhase.Moved;
				}
				this.oldMousePosition[fingerIndex] = finger.position;
				this.deltaTime[fingerIndex] = Time.realtimeSinceStartup;
				return finger;
			}
			else
			{
				if (Input.GetMouseButtonUp(0))
				{
					finger.fingerIndex = fingerIndex;
					finger.position = this.GetPointerPosition(fingerIndex);
					finger.deltaPosition = finger.position - this.oldMousePosition[fingerIndex];
					finger.tapCount = this.tapCount[fingerIndex];
					finger.deltaTime = Time.realtimeSinceStartup - this.deltaTime[fingerIndex];
					finger.phase = TouchPhase.Ended;
					this.oldMousePosition[fingerIndex] = finger.position;
					return finger;
				}
				return null;
			}
		}

		public Vector2 GetSecondFingerPosition()
		{
			Vector2 result = new Vector2(-1f, -1f);
			if ((UnityEngine.Input.GetKey(KeyCode.LeftAlt) || UnityEngine.Input.GetKey(EasyTouch.instance.twistKey)) && (UnityEngine.Input.GetKey(KeyCode.LeftControl) || UnityEngine.Input.GetKey(EasyTouch.instance.swipeKey)))
			{
				if (!this.bComplex)
				{
					this.bComplex = true;
					this.deltaFingerPosition = (Vector2)UnityEngine.Input.mousePosition - this.oldFinger2Position;
				}
				result = this.GetComplex2finger();
				return result;
			}
			if (UnityEngine.Input.GetKey(KeyCode.LeftAlt) || UnityEngine.Input.GetKey(EasyTouch.instance.twistKey))
			{
				result = this.GetPinchTwist2Finger(false);
				this.bComplex = false;
				return result;
			}
			if (UnityEngine.Input.GetKey(KeyCode.LeftControl) || UnityEngine.Input.GetKey(EasyTouch.instance.swipeKey))
			{
				result = this.GetComplex2finger();
				this.bComplex = false;
				return result;
			}
			return result;
		}

		private Vector2 GetPointerPosition(int index)
		{
			if (index == 0)
			{
				return UnityEngine.Input.mousePosition;
			}
			return this.GetSecondFingerPosition();
		}

		private Vector2 GetPinchTwist2Finger(bool newSim = false)
		{
			Vector2 result;
			if (this.complexCenter == Vector2.zero)
			{
				result.x = (float)Screen.width / 2f - (UnityEngine.Input.mousePosition.x - (float)Screen.width / 2f);
				result.y = (float)Screen.height / 2f - (UnityEngine.Input.mousePosition.y - (float)Screen.height / 2f);
			}
			else
			{
				result.x = this.complexCenter.x - (UnityEngine.Input.mousePosition.x - this.complexCenter.x);
				result.y = this.complexCenter.y - (UnityEngine.Input.mousePosition.y - this.complexCenter.y);
			}
			this.oldFinger2Position = result;
			return result;
		}

		private Vector2 GetComplex2finger()
		{
			Vector2 result;
			result.x = UnityEngine.Input.mousePosition.x - this.deltaFingerPosition.x;
			result.y = UnityEngine.Input.mousePosition.y - this.deltaFingerPosition.y;
			this.complexCenter = new Vector2((UnityEngine.Input.mousePosition.x + result.x) / 2f, (UnityEngine.Input.mousePosition.y + result.y) / 2f);
			this.oldFinger2Position = result;
			return result;
		}

		private Vector2[] oldMousePosition = new Vector2[2];

		private int[] tapCount = new int[2];

		private float[] startActionTime = new float[2];

		private float[] deltaTime = new float[2];

		private float[] tapeTime = new float[2];

		private bool bComplex;

		private Vector2 deltaFingerPosition;

		private Vector2 oldFinger2Position;

		private Vector2 complexCenter;
	}
}
