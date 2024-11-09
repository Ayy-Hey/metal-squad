using System;
using UnityEngine;

public class SingletonGame<T> : MonoBehaviour where T : MonoBehaviour
{
	public static T Instance
	{
		get
		{
			if (SingletonGame<T>._instance == null)
			{
				SingletonGame<T>._instance = (T)((object)UnityEngine.Object.FindObjectOfType(typeof(T)));
				if (UnityEngine.Object.FindObjectsOfType(typeof(T)).Length > 1)
				{
					return SingletonGame<T>._instance;
				}
				if (SingletonGame<T>._instance == null)
				{
					GameObject gameObject = new GameObject();
					SingletonGame<T>._instance = gameObject.AddComponent<T>();
					gameObject.name = "(singleton)" + typeof(T).ToString();
				}
			}
			return SingletonGame<T>._instance;
		}
	}

	private static T _instance;
}
