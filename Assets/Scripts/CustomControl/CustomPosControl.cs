using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CustomControl
{
	public class CustomPosControl : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
	{
		public void OnPointerDown(PointerEventData eventData)
		{
			for (int i = 0; i < this.objHighLights.Length; i++)
			{
				this.objHighLights[i].SetActive(true);
			}
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			for (int i = 0; i < this.objHighLights.Length; i++)
			{
				this.objHighLights[i].SetActive(false);
			}
		}

		public void OnDrag(PointerEventData eventData)
		{
			this.camPos = Camera.main.transform.position;
			this.mousePos = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
			this.mousePos.x = (this.mousePos.x + this.w - this.camPos.x) * this.deltaW + this.offset.x;
			this.mousePos.y = (this.mousePos.y + this.h - this.camPos.y) * this.deltaH + this.offset.y;
			this.mousePos.x = Mathf.Clamp(this.mousePos.x, this.spaceUI.z, this.spaceUI.w);
			this.mousePos.y = Mathf.Clamp(this.mousePos.y, this.spaceUI.y, this.spaceUI.x);
			StyleControl styleControl = this.style;
			if (styleControl != StyleControl.Button)
			{
				if (styleControl == StyleControl.Joystick)
				{
					this.customSpacing = this.ultJoystick.GetCustomSpacing(this.mousePos);
				}
			}
			else
			{
				this.customSpacing = this.ultButton.GetCustomSpacing(this.mousePos);
			}
			this.UpdateSpacing();
		}

		private void OnEnable()
		{
			this.h = Camera.main.orthographicSize;
			this.w = this.h * (float)Screen.width / (float)Screen.height;
			this.deltaH = (float)(Screen.height / 2) / this.h;
			this.deltaW = (float)(Screen.width / 2) / this.w;
			StyleControl styleControl = this.style;
			if (styleControl != StyleControl.Button)
			{
				if (styleControl == StyleControl.Joystick)
				{
					this.spaceUI = this.ultJoystick.GetSpaceJoystick();
				}
			}
			else
			{
				this.spaceUI = this.ultButton.GetSpaceButton();
			}
			this.LoadValue();
		}

		private void LoadValue()
		{
			this.customSpacing = ProfileManager.controlProfile.GetPosOption(this.control, this.option);
			base.StartCoroutine(this.UpdateSpacing(10));
		}

		private IEnumerator UpdateSpacing(int num)
		{
			while (num > 0)
			{
				this.UpdateSpacing();
				yield return null;
			}
			yield break;
		}

		public void UpdateSpacing()
		{
			StyleControl styleControl = this.style;
			if (styleControl != StyleControl.Button)
			{
				if (styleControl == StyleControl.Joystick)
				{
					this.ultJoystick.customSpacing_X = this.customSpacing.x;
					this.ultJoystick.customSpacing_Y = this.customSpacing.y;
					this.ultJoystick.UpdatePositioning();
				}
			}
			else
			{
				this.ultButton.customSpacing_X = this.customSpacing.x;
				this.ultButton.customSpacing_Y = this.customSpacing.y;
				this.ultButton.UpdatePositioning();
			}
			for (int i = 0; i < this.listsButtonFollow.Length; i++)
			{
				this.listsButtonFollow[i].UpdatePositioning();
			}
		}

		public void Save()
		{
			ProfileManager.controlProfile.SetOption(this.control, this.option, this.customSpacing);
		}

		public void ResetControl()
		{
			ProfileManager.controlProfile.Reset(this.control);
			this.LoadValue();
		}

		public EControl control;

		public EOptionControl option;

		public StyleControl style;

		[SerializeField]
		[Tooltip("không bỏ trống nếu style = Button")]
		private UltimateButton ultButton;

		[SerializeField]
		[Tooltip("không bỏ trống nếu style = Joystick")]
		private UltimateJoystick ultJoystick;

		[SerializeField]
		private UltimateButton[] listsButtonFollow;

		[SerializeField]
		private GameObject[] objHighLights;

		[SerializeField]
		private Vector2 offset;

		private float h;

		private float w;

		private float deltaH;

		private float deltaW;

		private Vector4 spaceUI;

		private Vector2 camPos;

		private Vector2 mousePos;

		private Vector2 customSpacing;
	}
}
