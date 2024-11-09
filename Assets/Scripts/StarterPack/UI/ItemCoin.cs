using System;
using UnityEngine;
using UnityEngine.UI;

namespace StarterPack.UI
{
	public class ItemCoin : MonoBehaviour
	{
		public void Show(int value)
		{
			this.txtValue.text = "x" + value;
		}

		[SerializeField]
		private Text txtValue;
	}
}
