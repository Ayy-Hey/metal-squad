using System;
using UnityEngine;
using UnityEngine.Events;

namespace HedgehogTeam.EasyTouch
{
	[AddComponentMenu("EasyTouch/Quick Drag")]
	public class QuickDrag : QuickBase
	{
		public QuickDrag()
		{
			this.quickActionName = "QuickDrag" + base.GetInstanceID().ToString();
			this.axesAction = QuickBase.AffectedAxesAction.XY;
		}

		public override void OnEnable()
		{
			base.OnEnable();
			EasyTouch.On_TouchStart += this.On_TouchStart;
			EasyTouch.On_TouchDown += this.On_TouchDown;
			EasyTouch.On_TouchUp += this.On_TouchUp;
			EasyTouch.On_Drag += this.On_Drag;
			EasyTouch.On_DragStart += this.On_DragStart;
			EasyTouch.On_DragEnd += this.On_DragEnd;
		}

		public override void OnDisable()
		{
			base.OnDisable();
			this.UnsubscribeEvent();
		}

		private void OnDestroy()
		{
			this.UnsubscribeEvent();
		}

		private void UnsubscribeEvent()
		{
			EasyTouch.On_TouchStart -= this.On_TouchStart;
			EasyTouch.On_TouchDown -= this.On_TouchDown;
			EasyTouch.On_TouchUp -= this.On_TouchUp;
			EasyTouch.On_Drag -= this.On_Drag;
			EasyTouch.On_DragStart -= this.On_DragStart;
			EasyTouch.On_DragEnd -= this.On_DragEnd;
		}

		private void OnCollisionEnter()
		{
			if (this.isStopOncollisionEnter && this.isOnDrag)
			{
				this.StopDrag();
			}
		}

		private void On_TouchStart(Gesture gesture)
		{
			if (this.realType == QuickBase.GameObjectType.UI && gesture.isOverGui && (gesture.pickedUIElement == base.gameObject || gesture.pickedUIElement.transform.IsChildOf(base.transform)) && this.fingerIndex == -1)
			{
				this.fingerIndex = gesture.fingerIndex;
				base.transform.SetAsLastSibling();
				this.onDragStart.Invoke(gesture);
				this.isOnDrag = true;
			}
		}

		private void On_TouchDown(Gesture gesture)
		{
			if (this.isOnDrag && this.fingerIndex == gesture.fingerIndex && this.realType == QuickBase.GameObjectType.UI && gesture.isOverGui && (gesture.pickedUIElement == base.gameObject || gesture.pickedUIElement.transform.IsChildOf(base.transform)))
			{
				base.transform.position += (Vector3)gesture.deltaPosition;
				if (gesture.deltaPosition != Vector2.zero)
				{
					this.onDrag.Invoke(gesture);
				}
				this.lastGesture = gesture;
			}
		}

		private void On_TouchUp(Gesture gesture)
		{
			if (this.fingerIndex == gesture.fingerIndex && this.realType == QuickBase.GameObjectType.UI)
			{
				this.lastGesture = gesture;
				this.StopDrag();
			}
		}

		private void On_DragStart(Gesture gesture)
		{
			if (this.realType != QuickBase.GameObjectType.UI && ((!this.enablePickOverUI && gesture.pickedUIElement == null) || this.enablePickOverUI) && gesture.pickedObject == base.gameObject && !this.isOnDrag)
			{
				this.isOnDrag = true;
				this.fingerIndex = gesture.fingerIndex;
				Vector3 touchToWorldPoint = gesture.GetTouchToWorldPoint(gesture.pickedObject.transform.position);
				this.deltaPosition = touchToWorldPoint - base.transform.position;
				if (this.resetPhysic)
				{
					if (this.cachedRigidBody)
					{
						this.cachedRigidBody.isKinematic = true;
					}
					if (this.cachedRigidBody2D)
					{
						this.cachedRigidBody2D.isKinematic = true;
					}
				}
				this.onDragStart.Invoke(gesture);
			}
		}

		private void On_Drag(Gesture gesture)
		{
			if (this.fingerIndex == gesture.fingerIndex && (this.realType == QuickBase.GameObjectType.Obj_2D || this.realType == QuickBase.GameObjectType.Obj_3D) && gesture.pickedObject == base.gameObject && this.fingerIndex == gesture.fingerIndex)
			{
				Vector3 position = gesture.GetTouchToWorldPoint(gesture.pickedObject.transform.position) - this.deltaPosition;
				base.transform.position = this.GetPositionAxes(position);
				if (gesture.deltaPosition != Vector2.zero)
				{
					this.onDrag.Invoke(gesture);
				}
				this.lastGesture = gesture;
			}
		}

		private void On_DragEnd(Gesture gesture)
		{
			if (this.fingerIndex == gesture.fingerIndex)
			{
				this.lastGesture = gesture;
				this.StopDrag();
			}
		}

		private Vector3 GetPositionAxes(Vector3 position)
		{
			Vector3 result = position;
			switch (this.axesAction)
			{
			case QuickBase.AffectedAxesAction.X:
				result = new Vector3(position.x, base.transform.position.y, base.transform.position.z);
				break;
			case QuickBase.AffectedAxesAction.Y:
				result = new Vector3(base.transform.position.x, position.y, base.transform.position.z);
				break;
			case QuickBase.AffectedAxesAction.Z:
				result = new Vector3(base.transform.position.x, base.transform.position.y, position.z);
				break;
			case QuickBase.AffectedAxesAction.XY:
				result = new Vector3(position.x, position.y, base.transform.position.z);
				break;
			case QuickBase.AffectedAxesAction.XZ:
				result = new Vector3(position.x, base.transform.position.y, position.z);
				break;
			case QuickBase.AffectedAxesAction.YZ:
				result = new Vector3(base.transform.position.x, position.y, position.z);
				break;
			}
			return result;
		}

		public void StopDrag()
		{
			this.fingerIndex = -1;
			if (this.resetPhysic)
			{
				if (this.cachedRigidBody)
				{
					this.cachedRigidBody.isKinematic = this.isKinematic;
				}
				if (this.cachedRigidBody2D)
				{
					this.cachedRigidBody2D.isKinematic = this.isKinematic2D;
				}
			}
			this.isOnDrag = false;
			this.onDragEnd.Invoke(this.lastGesture);
		}

		[SerializeField]
		public QuickDrag.OnDragStart onDragStart;

		[SerializeField]
		public QuickDrag.OnDrag onDrag;

		[SerializeField]
		public QuickDrag.OnDragEnd onDragEnd;

		public bool isStopOncollisionEnter;

		private Vector3 deltaPosition;

		private bool isOnDrag;

		private Gesture lastGesture;

		[Serializable]
		public class OnDragStart : UnityEvent<Gesture>
		{
		}

		[Serializable]
		public class OnDrag : UnityEvent<Gesture>
		{
		}

		[Serializable]
		public class OnDragEnd : UnityEvent<Gesture>
		{
		}
	}
}
