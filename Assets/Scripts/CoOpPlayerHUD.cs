using System;
using UnityEngine;
using UnityEngine.UI;

public class CoOpPlayerHUD : MonoBehaviour
{
	public void Init(int charactorId, float MaxHP)
	{
		this.targetHUD.SetActive(true);
		this.SetAvatar(charactorId);
		this.MaxHP = MaxHP;
		this.slideHp.value = 1f;
		this.IsInit = true;
	}

	public void DeActive()
	{
		this.targetHUD.SetActive(false);
	}

	private void SetAvatar(int charactorId)
	{
		for (int i = 0; i < this.avatar.Length; i++)
		{
			this.avatar[i].gameObject.SetActive(charactorId == i);
		}
	}

	public void ChangeHPSlide(float HPCurrent)
	{
		this.slideHp.value = HPCurrent / this.MaxHP;
	}

	public GameObject targetHUD;

	public Image[] avatar;

	public Slider slideHp;

	public float MaxHP;

	public bool IsInit;
}
