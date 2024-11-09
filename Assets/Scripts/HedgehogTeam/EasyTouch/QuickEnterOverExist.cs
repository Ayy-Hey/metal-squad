using System;
using UnityEngine;
using UnityEngine.Events;

namespace HedgehogTeam.EasyTouch
{
	[AddComponentMenu("EasyTouch/Quick Enter-Over-Exit")]
	public class QuickEnterOverExist : QuickBase
	{
		public QuickEnterOverExist()
		{
			this.quickActionName = "QuickEnterOverExit" + base.GetInstanceID().ToString();
		}

		private void Awake()
		{
			for (int i = 0; i < 100; i++)
			{
				this.fingerOver[i] = false;
			}
		}

		public override void OnEnable()
		{
			base.OnEnable();
			EasyTouch.On_TouchDown += this.On_TouchDown;
			EasyTouch.On_TouchUp += this.On_TouchUp;
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
			EasyTouch.On_TouchDown -= this.On_TouchDown;
			EasyTouch.On_TouchUp -= this.On_TouchUp;
		}

		private void On_TouchDown(Gesture gesture)
		{
			if (this.realType != QuickBase.GameObjectType.UI)
			{
				if ((!this.enablePickOverUI && gesture.GetCurrentFirstPickedUIElement(false) == null) || this.enablePickOverUI)
				{
					if (gesture.GetCurrentPickedObject(false) == base.gameObject)
					{
						if (!this.fingerOver[gesture.fingerIndex] && ((!this.isOnTouch && !this.isMultiTouch) || this.isMultiTouch))
						{
							this.fingerOver[gesture.fingerIndex] = true;
							this.onTouchEnter.Invoke(gesture);
							this.isOnTouch = true;
						}
						else if (this.fingerOver[gesture.fingerIndex])
						{
							this.onTouchOver.Invoke(gesture);
						}
					}
					else if (this.fingerOver[gesture.fingerIndex])
					{
						this.fingerOver[gesture.fingerIndex] = false;
						this.onTouchExit.Invoke(gesture);
						if (!this.isMultiTouch)
						{
							this.isOnTouch = false;
						}
					}
				}
				else if (gesture.GetCurrentPickedObject(false) == base.gameObject && !this.enablePickOverUI && gesture.GetCurrentFirstPickedUIElement(false) != null && this.fingerOver[gesture.fingerIndex])
				{
					this.fingerOver[gesture.fingerIndex] = false;
					this.onTouchExit.Invoke(gesture);
					if (!this.isMultiTouch)
					{
						this.isOnTouch = false;
					}
				}
			}
			else if (gesture.GetCurrentFirstPickedUIElement(false) == base.gameObject)
			{
				if (!this.fingerOver[gesture.fingerIndex] && ((!this.isOnTouch && !this.isMultiTouch) || this.isMultiTouch))
				{
					this.fingerOver[gesture.fingerIndex] = true;
					this.onTouchEnter.Invoke(gesture);
					this.isOnTouch = true;
				}
				else if (this.fingerOver[gesture.fingerIndex])
				{
					this.onTouchOver.Invoke(gesture);
				}
			}
			else if (this.fingerOver[gesture.fingerIndex])
			{
				this.fingerOver[gesture.fingerIndex] = false;
				this.onTouchExit.Invoke(gesture);
				if (!this.isMultiTouch)
				{
					this.isOnTouch = false;
				}
			}
		}

		private void On_TouchUp(Gesture gesture)
		{
			if (this.fingerOver[gesture.fingerIndex])
			{
				this.fingerOver[gesture.fingerIndex] = false;
				this.onTouchExit.Invoke(gesture);
				if (!this.isMultiTouch)
				{
					this.isOnTouch = false;
				}
			}
		}

		[SerializeField]
		public QuickEnterOverExist.OnTouchEnter onTouchEnter;

		[SerializeField]
		public QuickEnterOverExist.OnTouchOver onTouchOver;

		[SerializeField]
		public QuickEnterOverExist.OnTouchExit onTouchExit;

		private bool[] fingerOver = new bool[100];

		[Serializable]
		public class OnTouchEnter : UnityEvent<Gesture>
		{
		}

		[Serializable]
		public class OnTouchOver : UnityEvent<Gesture>
		{
		}

		[Serializable]
		public class OnTouchExit : UnityEvent<Gesture>
		{
		}
	}
}
