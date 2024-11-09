using System;
using UnityEngine;

namespace Util.Data
{
	[CreateAssetMenu(fileName = "DataShopItem", menuName = "Assets/DataShopItem", order = 1)]
	public class DataShopItem : ScriptableObject
	{
		public ShopItemInfo[] shopItem;
	}
}
