using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseTutorial : MonoBehaviour
{
	private void OnEnable()
	{
		this.StepObj[0].SetActive(true);
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
		{
			this.NextStep();
			return;
		}
	}

	public void NextStep()
	{
		if (this.completed)
		{
			return;
		}
		this.Step++;
		if (this.Step >= this.StepObj.Length)
		{
			for (int i = 0; i < this.StepObj.Length; i++)
			{
				this.StepObj[i].SetActive(false);
			}
			LeanTween.value(this.background.gameObject, this.background.color, new Color(this.background.color.r, this.background.color.g, this.background.color.b, 0f), 1f).setOnUpdate(delegate(Color value)
			{
				this.background.color = value;
			}).setOnComplete(delegate(object callback)
			{
				this.completed = false;
				base.gameObject.SetActive(false);
			});
			return;
		}
		for (int j = 0; j < this.StepObj.Length; j++)
		{
			this.StepObj[j].SetActive(j == this.Step);
		}
	}

	[SerializeField]
	private GameObject[] StepObj;

	private int Step;

	private bool completed;

	[SerializeField]
	private Image background;
}
