using System;
using System.Collections.Generic;

public class EventDispatcher
{
	public static void RegisterListener(string eventid, Action callback)
	{
		EventDispatcher.AddListener(eventid + "noparamid", callback);
	}

	public static void RegisterListener<T>(string eventid, Action<T> callback) where T : BaseMessage
	{
		EventDispatcher.AddListener(eventid + typeof(T).ToString(), callback);
	}

	public static void PostEvent(string eventid)
	{
		string key = eventid + "noparamid";
		if (EventDispatcher._listeners.ContainsKey(key))
		{
			List<Delegate> list = EventDispatcher._listeners[key];
			if (list == null)
			{
				return;
			}
			foreach (Delegate @delegate in list)
			{
				if (@delegate.GetType() == typeof(Action))
				{
					Action action = (Action)@delegate;
					action();
				}
			}
		}
	}

	public static void PostEvent<T>(string eventid, T param) where T : BaseMessage
	{
		string key = eventid + typeof(T).ToString();
		if (EventDispatcher._listeners.ContainsKey(key))
		{
			List<Delegate> list = EventDispatcher._listeners[key];
			if (list == null)
			{
				return;
			}
			foreach (Delegate @delegate in list)
			{
				if (@delegate.GetType() == typeof(Action<T>))
				{
					Action<T> action = (Action<T>)@delegate;
					action(param);
				}
			}
		}
	}

	public static void RemoveListener(string eventid, Action callback)
	{
		EventDispatcher.DeleteListener(eventid + "noparamid", callback);
	}

	public static void RemoveListener<T>(string eventid, Action<T> callback) where T : BaseMessage
	{
		EventDispatcher.DeleteListener(eventid + typeof(T).ToString(), callback);
	}

	private static void AddListener(string key, Delegate callback)
	{
		List<Delegate> list;
		if (!EventDispatcher._listeners.ContainsKey(key))
		{
			list = new List<Delegate>();
			EventDispatcher._listeners.Add(key, list);
		}
		else
		{
			list = EventDispatcher._listeners[key];
		}
		if (list.Contains(callback))
		{
			return;
		}
		list.Add(callback);
	}

	private static void DeleteListener(string key, Delegate callback)
	{
		if (EventDispatcher._listeners.ContainsKey(key))
		{
			List<Delegate> list = EventDispatcher._listeners[key];
			if (list.Contains(callback))
			{
				list.Remove(callback);
			}
		}
	}

	public const string noparamid = "noparamid";

	private static Dictionary<string, List<Delegate>> _listeners = new Dictionary<string, List<Delegate>>();
}
