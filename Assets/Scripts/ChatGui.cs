using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class ChatGui : MonoBehaviour, IChatClientListener
{
	public string UserName { get; set; }

	public void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.UserIdText.text = string.Empty;
		this.StateText.text = string.Empty;
		this.StateText.gameObject.SetActive(true);
		this.UserIdText.gameObject.SetActive(true);
		this.Title.SetActive(true);
		this.ChatPanel.gameObject.SetActive(false);
		this.ConnectingLabel.SetActive(false);
		if (string.IsNullOrEmpty(this.UserName))
		{
			this.UserName = "user" + Environment.TickCount % 99;
		}
		this.chatAppSettings = PhotonNetwork.PhotonServerSettings.AppSettings;
		bool flag = !string.IsNullOrEmpty(this.chatAppSettings.AppIdChat);
		this.missingAppIdErrorPanel.SetActive(!flag);
		this.UserIdFormPanel.gameObject.SetActive(flag);
		if (!flag)
		{
			UnityEngine.Debug.LogError("You need to set the chat app ID in the PhotonServerSettings file in order to continue.");
		}
	}

	public void Connect()
	{
		this.UserIdFormPanel.gameObject.SetActive(false);
		this.chatClient = new ChatClient(this, ConnectionProtocol.Udp);
		this.chatClient.UseBackgroundWorkerForSending = true;
		this.chatClient.Connect(this.chatAppSettings.AppIdChat, "1.0", new Photon.Chat.AuthenticationValues(this.UserName));
		this.ChannelToggleToInstantiate.gameObject.SetActive(false);
		UnityEngine.Debug.Log("Connecting as: " + this.UserName);
		this.ConnectingLabel.SetActive(true);
	}

	public void OnDestroy()
	{
		if (this.chatClient != null)
		{
			this.chatClient.Disconnect();
		}
	}

	public void OnApplicationQuit()
	{
		if (this.chatClient != null)
		{
			this.chatClient.Disconnect();
		}
	}

	public void Update()
	{
		if (this.chatClient != null)
		{
			this.chatClient.Service();
		}
		if (this.StateText == null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		this.StateText.gameObject.SetActive(this.ShowState);
	}

	public void OnEnterSend()
	{
		if (UnityEngine.Input.GetKey(KeyCode.Return) || UnityEngine.Input.GetKey(KeyCode.KeypadEnter))
		{
			this.SendChatMessage(this.InputFieldChat.text);
			this.InputFieldChat.text = string.Empty;
		}
	}

	public void OnClickSend()
	{
		if (this.InputFieldChat != null)
		{
			this.SendChatMessage(this.InputFieldChat.text);
			this.InputFieldChat.text = string.Empty;
		}
	}

	private void SendChatMessage(string inputLine)
	{
		if (string.IsNullOrEmpty(inputLine))
		{
			return;
		}
		if ("test".Equals(inputLine))
		{
			if (this.TestLength != this.testBytes.Length)
			{
				this.testBytes = new byte[this.TestLength];
			}
			this.chatClient.SendPrivateMessage(this.chatClient.AuthValues.UserId, this.testBytes, true);
		}
		bool flag = this.chatClient.PrivateChannels.ContainsKey(this.selectedChannelName);
		string target = string.Empty;
		if (flag)
		{
			string[] array = this.selectedChannelName.Split(new char[]
			{
				':'
			});
			target = array[1];
		}
		if (inputLine[0].Equals('\\'))
		{
			string[] array2 = inputLine.Split(new char[]
			{
				' '
			}, 2);
			if (array2[0].Equals("\\help"))
			{
				this.PostHelpToCurrentChannel();
			}
			if (array2[0].Equals("\\state"))
			{
				int num = 0;
				List<string> list = new List<string>();
				list.Add("i am state " + num);
				string[] array3 = array2[1].Split(new char[]
				{
					' ',
					','
				});
				if (array3.Length > 0)
				{
					num = int.Parse(array3[0]);
				}
				if (array3.Length > 1)
				{
					list.Add(array3[1]);
				}
				this.chatClient.SetOnlineStatus(num, list.ToArray());
			}
			else if ((array2[0].Equals("\\subscribe") || array2[0].Equals("\\s")) && !string.IsNullOrEmpty(array2[1]))
			{
				this.chatClient.Subscribe(array2[1].Split(new char[]
				{
					' ',
					','
				}));
			}
			else if ((array2[0].Equals("\\unsubscribe") || array2[0].Equals("\\u")) && !string.IsNullOrEmpty(array2[1]))
			{
				this.chatClient.Unsubscribe(array2[1].Split(new char[]
				{
					' ',
					','
				}));
			}
			else if (array2[0].Equals("\\clear"))
			{
				ChatChannel chatChannel;
				if (flag)
				{
					this.chatClient.PrivateChannels.Remove(this.selectedChannelName);
				}
				else if (this.chatClient.TryGetChannel(this.selectedChannelName, flag, out chatChannel))
				{
					chatChannel.ClearMessages();
				}
			}
			else if (array2[0].Equals("\\msg") && !string.IsNullOrEmpty(array2[1]))
			{
				string[] array4 = array2[1].Split(new char[]
				{
					' ',
					','
				}, 2);
				if (array4.Length < 2)
				{
					return;
				}
				string target2 = array4[0];
				string message = array4[1];
				this.chatClient.SendPrivateMessage(target2, message, false);
			}
			else if ((array2[0].Equals("\\join") || array2[0].Equals("\\j")) && !string.IsNullOrEmpty(array2[1]))
			{
				string[] array5 = array2[1].Split(new char[]
				{
					' ',
					','
				}, 2);
				if (this.channelToggles.ContainsKey(array5[0]))
				{
					this.ShowChannel(array5[0]);
				}
				else
				{
					this.chatClient.Subscribe(new string[]
					{
						array5[0]
					});
				}
			}
			else
			{
				UnityEngine.Debug.Log("The command '" + array2[0] + "' is invalid.");
			}
		}
		else if (flag)
		{
			this.chatClient.SendPrivateMessage(target, inputLine, false);
		}
		else
		{
			this.chatClient.PublishMessage(this.selectedChannelName, inputLine, false);
		}
	}

	public void PostHelpToCurrentChannel()
	{
		Text currentChannelText = this.CurrentChannelText;
		currentChannelText.text += ChatGui.HelpText;
	}

	public void DebugReturn(DebugLevel level, string message)
	{
		if (level == DebugLevel.ERROR)
		{
			UnityEngine.Debug.LogError(message);
		}
		else if (level == DebugLevel.WARNING)
		{
			UnityEngine.Debug.LogWarning(message);
		}
		else
		{
			UnityEngine.Debug.Log(message);
		}
	}

	public void OnConnected()
	{
		if (this.ChannelsToJoinOnConnect != null && this.ChannelsToJoinOnConnect.Length > 0)
		{
			this.chatClient.Subscribe(this.ChannelsToJoinOnConnect, this.HistoryLengthToFetch);
		}
		this.ConnectingLabel.SetActive(false);
		this.UserIdText.text = "Connected as " + this.UserName;
		this.ChatPanel.gameObject.SetActive(true);
		if (this.FriendsList != null && this.FriendsList.Length > 0)
		{
			this.chatClient.AddFriends(this.FriendsList);
			foreach (string text in this.FriendsList)
			{
				if (this.FriendListUiItemtoInstantiate != null && text != this.UserName)
				{
					this.InstantiateFriendButton(text);
				}
			}
		}
		if (this.FriendListUiItemtoInstantiate != null)
		{
			this.FriendListUiItemtoInstantiate.SetActive(false);
		}
		this.chatClient.SetOnlineStatus(2);
	}

	public void OnDisconnected()
	{
		this.ConnectingLabel.SetActive(false);
	}

	public void OnChatStateChange(ChatState state)
	{
		this.StateText.text = state.ToString();
	}

	public void OnSubscribed(string[] channels, bool[] results)
	{
		foreach (string channelName in channels)
		{
			this.chatClient.PublishMessage(channelName, "says 'hi'.", false);
			if (this.ChannelToggleToInstantiate != null)
			{
				this.InstantiateChannelButton(channelName);
			}
		}
		UnityEngine.Debug.Log("OnSubscribed: " + string.Join(", ", channels));
		this.ShowChannel(channels[0]);
	}

	private void InstantiateChannelButton(string channelName)
	{
		if (this.channelToggles.ContainsKey(channelName))
		{
			UnityEngine.Debug.Log("Skipping creation for an existing channel toggle.");
			return;
		}
		Toggle toggle = UnityEngine.Object.Instantiate<Toggle>(this.ChannelToggleToInstantiate);
		toggle.gameObject.SetActive(true);
		toggle.GetComponentInChildren<ChannelSelector>().SetChannel(channelName);
		toggle.transform.SetParent(this.ChannelToggleToInstantiate.transform.parent, false);
		this.channelToggles.Add(channelName, toggle);
	}

	private void InstantiateFriendButton(string friendId)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.FriendListUiItemtoInstantiate);
		gameObject.gameObject.SetActive(true);
		FriendItem component = gameObject.GetComponent<FriendItem>();
		component.FriendId = friendId;
		gameObject.transform.SetParent(this.FriendListUiItemtoInstantiate.transform.parent, false);
		this.friendListItemLUT[friendId] = component;
	}

	public void OnUnsubscribed(string[] channels)
	{
		foreach (string text in channels)
		{
			if (this.channelToggles.ContainsKey(text))
			{
				Toggle toggle = this.channelToggles[text];
				UnityEngine.Object.Destroy(toggle.gameObject);
				this.channelToggles.Remove(text);
				UnityEngine.Debug.Log("Unsubscribed from channel '" + text + "'.");
				if (text == this.selectedChannelName && this.channelToggles.Count > 0)
				{
					IEnumerator<KeyValuePair<string, Toggle>> enumerator = this.channelToggles.GetEnumerator();
					enumerator.MoveNext();
					KeyValuePair<string, Toggle> keyValuePair = enumerator.Current;
					this.ShowChannel(keyValuePair.Key);
					KeyValuePair<string, Toggle> keyValuePair2 = enumerator.Current;
					keyValuePair2.Value.isOn = true;
				}
			}
			else
			{
				UnityEngine.Debug.Log("Can't unsubscribe from channel '" + text + "' because you are currently not subscribed to it.");
			}
		}
	}

	public void OnGetMessages(string channelName, string[] senders, object[] messages)
	{
		if (channelName.Equals(this.selectedChannelName))
		{
			this.ShowChannel(this.selectedChannelName);
		}
	}

	public void OnPrivateMessage(string sender, object message, string channelName)
	{
		this.InstantiateChannelButton(channelName);
		byte[] array = message as byte[];
		if (array != null)
		{
			UnityEngine.Debug.Log("Message with byte[].Length: " + array.Length);
		}
		if (this.selectedChannelName.Equals(channelName))
		{
			this.ShowChannel(channelName);
		}
	}

	public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
	{
		UnityEngine.Debug.LogWarning("status: " + string.Format("{0} is {1}. Msg:{2}", user, status, message));
		if (this.friendListItemLUT.ContainsKey(user))
		{
			FriendItem friendItem = this.friendListItemLUT[user];
			if (friendItem != null)
			{
				friendItem.OnFriendStatusUpdate(status, gotMessage, message);
			}
		}
	}

	public void AddMessageToSelectedChannel(string msg)
	{
		ChatChannel chatChannel = null;
		if (!this.chatClient.TryGetChannel(this.selectedChannelName, out chatChannel))
		{
			UnityEngine.Debug.Log("AddMessageToSelectedChannel failed to find channel: " + this.selectedChannelName);
			return;
		}
		if (chatChannel != null)
		{
			chatChannel.Add("Bot", msg, 0);
		}
	}

	public void ShowChannel(string channelName)
	{
		if (string.IsNullOrEmpty(channelName))
		{
			return;
		}
		ChatChannel chatChannel = null;
		if (!this.chatClient.TryGetChannel(channelName, out chatChannel))
		{
			UnityEngine.Debug.Log("ShowChannel failed to find channel: " + channelName);
			return;
		}
		this.selectedChannelName = channelName;
		this.CurrentChannelText.text = chatChannel.ToStringMessages();
		UnityEngine.Debug.Log("ShowChannel: " + this.selectedChannelName);
		foreach (KeyValuePair<string, Toggle> keyValuePair in this.channelToggles)
		{
			keyValuePair.Value.isOn = (keyValuePair.Key == channelName);
		}
	}

	public void OpenDashboard()
	{
		Application.OpenURL("https://dashboard.photonengine.com");
	}

	public string[] ChannelsToJoinOnConnect;

	public string[] FriendsList;

	public int HistoryLengthToFetch;

	private string selectedChannelName;

	public ChatClient chatClient;

	protected internal AppSettings chatAppSettings;

	public GameObject missingAppIdErrorPanel;

	public GameObject ConnectingLabel;

	public RectTransform ChatPanel;

	public GameObject UserIdFormPanel;

	public InputField InputFieldChat;

	public Text CurrentChannelText;

	public Toggle ChannelToggleToInstantiate;

	public GameObject FriendListUiItemtoInstantiate;

	private readonly Dictionary<string, Toggle> channelToggles = new Dictionary<string, Toggle>();

	private readonly Dictionary<string, FriendItem> friendListItemLUT = new Dictionary<string, FriendItem>();

	public bool ShowState = true;

	public GameObject Title;

	public Text StateText;

	public Text UserIdText;

	private static string HelpText = "\n    -- HELP --\nTo subscribe to channel(s):\n\t<color=#E07B00>\\subscribe</color> <color=green><list of channelnames></color>\n\tor\n\t<color=#E07B00>\\s</color> <color=green><list of channelnames></color>\n\nTo leave channel(s):\n\t<color=#E07B00>\\unsubscribe</color> <color=green><list of channelnames></color>\n\tor\n\t<color=#E07B00>\\u</color> <color=green><list of channelnames></color>\n\nTo switch the active channel\n\t<color=#E07B00>\\join</color> <color=green><channelname></color>\n\tor\n\t<color=#E07B00>\\j</color> <color=green><channelname></color>\n\nTo send a private message:\n\t\\<color=#E07B00>msg</color> <color=green><username></color> <color=green><message></color>\n\nTo change status:\n\t\\<color=#E07B00>state</color> <color=green><stateIndex></color> <color=green><message></color>\n<color=green>0</color> = Offline <color=green>1</color> = Invisible <color=green>2</color> = Online <color=green>3</color> = Away \n<color=green>4</color> = Do not disturb <color=green>5</color> = Looking For Group <color=green>6</color> = Playing\n\nTo clear the current chat tab (private chats get closed):\n\t<color=#E07B00>\\clear</color>";

	public int TestLength = 2048;

	private byte[] testBytes = new byte[2048];
}
