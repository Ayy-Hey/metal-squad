using System;
using UnityEngine;

public class TutorialCharacter : MonoBehaviour
{
	private void OnEnable()
	{
		this.Step[0].SetActive(true);
	}

	public void ShowStep1()
	{
		this.Step[0].SetActive(false);
		this.Step[1].SetActive(true);
	}

	[SerializeField]
	private GameObject[] Step;
}
