using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
	public static EventManager instance
	{
		get
		{
			if (!EventManager.eventManager)
			{
				EventManager.eventManager = (UnityEngine.Object.FindObjectOfType(typeof(EventManager)) as EventManager);
				if (EventManager.eventManager)
				{
					EventManager.eventManager.Init();
				}
			}
			return EventManager.eventManager;
		}
	}

	private void Init()
	{
		if (this.eventDictionary == null)
		{
			this.eventDictionary = new Dictionary<string, UnityEvent>();
		}
	}

	public static void StartListening(string eventName, UnityAction listener)
	{
		UnityEvent unityEvent = null;
		if (EventManager.instance.eventDictionary.TryGetValue(eventName, out unityEvent))
		{
			unityEvent.AddListener(listener);
		}
		else
		{
			unityEvent = new UnityEvent();
			unityEvent.AddListener(listener);
			EventManager.instance.eventDictionary.Add(eventName, unityEvent);
		}
	}

	public static void StopListening(string eventName, UnityAction listener)
	{
		if (EventManager.eventManager == null)
		{
			return;
		}
		UnityEvent unityEvent = null;
		if (EventManager.instance.eventDictionary.TryGetValue(eventName, out unityEvent))
		{
			unityEvent.RemoveListener(listener);
		}
	}

	public static void TriggerEvent(string eventName)
	{
		UnityEvent unityEvent = null;
		if (EventManager.instance && EventManager.instance.eventDictionary.TryGetValue(eventName, out unityEvent))
		{
			unityEvent.Invoke();
		}
	}

	public static void AddEventNextFrame(UnityAction listener)
	{
		UnityEvent unityEvent = new UnityEvent();
		unityEvent.AddListener(listener);
		EventManager.instance.eventStack.Add(unityEvent);
	}

	private void Update()
	{
		while (EventManager.instance.eventStack.Count > 0)
		{
			UnityEvent unityEvent = EventManager.instance.eventStack[0];
			unityEvent.Invoke();
			EventManager.instance.eventStack.RemoveAt(0);
		}
	}

	private Dictionary<string, UnityEvent> eventDictionary;

	private List<UnityEvent> eventStack = new List<UnityEvent>();

	private static EventManager eventManager;
}
