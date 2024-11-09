using System;
using UnityEngine;
using UnityEngine.UI;

namespace Profile.UI
{
	public class MapProfile : MonoBehaviour
	{
		public void Show()
		{
			for (int i = 0; i < ProfileManager.worldMapProfile.Length; i++)
			{
			}
		}

		[SerializeField]
		private Text[] txtStar;
	}
}
