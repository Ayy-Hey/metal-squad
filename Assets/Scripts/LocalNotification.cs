using System;
using UnityEngine;

public class LocalNotification
{
	private static string bundleIdentifier
	{
		get
		{
			return Application.identifier;
		}
	}

	public static int SendNotification(TimeSpan delay, string title, string message, Color32 bgColor, bool sound = true, bool vibrate = true, bool lights = true, string bigIcon = "", string soundName = null, string channel = "default", params LocalNotification.Action[] actions)
	{
		int id = new System.Random().Next();
		return LocalNotification.SendNotification(id, (long)delay.TotalSeconds * 1000L, title, message, bgColor, sound, vibrate, lights, bigIcon, soundName, channel, actions);
	}

	public static int SendNotification(int id, TimeSpan delay, string title, string message, Color32 bgColor, bool sound = true, bool vibrate = true, bool lights = true, string bigIcon = "", string soundName = null, string channel = "default", params LocalNotification.Action[] actions)
	{
		return LocalNotification.SendNotification(id, (long)delay.TotalSeconds * 1000L, title, message, bgColor, sound, vibrate, lights, bigIcon, soundName, channel, actions);
	}

	public static int SendNotification(int id, long delayMs, string title, string message, Color32 bgColor, bool sound = true, bool vibrate = true, bool lights = true, string bigIcon = "", string soundName = null, string channel = "default", params LocalNotification.Action[] actions)
	{
		UnityEngine.Debug.Log("???SendNotification");
		AndroidJavaClass androidJavaClass = new AndroidJavaClass(LocalNotification.fullClassName);
		if (androidJavaClass != null)
		{
			androidJavaClass.CallStatic("SetNotification", new object[]
			{
				id,
				delayMs,
				title,
				message,
				message,
				(!sound) ? 0 : 1,
				soundName,
				(!vibrate) ? 0 : 1,
				(!lights) ? 0 : 1,
				bigIcon,
				"notify_icon_small",
				LocalNotification.ToInt(bgColor),
				LocalNotification.bundleIdentifier,
				channel,
				LocalNotification.PopulateActions(actions)
			});
		}
		UnityEngine.Debug.Log("???SendNotification_Return" + id);
		return id;
	}

	public static int SendRepeatingNotification(TimeSpan delay, TimeSpan timeout, string title, string message, Color32 bgColor, bool sound = true, bool vibrate = true, bool lights = true, string bigIcon = "", string soundName = null, string channel = "default", params LocalNotification.Action[] actions)
	{
		int id = new System.Random().Next();
		return LocalNotification.SendRepeatingNotification(id, (long)delay.TotalSeconds * 1000L, (long)((int)timeout.TotalSeconds * 1000), title, message, bgColor, sound, vibrate, lights, bigIcon, soundName, channel, actions);
	}

	public static int SendRepeatingNotification(int id, TimeSpan delay, TimeSpan timeout, string title, string message, Color32 bgColor, bool sound = true, bool vibrate = true, bool lights = true, string bigIcon = "", string soundName = null, string channel = "default", params LocalNotification.Action[] actions)
	{
		return LocalNotification.SendRepeatingNotification(id, (long)delay.TotalSeconds * 1000L, (long)((int)timeout.TotalSeconds * 1000), title, message, bgColor, sound, vibrate, lights, bigIcon, soundName, channel, actions);
	}

	public static int SendRepeatingNotification(int id, long delayMs, long timeoutMs, string title, string message, Color32 bgColor, bool sound = true, bool vibrate = true, bool lights = true, string bigIcon = "", string soundName = null, string channel = "default", params LocalNotification.Action[] actions)
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass(LocalNotification.fullClassName);
		if (androidJavaClass != null)
		{
			androidJavaClass.CallStatic("SetRepeatingNotification", new object[]
			{
				id,
				delayMs,
				title,
				message,
				message,
				timeoutMs,
				(!sound) ? 0 : 1,
				soundName,
				(!vibrate) ? 0 : 1,
				(!lights) ? 0 : 1,
				bigIcon,
				"notify_icon_small",
				LocalNotification.ToInt(bgColor),
				LocalNotification.bundleIdentifier,
				channel,
				LocalNotification.PopulateActions(actions)
			});
		}
		return id;
	}

	public static void CancelNotification(int id)
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass(LocalNotification.fullClassName);
		if (androidJavaClass != null)
		{
			androidJavaClass.CallStatic("CancelPendingNotification", new object[]
			{
				id
			});
		}
	}

	public static void ClearNotifications()
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass(LocalNotification.fullClassName);
		if (androidJavaClass != null)
		{
			androidJavaClass.CallStatic("ClearShowingNotifications", new object[0]);
		}
	}

	public static void CreateChannel(string identifier, string name, string description, Color32 lightColor, bool enableLights = true, string soundName = null, LocalNotification.Importance importance = LocalNotification.Importance.Default, bool vibrate = true, long[] vibrationPattern = null)
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass(LocalNotification.fullClassName);
		if (androidJavaClass != null)
		{
			androidJavaClass.CallStatic("CreateChannel", new object[]
			{
				identifier,
				name,
				description,
				(int)importance,
				soundName,
				(!enableLights) ? 0 : 1,
				LocalNotification.ToInt(lightColor),
				(!vibrate) ? 0 : 1,
				vibrationPattern,
				LocalNotification.bundleIdentifier
			});
		}
	}

	private static int ToInt(Color32 color)
	{
		return (int)color.r * 65536 + (int)color.g * 256 + (int)color.b;
	}

	private static AndroidJavaObject PopulateActions(LocalNotification.Action[] actions)
	{
		AndroidJavaObject androidJavaObject = null;
		if (actions.Length > 0)
		{
			androidJavaObject = new AndroidJavaObject("java.util.ArrayList", new object[0]);
			foreach (LocalNotification.Action action in actions)
			{
				using (AndroidJavaObject androidJavaObject2 = new AndroidJavaObject(LocalNotification.actionClassName, new object[0]))
				{
					androidJavaObject2.Call("setIdentifier", new object[]
					{
						action.Identifier
					});
					androidJavaObject2.Call("setTitle", new object[]
					{
						action.Title
					});
					androidJavaObject2.Call("setIcon", new object[]
					{
						action.Icon
					});
					androidJavaObject2.Call("setForeground", new object[]
					{
						action.Foreground
					});
					androidJavaObject2.Call("setGameObject", new object[]
					{
						action.GameObject
					});
					androidJavaObject2.Call("setHandlerMethod", new object[]
					{
						action.HandlerMethod
					});
					androidJavaObject.Call<bool>("add", new object[]
					{
						androidJavaObject2
					});
				}
			}
		}
		return androidJavaObject;
	}

	private static string fullClassName = "net.agasper.unitynotification.UnityNotificationManager";

	private static string actionClassName = "net.agasper.unitynotification.NotificationAction";

	public enum Importance
	{
		Default = 3,
		High,
		Low = 2,
		Max = 5,
		Min = 1,
		None = 0
	}

	public class Action
	{
		public Action(string identifier, string title, MonoBehaviour handler)
		{
			this.Identifier = identifier;
			this.Title = title;
			if (handler != null)
			{
				this.GameObject = handler.gameObject.name;
				this.HandlerMethod = "OnAction";
			}
		}

		public string Identifier;

		public string Title;

		public string Icon;

		public bool Foreground = true;

		public string GameObject;

		public string HandlerMethod;
	}
}
