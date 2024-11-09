using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TwoDLaserPack
{
	public class DemoSceneNavigation : MonoBehaviour
	{
		private void Start()
		{
			this.buttonNextDemo.onClick.AddListener(new UnityAction(this.OnButtonNextDemoClick));
		}

		private void OnButtonNextDemoClick()
		{
			int loadedLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
			if (loadedLevel < Application.levelCount - 1)
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene(loadedLevel + 1);
			}
			else
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene(0);
			}
		}

		private void Update()
		{
		}

		public Button buttonNextDemo;
	}
}
