using System;
using ABI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UIManager.Popup
{
	public class DialogWarningOldData : MonoBehaviour
	{
		public void OnShow(int version, Action OnCompleted)
		{
			this.version = version;
			this.OnCompleted = OnCompleted;
			base.gameObject.SetActive(true);
			this.objChild.SetActive(true);
			base.transform.localScale = Vector3.one;
			base.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
			base.transform.SetAsLastSibling();
		}

		private void OnDisable()
		{
			if (this.OnCompleted != null)
			{
				this.OnCompleted();
			}
		}

		public void OnRestore(int typeData)
		{
			this.objChild.SetActive(false);
			if (typeData != 0)
			{
				if (typeData == 1)
				{
					if (FirebaseDatabaseManager.Instance.isLoginFB)
					{
						this.OnRestoreWithID(MonoSingleton<FBAndPlayfabMgr>.Instance.PlayFabUserId);
					}
					else
					{
						
					}
				}
			}
			else
			{
				this.OnRestoreWithID(SystemInfo.deviceUniqueIdentifier);
			}
		}

		private void OnRestoreWithID(string ID)
		{
			PopupManager.Instance.ShowMiniLoading();
			DataLoader.LoadData();
			DataLoader.LoadDataBossMode();
			DataLoader.LoadDataCampaign();
			DataLoader.LoadDataEndlessMode();
			FirebaseDatabaseManager.Instance.DoRestoreDataWithID(ID, delegate(bool isSuccess)
			{
				PopupManager.Instance.CloseMiniLoading();
				if (isSuccess)
				{
					MenuManager.Instance.tutorial.CloseTutorial();
					base.transform.localScale = Vector3.zero;
					PopupManager.Instance.ShowDialog(delegate(bool ok)
					{
						MenuManager.Instance.objLoading.gameObject.SetActive(true);
						SceneManager.LoadScene("Menu");
						ProfileManager.versionProfile.setValue(this.version);
						LogUserCheat.isCheck = true;
					}, 0, PopupManager.Instance.GetText(Localization0.Sync_Data_Success, null), PopupManager.Instance.GetText(Localization0.Congratulations, null));
				}
				else
				{
					PopupManager.Instance.ShowDialog(delegate(bool ok)
					{
						this.objChild.SetActive(true);
					}, 0, PopupManager.Instance.GetText(Localization0.Cannot_connect_to_Server, null), PopupManager.Instance.GetText(Localization0.Error, null));
				}
			});
		}

		private Action OnCompleted;

		public GameObject objChild;

		private int version;

		public enum TypeData
		{
			Device,
			Facebook
		}
	}
}
