using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadSceneOnKeyDown : MonoBehaviour
{
	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(this.reloadKey))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
		}
	}

	public KeyCode reloadKey = KeyCode.R;
}
