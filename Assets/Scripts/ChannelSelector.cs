using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChannelSelector : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	public void SetChannel(string channel)
	{
		this.Channel = channel;
		Text componentInChildren = base.GetComponentInChildren<Text>();
		componentInChildren.text = this.Channel;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		ChatGui chatGui = UnityEngine.Object.FindObjectOfType<ChatGui>();
		chatGui.ShowChannel(this.Channel);
	}

	public string Channel;
}
