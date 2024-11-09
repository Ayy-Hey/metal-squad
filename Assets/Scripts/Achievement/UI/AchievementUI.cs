using System;
using System.Collections.Generic;
using UnityEngine;

namespace Achievement.UI
{
	public class AchievementUI : MonoBehaviour
	{
		public void Show(Action<bool> callback, Action<bool> callbackRank)
		{
			this.obj_ListAchievement.SetActive(true);
			this.obj_ActiveAchievement.SetActive(true);
			AchievementManager.Instance.MissionWeaponShop(null);
			for (int i = 0; i < DataLoader.achievement.Length; i++)
			{
				if (this.elements.Count < DataLoader.achievement.Length)
				{
					ElementAchievement component = UnityEngine.Object.Instantiate<ElementAchievement>(this.elementAchievement).GetComponent<ElementAchievement>();
					component.transform.parent = this.elementAchievement.transform.parent;
					this.elements.Add(component);
				}
				if (DataLoader.achievement[i].achievement.State == EChievement.DOING && DataLoader.achievement[i].achievement.Total >= DataLoader.achievement[i].achievement.TotalRequirement)
				{
					DataLoader.achievement[i].achievement.State = EChievement.DONE;
					
				}
			}
			for (int j = 0; j < this.elements.Count; j++)
			{
				this.elements[j].Show(DataLoader.achievement[j].achievement, j, delegate(bool cb)
				{
					callback(cb);
					if (cb)
					{
						this.Resort();
					}
				}, delegate(bool cbrank)
				{
					callbackRank(cbrank);
				});
			}
			this.Resort();
		}

		private void Resort()
		{
			for (int i = 0; i < this.elements.Count; i++)
			{
				if (DataLoader.achievement[i].achievement.State == EChievement.DONE)
				{
					this.elements[i].transform.SetSiblingIndex(0);
				}
			}
			for (int j = this.elements.Count - 1; j >= 0; j--)
			{
				if (DataLoader.achievement[j].achievement.State == EChievement.CLEAR)
				{
					this.elements[j].transform.SetSiblingIndex(this.elements.Count);
				}
			}
		}

		public void Hide()
		{
			this.obj_ListAchievement.SetActive(false);
			this.obj_ActiveAchievement.SetActive(false);
		}

		public List<ElementAchievement> elements;

		[SerializeField]
		private ElementAchievement elementAchievement;

		public GameObject obj_ListAchievement;

		public GameObject obj_ActiveAchievement;
	}
}
