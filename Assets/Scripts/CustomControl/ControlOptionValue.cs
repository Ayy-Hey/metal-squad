using System;
using UnityEngine;

namespace CustomControl
{
	public class ControlOptionValue : MonoBehaviour
	{
		private void OnEnable()
		{
			Vector2 posOption = ProfileManager.controlProfile.GetPosOption(this.control, this.option);
			StyleControl styleControl = this.style;
			if (styleControl != StyleControl.Button)
			{
				if (styleControl == StyleControl.Joystick)
				{
					this.ultJoystick.customSpacing_X = posOption.x;
					this.ultJoystick.customSpacing_Y = posOption.y;
				}
			}
			else
			{
				this.ultButton.customSpacing_X = posOption.x;
				this.ultButton.customSpacing_Y = posOption.y;
			}
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
	}
}
