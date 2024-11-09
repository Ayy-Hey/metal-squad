using System;
using UnityEngine;

namespace CustomControl
{
	public class PopupCustomControl : MonoBehaviour
	{
		public void Show(int idControl, Action callback = null)
		{
			this.callback = callback;
			this.idControl = idControl;
			base.gameObject.SetActive(true);
			this.controlCustoms[idControl].Show();
		}

		public void Off()
		{
			this.controlCustoms[this.idControl].Off();
			UnityEngine.Object.Destroy(base.gameObject);
			if (this.callback != null)
			{
				this.callback();
			}
		}

		public void ResetControl()
		{
			this.controlCustoms[this.idControl].ResetControl();
		}

		[SerializeField]
		private ControlCustom[] controlCustoms;

		private Action callback;

		private int idControl;
	}
}
