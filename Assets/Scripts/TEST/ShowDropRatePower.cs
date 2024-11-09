using System;
using UnityEngine;
using UnityEngine.UI;

namespace TEST
{
	public class ShowDropRatePower : MonoBehaviour
	{
		private void Start()
		{
			if (SplashScreen._isTesting)
			{
				this.txt = base.GetComponent<Text>();
			}
		}

		private void Update()
		{
			if (SplashScreen._isTesting)
			{
				this.txt.text = GameManager.Instance.RatePower * 100f + "%";
			}
		}

		private Text txt;
	}
}
