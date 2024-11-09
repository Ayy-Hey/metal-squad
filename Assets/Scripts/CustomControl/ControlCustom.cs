using System;
using UnityEngine;

namespace CustomControl
{
	public class ControlCustom : MonoBehaviour
	{
		public void Show()
		{
			base.gameObject.SetActive(true);
		}

		public void Off()
		{
			this.Save();
			base.gameObject.SetActive(false);
		}

		public void Save()
		{
			for (int i = 0; i < this.customPosControls.Length; i++)
			{
				this.customPosControls[i].Save();
			}
		}

		public void ResetControl()
		{
			for (int i = 0; i < this.customPosControls.Length; i++)
			{
				this.customPosControls[i].ResetControl();
			}
		}

		[SerializeField]
		private CustomPosControl[] customPosControls;
	}
}
