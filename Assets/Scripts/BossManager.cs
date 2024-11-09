using System;
using UnityEngine;

public class BossManager : MonoBehaviour
{
	public void Init()
	{
	}

	public void CreateBoss()
	{
		if (this.Boss)
		{
			EBoss boss = this.Boss.boss;
			if (boss != EBoss.Tenguka)
			{
				if (boss != EBoss.Heavy_Mech)
				{
					if (boss == EBoss.U_F_O)
					{
						this.Boss.gameObject.transform.position = CameraController.Instance.transform.position + new Vector3(0f, 3f, 10f);
					}
				}
				else
				{
					this.Boss.gameObject.transform.position = new Vector3(CameraController.Instance.Position.x + CameraController.Instance.Size().x + 2f, 0f);
				}
			}
			else
			{
				this.Boss.gameObject.transform.position = new Vector3(CameraController.Instance.Position.x + CameraController.Instance.Size().x + 2f, 2f, 0f);
			}
			this.Boss.Init();
			return;
		}
		EventDispatcher.PostEvent("CompletedGame");
	}

	public void ShowLineBloodBoss(float hpCurrent, float HP)
	{
		GameManager.Instance.hudManager.LineBlood.gameObject.SetActive(true);
		float num = HP - hpCurrent;
		if (num <= HP * 0.333333343f)
		{
			float value = (HP * 0.333333343f - num) / (HP * 0.333333343f);
			GameManager.Instance.hudManager.arrLineBloodBoss[0].fillAmount = Mathf.Clamp01(value);
			return;
		}
		if (num > HP * 0.333333343f && num <= HP * 0.6666667f)
		{
			GameManager.Instance.hudManager.arrLineBloodBoss[0].fillAmount = 0f;
			float value2 = (HP * 0.6666667f - num) / (HP * 0.333333343f);
			GameManager.Instance.hudManager.arrLineBloodBoss[1].fillAmount = Mathf.Clamp01(value2);
			return;
		}
		if (num > HP * 0.6666667f && num <= HP * 1f)
		{
			GameManager.Instance.hudManager.arrLineBloodBoss[0].fillAmount = 0f;
			GameManager.Instance.hudManager.arrLineBloodBoss[1].fillAmount = 0f;
			float value3 = (HP * 1f - num) / (HP * 0.333333343f);
			GameManager.Instance.hudManager.arrLineBloodBoss[2].fillAmount = Mathf.Clamp01(value3);
			return;
		}
	}

	[Header("Boss:")]
	public BaseBoss Boss;

	[Header("Old:")]
	public Boss3_2 Boss32;

	public Transform[] Points_Boss3_2;
}
