using System;
using Photon.Pun;
using UnityEngine;

namespace PVPManager
{
	public class PhotonSingleton<T> : MonoBehaviourPunCallbacks where T : MonoBehaviourPunCallbacks
	{
		public static T Instance
		{
			get
			{
				if (PhotonSingleton<T>.m_ShuttingDown)
				{
					UnityEngine.Debug.LogWarning("[Singleton] Instance '" + typeof(T) + "' already destroyed. Returning null.");
					return (T)((object)null);
				}
				object @lock = PhotonSingleton<T>.m_Lock;
				T instance;
				lock (@lock)
				{
					if (PhotonSingleton<T>.m_Instance == null)
					{
						PhotonSingleton<T>.m_Instance = (T)((object)UnityEngine.Object.FindObjectOfType(typeof(T)));
						if (PhotonSingleton<T>.m_Instance == null)
						{
							GameObject gameObject = new GameObject();
							PhotonSingleton<T>.m_Instance = gameObject.AddComponent<T>();
							gameObject.name = typeof(T).ToString() + " (Singleton)";
							UnityEngine.Object.DontDestroyOnLoad(gameObject);
						}
					}
					instance = PhotonSingleton<T>.m_Instance;
				}
				return instance;
			}
		}

		private void OnApplicationQuit()
		{
			PhotonSingleton<T>.m_ShuttingDown = true;
		}

		private void OnDestroy()
		{
			PhotonSingleton<T>.m_ShuttingDown = true;
		}

		private static bool m_ShuttingDown = false;

		private static object m_Lock = new object();

		private static T m_Instance;
	}
}
