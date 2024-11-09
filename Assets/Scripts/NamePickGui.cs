using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ChatGui))]
public class NamePickGui : MonoBehaviour
{
	public void Start()
	{
		this.chatNewComponent = UnityEngine.Object.FindObjectOfType<ChatGui>();
		string @string = PlayerPrefs.GetString("NamePickUserName");
		if (!string.IsNullOrEmpty(@string))
		{
			this.idInput.text = @string;
		}
	}

	public void EndEditOnEnter()
	{
		if (UnityEngine.Input.GetKey(KeyCode.Return) || UnityEngine.Input.GetKey(KeyCode.KeypadEnter))
		{
			this.StartChat();
		}
	}

	public void StartChat()
	{
		ChatGui chatGui = UnityEngine.Object.FindObjectOfType<ChatGui>();
		chatGui.UserName = this.idInput.text.Trim();
		chatGui.Connect();
		base.enabled = false;
		PlayerPrefs.SetString("NamePickUserName", chatGui.UserName);
	}

	private const string UserNamePlayerPref = "NamePickUserName";

	public ChatGui chatNewComponent;

	public InputField idInput;
}
