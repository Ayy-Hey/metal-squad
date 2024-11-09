using System;
using UnityEngine;

namespace HedgehogTeam.EasyTouch
{
	public class QuickBase : MonoBehaviour
	{
		private void Awake()
		{
			this.cachedRigidBody = base.GetComponent<Rigidbody>();
			if (this.cachedRigidBody)
			{
				this.isKinematic = this.cachedRigidBody.isKinematic;
			}
			this.cachedRigidBody2D = base.GetComponent<Rigidbody2D>();
			if (this.cachedRigidBody2D)
			{
				this.isKinematic2D = this.cachedRigidBody2D.isKinematic;
			}
		}

		public virtual void Start()
		{
			EasyTouch.SetEnableAutoSelect(true);
			this.realType = QuickBase.GameObjectType.Obj_3D;
			if (base.GetComponent<Collider>())
			{
				this.realType = QuickBase.GameObjectType.Obj_3D;
			}
			else if (base.GetComponent<Collider2D>())
			{
				this.realType = QuickBase.GameObjectType.Obj_2D;
			}
			else if (base.GetComponent<CanvasRenderer>())
			{
				this.realType = QuickBase.GameObjectType.UI;
			}
			QuickBase.GameObjectType gameObjectType = this.realType;
			if (gameObjectType != QuickBase.GameObjectType.Obj_3D)
			{
				if (gameObjectType != QuickBase.GameObjectType.Obj_2D)
				{
					if (gameObjectType == QuickBase.GameObjectType.UI)
					{
						EasyTouch.instance.enableUIMode = true;
						EasyTouch.SetUICompatibily(false);
					}
				}
				else
				{
					EasyTouch.SetEnable2DCollider(true);
					LayerMask mask = EasyTouch.Get2DPickableLayer();
					mask |= 1 << base.gameObject.layer;
					EasyTouch.Set2DPickableLayer(mask);
				}
			}
			else
			{
				LayerMask mask = EasyTouch.Get3DPickableLayer();
				mask |= 1 << base.gameObject.layer;
				EasyTouch.Set3DPickableLayer(mask);
			}
			if (this.enablePickOverUI)
			{
				EasyTouch.instance.enableUIMode = true;
				EasyTouch.SetUICompatibily(false);
			}
		}

		public virtual void OnEnable()
		{
		}

		public virtual void OnDisable()
		{
		}

		protected Vector3 GetInfluencedAxis()
		{
			Vector3 zero = Vector3.zero;
			switch (this.axesAction)
			{
			case QuickBase.AffectedAxesAction.X:
				zero = new Vector3(1f, 0f, 0f);
				break;
			case QuickBase.AffectedAxesAction.Y:
				zero = new Vector3(0f, 1f, 0f);
				break;
			case QuickBase.AffectedAxesAction.Z:
				zero = new Vector3(0f, 0f, 1f);
				break;
			case QuickBase.AffectedAxesAction.XY:
				zero = new Vector3(1f, 1f, 0f);
				break;
			case QuickBase.AffectedAxesAction.XZ:
				zero = new Vector3(1f, 0f, 1f);
				break;
			case QuickBase.AffectedAxesAction.YZ:
				zero = new Vector3(0f, 1f, 1f);
				break;
			case QuickBase.AffectedAxesAction.XYZ:
				zero = new Vector3(1f, 1f, 1f);
				break;
			}
			return zero;
		}

		protected void DoDirectAction(float value)
		{
			Vector3 influencedAxis = this.GetInfluencedAxis();
			switch (this.directAction)
			{
			case QuickBase.DirectAction.Rotate:
				base.transform.Rotate(influencedAxis * value, Space.World);
				break;
			case QuickBase.DirectAction.RotateLocal:
				base.transform.Rotate(influencedAxis * value, Space.Self);
				break;
			case QuickBase.DirectAction.Translate:
				if (this.directCharacterController == null)
				{
					base.transform.Translate(influencedAxis * value, Space.World);
				}
				else
				{
					Vector3 motion = influencedAxis * value;
					this.directCharacterController.Move(motion);
				}
				break;
			case QuickBase.DirectAction.TranslateLocal:
				if (this.directCharacterController == null)
				{
					base.transform.Translate(influencedAxis * value, Space.Self);
				}
				else
				{
					Vector3 motion2 = this.directCharacterController.transform.TransformDirection(influencedAxis) * value;
					this.directCharacterController.Move(motion2);
				}
				break;
			case QuickBase.DirectAction.Scale:
				base.transform.localScale += influencedAxis * value;
				break;
			}
		}

		public void EnabledQuickComponent(string quickActionName)
		{
			QuickBase[] components = base.GetComponents<QuickBase>();
			foreach (QuickBase quickBase in components)
			{
				if (quickBase.quickActionName == quickActionName)
				{
					quickBase.enabled = true;
				}
			}
		}

		public void DisabledQuickComponent(string quickActionName)
		{
			QuickBase[] components = base.GetComponents<QuickBase>();
			foreach (QuickBase quickBase in components)
			{
				if (quickBase.quickActionName == quickActionName)
				{
					quickBase.enabled = false;
				}
			}
		}

		public void DisabledAllSwipeExcepted(string quickActionName)
		{
			QuickSwipe[] array = UnityEngine.Object.FindObjectsOfType(typeof(QuickSwipe)) as QuickSwipe[];
			foreach (QuickSwipe quickSwipe in array)
			{
				if (quickSwipe.quickActionName != quickActionName || (quickSwipe.quickActionName == quickActionName && quickSwipe.gameObject != base.gameObject))
				{
					quickSwipe.enabled = false;
				}
			}
		}

		public string quickActionName;

		public bool isMultiTouch;

		public bool is2Finger;

		public bool isOnTouch;

		public bool enablePickOverUI;

		public bool resetPhysic;

		public QuickBase.DirectAction directAction;

		public QuickBase.AffectedAxesAction axesAction;

		public float sensibility = 1f;

		public CharacterController directCharacterController;

		public bool inverseAxisValue;

		protected Rigidbody cachedRigidBody;

		protected bool isKinematic;

		protected Rigidbody2D cachedRigidBody2D;

		protected bool isKinematic2D;

		protected QuickBase.GameObjectType realType;

		protected int fingerIndex = -1;

		protected enum GameObjectType
		{
			Auto,
			Obj_3D,
			Obj_2D,
			UI
		}

		public enum DirectAction
		{
			None,
			Rotate,
			RotateLocal,
			Translate,
			TranslateLocal,
			Scale
		}

		public enum AffectedAxesAction
		{
			X,
			Y,
			Z,
			XY,
			XZ,
			YZ,
			XYZ
		}
	}
}
