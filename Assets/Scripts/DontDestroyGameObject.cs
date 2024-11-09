using System;
using UnityEngine;

public class DontDestroyGameObject : MonoBehaviour
{
	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}
}
