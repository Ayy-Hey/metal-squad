using System;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	public static T Instance
	{
		get
		{
			if (MonoSingleton<T>._instance == null)
			{
				MonoSingleton<T>._instance = UnityEngine.Object.FindObjectOfType<T>();
				if (MonoSingleton<T>._instance == null)
				{
					GameObject gameObject = new GameObject(typeof(T).Name);
					MonoSingleton<T>._instance = gameObject.AddComponent<T>();
				}
			}
			return MonoSingleton<T>._instance;
		}
	}

	protected virtual void Awake()
	{
		if (MonoSingleton<T>._instance == null)
		{
			MonoSingleton<T>._instance = (this as T);
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private static T _instance;
}
