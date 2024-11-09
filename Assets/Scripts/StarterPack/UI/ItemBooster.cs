using System;
using UnityEngine;
using UnityEngine.UI;

namespace StarterPack.UI
{
	public class ItemBooster : MonoBehaviour
	{
		public void Show(ItemBoosterData item)
		{
			this.imgIcon.sprite = this.sprites[item.ID];
			this.txtValue.text = "x" + item.Value;
		}

		[SerializeField]
		private Sprite[] sprites;

		[SerializeField]
		private Image imgIcon;

		[SerializeField]
		private Text txtValue;
	}
}
