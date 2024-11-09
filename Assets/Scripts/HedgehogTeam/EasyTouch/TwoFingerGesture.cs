using System;
using UnityEngine;

namespace HedgehogTeam.EasyTouch
{
	public class TwoFingerGesture
	{
		public void ClearPickedObjectData()
		{
			this.pickedObject = null;
			this.oldPickedObject = null;
			this.pickedCamera = null;
			this.isGuiCamera = false;
		}

		public void ClearPickedUIData()
		{
			this.isOverGui = false;
			this.pickedUIElement = null;
		}

		public EasyTouch.GestureType currentGesture = EasyTouch.GestureType.None;

		public EasyTouch.GestureType oldGesture = EasyTouch.GestureType.None;

		public int finger0;

		public int finger1;

		public float startTimeAction;

		public float timeSinceStartAction;

		public Vector2 startPosition;

		public Vector2 position;

		public Vector2 deltaPosition;

		public Vector2 oldStartPosition;

		public float startDistance;

		public float fingerDistance;

		public float oldFingerDistance;

		public bool lockPinch;

		public bool lockTwist = true;

		public float lastPinch;

		public float lastTwistAngle;

		public GameObject pickedObject;

		public GameObject oldPickedObject;

		public Camera pickedCamera;

		public bool isGuiCamera;

		public bool isOverGui;

		public GameObject pickedUIElement;

		public bool dragStart;

		public bool swipeStart;

		public bool inSingleDoubleTaps;

		public float tapCurentTime;
	}
}
