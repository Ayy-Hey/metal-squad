using System;
using UnityEngine;

public class UIControl : MonoBehaviour
{
	private void Update()
	{
		if (SplashScreen._isBuildMarketing)
		{
			for (int i = 0; i < this.AllObject.Length; i++)
			{
				this.AllObject[i].SetActive(false);
			}
		}
	}

	[SerializeField]
	private GameObject[] AllObject;
}
