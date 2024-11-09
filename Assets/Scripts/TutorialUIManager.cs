using System;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUIManager : MonoBehaviour
{
	public void StartTutorial(TutorialUIManager.TutorialID tutorialID)
	{
		TutorialUIManager.isFirstTutorial = true;
		this.OnSkipTut = null;
		MenuManager.Instance.ClearPopUpStack();
		TutorialUIManager.tutorialIDCurrent = tutorialID;
		TutorialUIManager.indexStepTutorial = 0;
		this.listTutorialUI[(int)TutorialUIManager.tutorialIDCurrent].cardsTutorial[TutorialUIManager.indexStepTutorial].gameObject.SetActive(true);
		this.LoadTarget(TutorialUIManager.tutorialIDCurrent, TutorialUIManager.indexStepTutorial);
		GameObject obj_Target = this.listTutorialUI[(int)TutorialUIManager.tutorialIDCurrent].cardsTutorial[TutorialUIManager.indexStepTutorial].obj_Target;
		if (obj_Target != null)
		{
			TutorialUIManager.canvasTarget = this.listTutorialUI[(int)TutorialUIManager.tutorialIDCurrent].cardsTutorial[TutorialUIManager.indexStepTutorial].obj_Target.AddComponent<Canvas>();
			TutorialUIManager.canvasTarget.overrideSorting = true;
			TutorialUIManager.canvasTarget.sortingOrder = 2;
			TutorialUIManager.canvasTarget.sortingLayerName = "Tutorial";
			if (this.listTutorialUI[(int)TutorialUIManager.tutorialIDCurrent].cardsTutorial[TutorialUIManager.indexStepTutorial].isRaycaster)
			{
				TutorialUIManager.raycasterTarget = this.listTutorialUI[(int)TutorialUIManager.tutorialIDCurrent].cardsTutorial[TutorialUIManager.indexStepTutorial].obj_Target.AddComponent<GraphicRaycaster>();
			}
		}
		else
		{
			TutorialUIManager.canvasTarget = null;
			TutorialUIManager.raycasterTarget = null;
		}
		this.ResizeIphoneX();
	}

	public void NextTutorial(int indexStepCurrent)
	{
		if (indexStepCurrent != TutorialUIManager.indexStepTutorial + 1)
		{
			UnityEngine.Debug.LogError("Lỗi: Next Tutorial");
		}
		this.listTutorialUI[(int)TutorialUIManager.tutorialIDCurrent].cardsTutorial[TutorialUIManager.indexStepTutorial].gameObject.SetActive(false);
		if (this.listTutorialUI[(int)TutorialUIManager.tutorialIDCurrent].cardsTutorial.Length > TutorialUIManager.indexStepTutorial + 1)
		{
			TutorialUIManager.indexStepTutorial++;
			this.listTutorialUI[(int)TutorialUIManager.tutorialIDCurrent].cardsTutorial[TutorialUIManager.indexStepTutorial].gameObject.SetActive(true);
			this.LoadTarget(TutorialUIManager.tutorialIDCurrent, TutorialUIManager.indexStepTutorial);
			if (TutorialUIManager.canvasTarget != null && TutorialUIManager.canvasTarget.gameObject != this.listTutorialUI[(int)TutorialUIManager.tutorialIDCurrent].cardsTutorial[TutorialUIManager.indexStepTutorial].obj_Target)
			{
				if (TutorialUIManager.canvasTarget.gameObject != null)
				{
					UnityEngine.Object.Destroy(TutorialUIManager.raycasterTarget);
					UnityEngine.Object.Destroy(TutorialUIManager.canvasTarget);
				}
				if (this.listTutorialUI[(int)TutorialUIManager.tutorialIDCurrent].cardsTutorial[TutorialUIManager.indexStepTutorial].obj_Target != null)
				{
					TutorialUIManager.canvasTarget = this.listTutorialUI[(int)TutorialUIManager.tutorialIDCurrent].cardsTutorial[TutorialUIManager.indexStepTutorial].obj_Target.AddComponent<Canvas>();
					TutorialUIManager.canvasTarget.overrideSorting = true;
					UnityEngine.Debug.Log("obj_Target_1" + TutorialUIManager.canvasTarget.name);
					TutorialUIManager.canvasTarget.sortingOrder = 2;
					TutorialUIManager.canvasTarget.sortingLayerName = "Tutorial";
					if (this.listTutorialUI[(int)TutorialUIManager.tutorialIDCurrent].cardsTutorial[TutorialUIManager.indexStepTutorial].isRaycaster)
					{
						TutorialUIManager.raycasterTarget = this.listTutorialUI[(int)TutorialUIManager.tutorialIDCurrent].cardsTutorial[TutorialUIManager.indexStepTutorial].obj_Target.AddComponent<GraphicRaycaster>();
					}
				}
			}
			else if (TutorialUIManager.canvasTarget == null && this.listTutorialUI[(int)TutorialUIManager.tutorialIDCurrent].cardsTutorial[TutorialUIManager.indexStepTutorial].obj_Target != null)
			{
				TutorialUIManager.canvasTarget = this.listTutorialUI[(int)TutorialUIManager.tutorialIDCurrent].cardsTutorial[TutorialUIManager.indexStepTutorial].obj_Target.AddComponent<Canvas>();
				if (this.listTutorialUI[(int)TutorialUIManager.tutorialIDCurrent].cardsTutorial[TutorialUIManager.indexStepTutorial].isRaycaster)
				{
					TutorialUIManager.raycasterTarget = this.listTutorialUI[(int)TutorialUIManager.tutorialIDCurrent].cardsTutorial[TutorialUIManager.indexStepTutorial].obj_Target.AddComponent<GraphicRaycaster>();
				}
				UnityEngine.Debug.Log("dkm_" + TutorialUIManager.canvasTarget.overrideSorting);
				TutorialUIManager.canvasTarget.overrideSorting = true;
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					"obj_Target_2",
					TutorialUIManager.canvasTarget.name,
					"_canvasTarget.overrideSorting_",
					TutorialUIManager.canvasTarget.overrideSorting
				}));
				TutorialUIManager.canvasTarget.sortingOrder = 2;
				TutorialUIManager.canvasTarget.sortingLayerName = "Tutorial";
			}
			this.ResizeIphoneX();
		}
		else
		{
			if (this.listTutorialUI[(int)TutorialUIManager.tutorialIDCurrent] != null)
			{
				PlayerPrefs.SetInt(this.listTutorialUI[(int)TutorialUIManager.tutorialIDCurrent].keyPlayerPrefs, 1);
			}
			TutorialUIManager.tutorialIDCurrent = TutorialUIManager.TutorialID.None;
			TutorialUIManager.indexStepTutorial = 0;
			UnityEngine.Object.Destroy(TutorialUIManager.raycasterTarget);
			UnityEngine.Object.Destroy(TutorialUIManager.canvasTarget);
		}
	}

	public void CloseTutorial()
	{
		try
		{
			UnityEngine.Object.Destroy(TutorialUIManager.raycasterTarget);
			UnityEngine.Object.Destroy(TutorialUIManager.canvasTarget);
			if (TutorialUIManager.tutorialIDCurrent != TutorialUIManager.TutorialID.None)
			{
				for (int i = 0; i < this.listTutorialUI[(int)TutorialUIManager.tutorialIDCurrent].cardsTutorial.Length; i++)
				{
					this.listTutorialUI[(int)TutorialUIManager.tutorialIDCurrent].cardsTutorial[i].gameObject.SetActive(false);
				}
				if (this.listTutorialUI[(int)TutorialUIManager.tutorialIDCurrent] != null)
				{
					PlayerPrefs.SetInt(this.listTutorialUI[(int)TutorialUIManager.tutorialIDCurrent].keyPlayerPrefs, 1);
				}
			}
			TutorialUIManager.tutorialIDCurrent = TutorialUIManager.TutorialID.None;
			TutorialUIManager.indexStepTutorial = 0;
			if (this.OnSkipTut != null)
			{
				this.OnSkipTut();
				this.OnSkipTut = null;
			}
		}
		catch (Exception message)
		{
			UnityEngine.Debug.LogError(message);
		}
	}

	private void ResizeIphoneX()
	{
		RectTransform rect_BtnSkip = this.listTutorialUI[(int)TutorialUIManager.tutorialIDCurrent].cardsTutorial[TutorialUIManager.indexStepTutorial].rect_BtnSkip;
		if (rect_BtnSkip != null)
		{
			if (ThisPlatform.IsIphoneX)
			{
				rect_BtnSkip.localPosition = new Vector3(150f, rect_BtnSkip.localPosition.y, 0f);
			}
			else
			{
				rect_BtnSkip.localPosition = new Vector3(90f, rect_BtnSkip.localPosition.y, 0f);
			}
		}
	}

	public void LoadRewardQuest()
	{
		int num = DailyQuestManager.Instance.CheckDailyQuest();
		this.listTutorialUI[(int)TutorialUIManager.tutorialIDCurrent].cardsTutorial[1].obj_Target = MenuManager.Instance.popupQuest.dailyQuest.Mission[num].objClaim[1];
		this.listTutorialUI[(int)TutorialUIManager.tutorialIDCurrent].cardsTutorial[1].transform_TipInfor.gameObject.SetActive(MenuManager.Instance.popupQuest.dailyQuest.Mission[num].objClaim[1].activeSelf);
	}

	public void LoadTarget(TutorialUIManager.TutorialID IDTutorial, int IDStep)
	{
		if (this.listTutorialUI[(int)IDTutorial] != null && this.listTutorialUI[(int)IDTutorial].cardsTutorial.Length > IDStep)
		{
			CardTutorial cardTutorial = this.listTutorialUI[(int)IDTutorial].cardsTutorial[IDStep];
			if (cardTutorial.IDForm != FormUI.None && cardTutorial.obj_Target == null)
			{
				cardTutorial.obj_Target = MenuManager.Instance.formCurrent.obj_ListTut[cardTutorial.IDObj];
			}
		}
	}

	public static int indexStepTutorial;

	public static bool isFirstTutorial;

	public static TutorialUIManager.TutorialID tutorialIDCurrent;

	public static Canvas canvasTarget;

	public static GraphicRaycaster raycasterTarget;

	public Action OnSkipTut;

	public TutorialUI[] listTutorialUI;

	public enum TutorialID
	{
		None,
		Tut_FormMainMenu_1,
		Tut_FormCampaign_1,
		Tut_FormLoadout_1,
		Tut_FormBossMode_1,
		Tut_UpgradeWeapon,
		Tut_UpgradeCharacter,
		Tut_RewardStar,
		Tut_UnlockDifficult,
		Tut_UnlockMap,
		Tut_Endless,
		Tut_RewardQuest,
		Tut_LevelUpVip
	}
}
