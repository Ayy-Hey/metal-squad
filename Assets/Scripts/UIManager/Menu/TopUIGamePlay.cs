using System;
using System.Collections.Generic;
using UnityEngine;

namespace UIManager.Menu
{
	public class TopUIGamePlay : MonoBehaviour
	{
		private void Awake()
		{
			while (FormLoadout.indexBoostersSelected != null && FormLoadout.indexBoostersSelected.Count > 0)
			{
				int num = FormLoadout.indexBoostersSelected[0];
				num--;
				this.ListIcon[num].gameObject.SetActive(true);
				FormLoadout.indexBoostersSelected.RemoveAt(0);
			}
			int num2 = 0;
			for (int i = 0; i < this.ListIcon.Count; i++)
			{
				if (this.ListIcon[i].gameObject.activeSelf)
				{
					Vector3 v = default(Vector3);
					v.x = (float)(-41 + num2 * 27);
					v.y = 51f;
					this.ListIcon[i].anchoredPosition = v;
					num2++;
				}
			}
		}

		public List<RectTransform> ListIcon;
	}
}
