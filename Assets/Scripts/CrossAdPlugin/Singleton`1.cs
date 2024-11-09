using System;
using UnityEngine;

namespace CrossAdPlugin
{
	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		public static T Instance
		{
			get
			{
				if (Singleton<T>._instance == null)
				{
					Singleton<T>._instance = UnityEngine.Object.FindObjectOfType<T>();
					if (Singleton<T>._instance == null)
					{
						Singleton<T>._instance = new GameObject("[" + typeof(T).Name + "]").AddComponent<T>();
					}
				}
				return Singleton<T>._instance;
			}
		}

		protected virtual void Awake()
		{
			if (this != Singleton<T>.Instance)
			{
				GameObject gameObject = base.gameObject;
				UnityEngine.Object.Destroy(this);
				UnityEngine.Object.Destroy(gameObject);
				return;
			}
		}

		private static T _instance;
	}
}
