using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Photon.Chat.UtilityScripts
{
	[RequireComponent(typeof(Text))]
	public class TextToggleIsOnTransition : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
	{
		public void OnEnable()
		{
			this._text = base.GetComponent<Text>();
			this.OnValueChanged(this.toggle.isOn);
			this.toggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnValueChanged));
		}

		public void OnDisable()
		{
			this.toggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.OnValueChanged));
		}

		public void OnValueChanged(bool isOn)
		{
			this._text.color = ((!isOn) ? ((!this.isHover) ? this.NormalOffColor : this.NormalOffColor) : ((!this.isHover) ? this.HoverOnColor : this.HoverOnColor));
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			this.isHover = true;
			this._text.color = ((!this.toggle.isOn) ? this.HoverOffColor : this.HoverOnColor);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			this.isHover = false;
			this._text.color = ((!this.toggle.isOn) ? this.NormalOffColor : this.NormalOnColor);
		}

		public Toggle toggle;

		private Text _text;

		public Color NormalOnColor = Color.white;

		public Color NormalOffColor = Color.black;

		public Color HoverOnColor = Color.black;

		public Color HoverOffColor = Color.black;

		private bool isHover;
	}
}
